// backend/backend-auth/routes/authRoutes.js
import express from "express";
import bcrypt from "bcryptjs";
import jwt from "jsonwebtoken";
import { body, validationResult } from "express-validator";
import User from "../models/User.js";
import Role from "../models/role.js";
import RefreshToken from "../models/RefreshToken.js";

const router = express.Router();

/* =======================
   POST /signup
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
      if (existingUser) return res.status(400).json({ message: "Email đã tồn tại!" });

      const selectedRole =
        (await Role.findOne({ name: role?.toLowerCase() })) ||
        (await Role.findOne({ name: "user" }));

      const hashedPassword = await bcrypt.hash(password, 10);
      const newUser = new User({
        name,
        email,
        password: hashedPassword,
        role: selectedRole._id,
      });

      await newUser.save();
      res.status(201).json({ message: `Đăng ký thành công với vai trò: ${selectedRole.name}` });
    } catch (err) {
      console.error(err);
      res.status(500).json({ message: "Lỗi server" });
    }
  }
);

/* =======================
   POST /login
======================= */
router.post("/login", async (req, res) => {
  const { email, password } = req.body;

  try {
    const user = await User.findOne({ email }).populate("role");
    if (!user) return res.status(400).json({ message: "Email không tồn tại" });

    const isMatch = await bcrypt.compare(password, user.password);
    if (!isMatch) return res.status(400).json({ message: "Sai mật khẩu" });

    const payload = {
      id: user._id,
      email: user.email,
      role: user.role.name,
    };

    const accessToken = jwt.sign(payload, process.env.ACCESS_TOKEN_SECRET || "access_secret_key", {
      expiresIn: "15m",
    });

    const refreshToken = jwt.sign(payload, process.env.REFRESH_TOKEN_SECRET || "refresh_secret_key", {
      expiresIn: "7d",
    });

    await RefreshToken.create({
      user: user._id,
      token: refreshToken,
      expiresAt: new Date(Date.now() + 7 * 24 * 60 * 60 * 1000),
    });

    res.json({
      message: "Đăng nhập thành công!",
      accessToken,
      refreshToken,
      user: { name: user.name, email: user.email, role: user.role.name },
    });
  } catch (err) {
    console.error(err);
    res.status(500).json({ message: "Lỗi server" });
  }
});

export default router;
