// controllers/userController.js
const User = require("../models/User");

// [GET] /users
const getUsers = async (req, res) => {
  try {
    const users = await User.find();
    res.status(200).json(users);
  } catch (err) {
    res.status(500).json({
      message: "❌ Lỗi khi lấy danh sách user",
      error: err.message,
    });
  }
};

// [POST] /users
const addUser = async (req, res) => {
  try {
    const { name, email } = req.body;

    if (!name || !email) {
      return res
        .status(400)
        .json({ message: "⚠️ Vui lòng nhập đầy đủ name và email" });
    }

    const newUser = new User({ name, email });
    await newUser.save();

    res.status(201).json({
      message: "✅ Thêm user thành công",
      user: newUser,
    });
  } catch (err) {
    if (err.code === 11000) {
      return res.status(400).json({ message: "⚠️ Email đã tồn tại" });
    }
    res.status(500).json({
      message: "❌ Lỗi khi thêm user",
      error: err.message,
    });
  }
};

module.exports = { getUsers, addUser };