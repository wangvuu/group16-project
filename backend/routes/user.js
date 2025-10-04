// routes/user.js
const express = require("express");
const router = express.Router();
const userController = require("../controllers/userController");

console.log("👉 userController:", userController);

// API lấy danh sách user
router.get("/users", userController.getUsers);

// API thêm user mới
router.post("/users", userController.addUser);

module.exports = router;