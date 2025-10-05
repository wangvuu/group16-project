// server.js
const express = require('express');
const mongoose = require('mongoose');
const dotenv = require('dotenv');
const cors = require('cors');
const userRoutes = require('./routes/user');

dotenv.config();

const app = express();
app.use(cors());
app.use(express.json());

// Kết nối MongoDB Atlas
mongoose.connect(process.env.MONGODB_URI, { })
  .then(() => console.log('✅ Kết nối MongoDB Atlas thành công'))
  .catch(err => console.error('❌ Lỗi kết nối MongoDB:', err));

// Routes
app.use('/', userRoutes);

// Khởi động server
const PORT = process.env.PORT || 5000;
app.listen(PORT, () => console.log(`🚀 Server running on port ${PORT}`));
