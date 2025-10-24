import Log from "../models/Log.js";
import jwt from "jsonwebtoken";
import User from "../models/User.js";

export const logActivity = async (req, res, next) => {
  try {
    // ⚡ Bỏ qua các route đặc biệt (đã tự ghi log thủ công trong controller)
    const skipPaths = [
      "/api/auth/login",
      "/api/auth/signup",
      "/api/auth/logout",
      "/api/auth/refresh",
    ];

    if (skipPaths.some((path) => req.path.startsWith(path))) {
      return next();
    }

    let userId = null;
    let email = "Guest";

    // ✅ Nếu có token thì giải mã để biết user nào
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

    // ✅ Ghi log (bỏ qua các request favicon, static)
    if (!req.path.includes("favicon.ico")) {
      await Log.create({
        userId,
        action: `${req.method} ${req.originalUrl}`,
      });
    }

    next();
  } catch (err) {
    console.error("⚠️ Lỗi ghi log:", err.message);
    next();
  }
};
