const jwt = require('jsonwebtoken');
const bcrypt = require('bcryptjs');
const asyncHandler = require("express-async-handler");

const User = require('../models/userModel');

// Generate JWT
const generateToken = (id) => {
    return jwt.sign({ id }, process.env.SECRET, {
        expiresIn: '1d',
    });
}

// @desc Get user data
// @route GET /api/auth/me
// @access Private
const getMe = asyncHandler(async (req, res) => {
    res.status(200).json(req.user);
});

// @desc Authenticate a user
// @route POST /api/auth/login
// @access Public
const loginUser = asyncHandler(async (req, res) => {
    const { username, email, password } = req.body;

    // Find user by username or email
    const user = await User.findOne({ 
        $or: [{ username }, { email }]
    });

    if (!user) {
        return res.status(403).json({ message: 'Invalid Credentials.' });
    }

    // Check if password matches
    const passwordMatch = await bcrypt.compare(password, user.password);

    if (passwordMatch) {
        res.json({
            _id: user.id,
            name: user.username,
            token: generateToken(user._id),
        });
    } else {
        return res.status(400).json({ message: 'Invalid Credentials.' });
    }
});

// @desc Register a new user
// @route POST /api/auth/register
// @access Public
const signupUser = asyncHandler(async (req, res) => {
    const { username, email, password } = req.body;

    // Check if username or email already exists
    const existingUser = await User.findOne({ 
        $or: [{ username }, { email }]
    });

    if (existingUser) {
        return res.status(400).json({ message: 'User with this username or email already exists.' });
    }

    // Hash the password
    const hashedPassword = await bcrypt.hash(password, 10);

    // Create new user
    const newUser = new User({
        username,
        email,
        password: hashedPassword,
    });

    await newUser.save();

    // Generate token
    const token = generateToken(newUser._id);

    // Send the user and token in the response
    res.status(201).json({ token });
});

module.exports = { loginUser, signupUser, getMe };