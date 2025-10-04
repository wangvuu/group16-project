const express = require('express');
const router = express.Router();
const userController = require('../controllers/userController');

// Định nghĩa route
router.get('/users', userController.getUsers);
router.post('/users', userController.createUser);

module.exports = router;
