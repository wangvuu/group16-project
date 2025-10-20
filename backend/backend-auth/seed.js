// backend/backend-auth/seed.js
import mongoose from "mongoose";
import dotenv from "dotenv";
import bcrypt from "bcryptjs";
import { createRequire } from "module";
import User from "./models/User.js";

// 👇 Giữ nguyên Role ở dạng CommonJS
const require = createRequire(import.meta.url);
const Role = require("./models/role.js");

dotenv.config(); // Load biến môi trường
const MONGO_URI = process.env.MONGO_URI;

const seedData = async () => {
  try {
    // 🔗 Kết nối tới MongoDB và chọn DB "AuthDB"
    await mongoose.connect(MONGO_URI, { dbName: "AuthDB" });
    console.log("✅ Connected to MongoDB Atlas (AuthDB)");

    // 🧹 Xóa dữ liệu cũ
    await User.deleteMany();
    await Role.deleteMany();
    console.log("🧹 Cleared old Users and Roles");

    // 🧩 Tạo Role mẫu (Admin / Editor / User)
    const roles = await Role.insertMany([
      {
        name: "admin",
        description: "Toàn quyền quản trị hệ thống",
        permissions: ["create", "read", "update", "delete"],
      },
      {
        name: "editor",
        description: "Chỉnh sửa nội dung, cập nhật người dùng",
        permissions: ["read", "update"],
      },
      {
        name: "user",
        description: "Người dùng cơ bản, chỉ xem dữ liệu cá nhân",
        permissions: ["read"],
      },
    ]);
    console.log("✅ Roles created:", roles.map((r) => r.name).join(", "));

    // 🧠 Lấy role theo tên
    const adminRole = roles.find((r) => r.name === "admin");
    const editorRole = roles.find((r) => r.name === "editor");
    const userRole = roles.find((r) => r.name === "user");

    // 🔒 Hash mật khẩu mẫu
    const hashedPassword = await bcrypt.hash("123456", 10);

    // 👤 Tạo User mẫu
    await User.insertMany([
      {
        name: "Admin User",
        email: "admin@example.com",
        password: hashedPassword,
        role: adminRole._id,
      },
      {
        name: "Editor User",
        email: "editor@example.com",
        password: hashedPassword,
        role: editorRole._id,
      },
      {
        name: "Normal User",
        email: "user@example.com",
        password: hashedPassword,
        role: userRole._id,
      },
    ]);

    console.log("✅ Users created successfully");
    console.log("🎉 SEED COMPLETED SUCCESSFULLY!");
    process.exit(0);
  } catch (err) {
    console.error("❌ Seed error:", err.message);
    process.exit(1);
  }
};

seedData();