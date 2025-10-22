// ✅ Đặt dotenv ở ngay DÒNG ĐẦU TIÊN
import dotenv from "dotenv";
dotenv.config();

import express from "express";
import cors from "cors";
import connectDB from "./config/db.js";
import authRoutes from "./routes/authRoutes.js";
import profileRoutes from "./routes/profileRoutes.js";
import userRoutes from "./routes/userRoutes.js";

// 🆕 Thêm import mới cho hoạt động 5
import logRoutes from "./routes/logRoutes.js";
import { logActivity } from "./middleware/logMiddleware.js";

const app = express();
app.use(cors());
app.use(express.json());

// Kết nối MongoDB
connectDB();

// 🆕 Middleware ghi log cho tất cả request
app.use(logActivity);

// Định tuyến
app.use("/api/auth", authRoutes);
app.use("/api/profile", profileRoutes);
app.use("/api/users", userRoutes);
app.use("/api/logs", logRoutes); // 🆕 route xem log cho admin

// Chạy server
const PORT = process.env.PORT || 5000;
app.listen(PORT, () => {
  console.log(`🚀 Server running on port ${PORT}`);
  console.log("✅ MONGO_URI:", process.env.MONGO_URI ? "Đã load" : "❌ Chưa load");
  console.log("✅ CLOUD_NAME:", process.env.CLOUDINARY_NAME);
  console.log("✅ CLOUD_KEY:", process.env.CLOUDINARY_API_KEY);
  console.log("✅ CLOUD_SECRET:", process.env.CLOUDINARY_API_SECRET ? "Đã có" : "❌ Rỗng");
});
