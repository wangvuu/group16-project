// backend/backend-auth/middleware/authMiddleware.js
import jwt from "jsonwebtoken";
import User from "../models/User.js";
import Role from "../models/role.js";

// ✅ Middleware xác thực người dùng
export const protect = async (req, res, next) => {
  let token;

  if (req.headers.authorization?.startsWith("Bearer")) {
    try {
      token = req.headers.authorization.split(" ")[1];
      const decoded = jwt.verify(
        token,
        process.env.ACCESS_TOKEN_SECRET || "access_secret_key"
      );

      const user = await User.findById(decoded.id).populate("role");
      if (!user) return res.status(404).json({ message: "Không tìm thấy người dùng!" });

      req.user = user;
      next();
    } catch (error) {
      console.error("❌ Token error:", error);
      return res.status(401).json({ message: "Token không hợp lệ hoặc hết hạn!" });
    }
  } else {
    return res.status(401).json({ message: "Không có token xác thực!" });
  }
};

// ✅ Middleware chỉ cho phép Admin
export const adminOnly = (req, res, next) => {
  if (req.user?.role?.name?.toLowerCase() === "admin") {
    next();
  } else {
    return res.status(403).json({ message: "Chỉ admin được phép!" });
  }
};

// ✅ Middleware phân quyền nâng cao (RBAC)
export const checkRole = (...roles) => {
  return (req, res, next) => {
    if (!req.user)
      return res.status(401).json({ message: "Chưa xác thực người dùng!" });

    const userRole = req.user.role?.name?.toLowerCase();
    const allowed = roles.map((r) => r.toLowerCase());
    if (!allowed.includes(userRole)) {
      return res
        .status(403)
        .json({ message: `Không có quyền truy cập! Yêu cầu vai trò: ${roles.join(", ")}` });
    }

    next();
  };
};
