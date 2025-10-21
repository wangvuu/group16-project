import express from "express";
import User from "../models/User.js";
import { protect, adminOnly } from "../middleware/authMiddleware.js";
import multer from "multer";
import sharp from "sharp";
import cloudinary from "../utils/cloudinary.js";

const router = express.Router();

// =========================
// 📸 Upload Avatar (JWT cần thiết)
// =========================

// Cấu hình Multer: lưu ảnh tạm trong bộ nhớ RAM (không ghi file tạm)
const storage = multer.memoryStorage();
const upload = multer({ storage });

// POST /api/users/avatar
router.post("/avatar", protect, upload.single("avatar"), async (req, res) => {
  try {
    if (!req.file) {
      return res.status(400).json({ message: "Vui lòng chọn ảnh" });
    }

    // Resize ảnh bằng Sharp
    const buffer = await sharp(req.file.buffer)
      .resize(300, 300)
      .jpeg({ quality: 80 })
      .toBuffer();

    // Upload ảnh lên Cloudinary
    const uploadResult = await new Promise((resolve, reject) => {
      const stream = cloudinary.uploader.upload_stream(
        { folder: "avatars" },
        (error, result) => {
          if (error) reject(error);
          else resolve(result);
        }
      );
      stream.end(buffer);
    });

    // Cập nhật avatar trong MongoDB
    const user = await User.findById(req.user._id);
    if (!user) return res.status(404).json({ message: "Không tìm thấy user" });

    user.avatar = uploadResult.secure_url;
    await user.save();

    res.status(200).json({
      message: "Upload avatar thành công",
      avatar: uploadResult.secure_url,
    });
  } catch (error) {
    console.error("Lỗi upload avatar:", error);
    res.status(500).json({ message: "Lỗi server khi upload ảnh" });
  }
});

// =========================
// 👤 Lấy danh sách user (Admin)
// =========================
router.get("/", protect, adminOnly, async (req, res) => {
  try {
    const users = await User.find().select("-password");
    res.json(users);
  } catch (err) {
    res.status(500).json({ message: "Lỗi server" });
  }
});

// =========================
// ❌ Xóa user (Admin hoặc chính user đó)
// =========================
router.delete("/:id", protect, async (req, res) => {
  try {
    const user = await User.findById(req.params.id);
    if (!user) return res.status(404).json({ message: "Không tìm thấy user" });

    if (req.user.role === "admin" || req.user._id.toString() === user._id.toString()) {
      await user.deleteOne();
      res.json({ message: "Đã xóa tài khoản!" });
    } else {
      res.status(403).json({ message: "Bạn không có quyền xóa tài khoản này!" });
    }
  } catch (err) {
    res.status(500).json({ message: "Lỗi server" });
  }
});

export default router;
