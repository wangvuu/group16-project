// backend/server.js
const express = require('express');
const mongoose = require('mongoose');
const dotenv = require('dotenv');
const cors = require('cors');

// Import routes
const authRoutes = require('./routes/auth');
const profileRoutes = require('./routes/profile'); // ✅ thêm dòng này

dotenv.config();

const app = express();

// Middleware
app.use(cors());
app.use(express.json());

// ✅ Kết nối MongoDB Atlas
mongoose.connect(process.env.MONGODB_URI)
  .then(() => console.log('✅ Kết nối MongoDB Atlas thành công'))
  .catch(err => console.error('❌ Lỗi kết nối MongoDB:', err));

// ✅ Routes chính
app.use('/api/auth', authRoutes);
app.use('/api/profile', profileRoutes); // ✅ mount route profile

// ✅ Khởi động server
const PORT = process.env.PORT || 5000;
app.listen(PORT, () => console.log(`🚀 Server chạy tại cổng ${PORT}`));
