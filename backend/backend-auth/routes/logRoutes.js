import express from "express";
import { getAllLogs } from "../controllers/logController.js";
import { verifyToken, isAdmin } from "../middleware/authMiddleware.js";
import Log from "../models/Log.js"; // 🆕 thêm để có thể mở rộng route trực tiếp

const router = express.Router();

// ✅ Route gốc - dùng controller
router.get("/", verifyToken, isAdmin, getAllLogs);

// 🆕 Bổ sung thêm route phụ để kiểm tra nhanh log có populate
router.get("/with-user", verifyToken, isAdmin, async (req, res) => {
  try {
    const logs = await Log.find()
      .populate("userId", "name email role") // 🧩 lấy cả name, email, role
      .sort({ createdAt: -1 });

    res.json(logs);
  } catch (err) {
    console.error("❌ Lỗi khi lấy logs:", err);
    res.status(500).json({ message: "Lỗi server khi lấy logs" });
  }
});

export default router;
