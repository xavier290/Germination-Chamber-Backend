const mongoose = require('mongoose');

const userSchema = new mongoose.Schema({
    username: { type: String, unique: true },
    email: { type: String, unique: true },
    password: String, // Hashed and salted password
});

const User = mongoose.model('User', userSchema);

module.exports = User;