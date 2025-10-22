import express from "express";
import bcrypt from "bcryptjs";
import jwt from "jsonwebtoken";
import { body, validationResult } from "express-validator";
import User from "../models/User.js";
import RefreshToken from "../models/RefreshToken.js";
import Log from "../models/Log.js"; // 📝 Ghi log hoạt động
import { loginRateLimiter } from "../middleware/rateLimit.js";

const router = express.Router();

/* =======================
   🧾 POST /signup
======================= */
router.post(
  "/signup",
  [
    body("name").notEmpty().withMessage("Tên không được để trống"),
    body("email").isEmail().withMessage("Email không hợp lệ"),
    body("password").isLength({ min: 6 }).withMessage("Mật khẩu tối thiểu 6 ký tự"),
  ],
  async (req, res) => {
    const errors = validationResult(req);
    if (!errors.isEmpty()) return res.status(400).json({ errors: errors.array() });

    const { name, email, password, role } = req.body;

    try {
      const existingUser = await User.findOne({ email });
      if (existingUser)
        return res.status(400).json({ message: "Email đã tồn tại!" });

      const validRoles = ["admin", "moderator", "user"];
      const assignedRole = validRoles.includes(role) ? role : "user";

      const hashedPassword = await bcrypt.hash(password, 10);

      const newUser = new User({
        name,
        email,
        password: hashedPassword,
        role: assignedRole,
      });

      await newUser.save();

      // 📝 Ghi log đăng ký
      await Log.create({
        userId: newUser._id,
        action: `🆕 Đăng ký tài khoản mới (${email})`,
      });

      res.status(201).json({
        message: `Đăng ký thành công với vai trò: ${assignedRole}`,
      });
    } catch (err) {
      console.error("🔥 Lỗi signup:", err);
      res.status(500).json({ message: "Lỗi server" });
    }
  }
);

/* =======================
   🔐 POST /login (Rate Limit + Log)
======================= */
router.post("/login", loginRateLimiter, async (req, res) => {
  const { email, password } = req.body;

  try {
    const user = await User.findOne({ email });
    if (!user) {
      await Log.create({
        action: `❌ Đăng nhập thất bại: email không tồn tại (${email})`,
      });
      return res.status(400).json({ message: "Email không tồn tại" });
    }

    const isMatch = await bcrypt.compare(password, user.password);
    if (!isMatch) {
      await Log.create({
        userId: user._id,
        action: `❌ Sai mật khẩu khi đăng nhập (${email})`,
      });
      return res.status(400).json({ message: "Sai mật khẩu" });
    }

    // 🧾 Xóa refresh token cũ để tránh trùng
    await RefreshToken.deleteMany({ user: user._id });

    // 🔑 Tạo Access Token & Refresh Token mới
    const accessToken = jwt.sign(
      { id: user._id, email: user.email, role: user.role },
      process.env.ACCESS_TOKEN_SECRET || "access_secret_key",
      { expiresIn: "15m" }
    );

    const refreshToken = jwt.sign(
      { id: user._id, email: user.email, role: user.role },
      process.env.REFRESH_TOKEN_SECRET || "refresh_secret_key",
      { expiresIn: "7d" }
    );

    // 💾 Lưu refresh token
    await RefreshToken.create({
      user: user._id,
      token: refreshToken,
      expiresAt: new Date(Date.now() + 7 * 24 * 60 * 60 * 1000),
    });

    // 📝 Ghi log đăng nhập thành công
    await Log.create({
      userId: user._id,
      action: `✅ Đăng nhập thành công (${email})`,
    });

    res.json({
      message: "Đăng nhập thành công!",
      accessToken,
      refreshToken,
      user: { name: user.name, email: user.email, role: user.role },
    });
  } catch (err) {
    console.error("🔥 Lỗi login:", err);
    res.status(500).json({ message: "Lỗi server" });
  }
});

/* =======================
   🔁 POST /refresh
======================= */
router.post("/refresh", async (req, res) => {
  const { refreshToken } = req.body;
  if (!refreshToken)
    return res.status(400).json({ message: "Thiếu refresh token" });

  try {
    const storedToken = await RefreshToken.findOne({
      token: refreshToken,
      revoked: false,
    });
    if (!storedToken)
      return res
        .status(403)
        .json({ message: "Refresh token không hợp lệ hoặc đã bị thu hồi" });

    jwt.verify(
      refreshToken,
      process.env.REFRESH_TOKEN_SECRET || "refresh_secret_key",
      async (err, user) => {
        if (err)
          return res
            .status(403)
            .json({ message: "Refresh token hết hạn hoặc sai" });

        const newAccessToken = jwt.sign(
          { id: user.id, email: user.email, role: user.role },
          process.env.ACCESS_TOKEN_SECRET || "access_secret_key",
          { expiresIn: "15m" }
        );

        await Log.create({
          userId: user.id,
          action: `🔁 Làm mới access token (${user.email})`,
        });

        res.json({
          message: "Tạo access token mới thành công!",
          accessToken: newAccessToken,
        });
      }
    );
  } catch (err) {
    console.error("🔥 Lỗi refresh:", err);
    res.status(500).json({ message: "Lỗi server" });
  }
});

/* =======================
   🚪 POST /logout
======================= */
router.post("/logout", async (req, res) => {
  const { refreshToken } = req.body;

  try {
    if (refreshToken) {
      const tokenDoc = await RefreshToken.findOneAndUpdate(
        { token: refreshToken },
        { revoked: true }
      );

      if (tokenDoc) {
        await Log.create({
          userId: tokenDoc.user,
          action: "🚪 Người dùng đăng xuất và thu hồi refresh token",
        });
      }
    }

    res.json({ message: "Đăng xuất thành công!" });
  } catch (err) {
    console.error("🔥 Lỗi logout:", err);
    res.status(500).json({ message: "Lỗi server" });
  }
});

export default router;
