const express = require('express');

const {loginUser, signupUser} = require('../controllers/userController')

const router = express.Router();

router.post('/login', loginUser);

// Route to register a new user
router.post('/register', signupUser);

module.exports = router;