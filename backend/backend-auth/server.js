// ✅ Đặt dotenv ở ngay DÒNG ĐẦU TIÊN
import dotenv from "dotenv";
dotenv.config();

import express from "express";
import cors from "cors";
import connectDB from "./config/db.js";

import authRoutes from "./routes/authRoutes.js";
import profileRoutes from "./routes/profileRoutes.js";
import userRoutes from "./routes/userRoutes.js";
import logRoutes from "./routes/logRoutes.js";
import { logActivity } from "./middleware/logMiddleware.js";

const app = express();
app.use(cors());
app.use(express.json());

// ✅ Kết nối MongoDB
connectDB();

// ⚙️ Gắn middleware log cho các route cần thiết (không log toàn bộ)
app.use("/api/profile", logActivity, profileRoutes);
app.use("/api/users", logActivity, userRoutes);

// ⚙️ Route auth (đã tự log trong controller)
app.use("/api/auth", authRoutes);

// ⚙️ Route xem log cho admin
app.use("/api/logs", logRoutes);

// ✅ Chạy server
const PORT = process.env.PORT || 5000;
app.listen(PORT, () => {
  console.log(`🚀 Server running on port ${PORT}`);
  console.log("✅ MONGO_URI:", process.env.MONGO_URI ? "Đã load" : "❌ Chưa load");
  console.log("✅ CLOUD_NAME:", process.env.CLOUDINARY_NAME);
});
