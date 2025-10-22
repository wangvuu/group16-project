import Log from "../models/Log.js";
import jwt from "jsonwebtoken";
import User from "../models/User.js";

export const logActivity = async (req, res, next) => {
  try {
    let userId = null;
    let email = "Guest";

    // ✅ Lấy token nếu có
    if (req.headers.authorization?.startsWith("Bearer")) {
      const token = req.headers.authorization.split(" ")[1];
      const decoded = jwt.verify(
        token,
        process.env.ACCESS_TOKEN_SECRET || "access_secret_key"
      );

      const user = await User.findById(decoded.id);
      if (user) {
        userId = user._id;
        email = user.email;
      }
    }

    // ✅ Lưu log (vd: GET /api/users)
    await Log.create({
      userId,
      action: `${req.method} ${req.originalUrl}`,
    });

    next();
  } catch (err) {
    console.error("❌ Lỗi log activity:", err);
    next(); // không chặn request chính
  }
};
