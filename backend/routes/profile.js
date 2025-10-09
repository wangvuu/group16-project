// routes/profile.js
const express = require('express');
const router = express.Router();
const User = require('../models/User');
const authMiddleware = require('../middleware/authMiddleware');

// GET /api/profile  -> xem profile hiện tại
router.get('/', authMiddleware, async (req, res) => {
  try {
    // req.user là object đã select('-passwordHash') từ middleware
    res.json({ user: req.user });
  } catch (error) {
    res.status(500).json({ message: 'Lỗi khi lấy thông tin người dùng' });
  }
});

// PUT /api/profile  -> cập nhật profile (name, email, avatarUrl...)
router.put('/', authMiddleware, async (req, res) => {
  try {
    const userId = req.user._id;
    const { name, email, avatarUrl } = req.body;

    // nếu thay đổi email -> kiểm tra trùng
    if (email && email !== req.user.email) {
      const exist = await User.findOne({ email });
      if (exist) return res.status(400).json({ message: 'Email đã được sử dụng' });
    }

    const update = {};
    if (name !== undefined) update.name = name;
    if (email !== undefined) update.email = email;
    if (avatarUrl !== undefined) update.avatarUrl = avatarUrl;

    // cập nhật và trả về document mới (không có passwordHash)
    const updatedUser = await User.findByIdAndUpdate(userId, update, { new: true }).select('-passwordHash');
    res.json({ message: 'Cập nhật thành công', user: updatedUser });
  } catch (error) {
    console.error(error);
    res.status(500).json({ message: 'Lỗi khi cập nhật thông tin' });
  }
});

module.exports = router;
