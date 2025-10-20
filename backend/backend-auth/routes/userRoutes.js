// backend/backend-auth/routes/userRoutes.js
import express from "express";
import User from "../models/User.js";
import { protect, adminOnly, checkRole } from "../middleware/authMiddleware.js";

const router = express.Router();

// ✅ Lấy danh sách user (chỉ Admin)
router.get("/", protect, adminOnly, async (req, res) => {
  try {
    const users = await User.find().populate("role").select("-password");
    res.json(users);
  } catch (err) {
    res.status(500).json({ message: "Lỗi server" });
  }
});

// ✅ Xóa user (Admin hoặc chính user đó)
router.delete("/:id", protect, async (req, res) => {
  try {
    const user = await User.findById(req.params.id);
    if (!user) return res.status(404).json({ message: "Không tìm thấy user" });

    if (
      req.user.role.name.toLowerCase() === "admin" ||
      req.user._id.toString() === user._id.toString()
    ) {
      await user.deleteOne();
      res.json({ message: "Đã xóa tài khoản!" });
    } else {
      res.status(403).json({ message: "Bạn không có quyền xóa tài khoản này!" });
    }
  } catch (err) {
    res.status(500).json({ message: "Lỗi server" });
  }
});

// ✅ Test RBAC nâng cao
router.get("/test/admin", protect, checkRole("Admin"), (req, res) => {
  res.json({ message: "Xin chào Admin!" });
});

router.get("/test/moderator", protect, checkRole("Admin", "Editor"), (req, res) => {
  res.json({ message: "Xin chào Editor hoặc Admin!" });
});

router.get("/test/user", protect, checkRole("User", "Editor", "Admin"), (req, res) => {
  res.json({ message: "Xin chào User, Editor hoặc Admin!" });
});

export default router;
