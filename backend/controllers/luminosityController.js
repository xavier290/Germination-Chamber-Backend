const asyncHandler = require('express-async-handler');
const Luminosity = require('../models/luminosityModel');

// @desc    Get latest luminosity data
// @route   GET /api/luminosity/latest
// @access  Public
const getLatestLuminosity = asyncHandler(async (req, res) => {
    const latestLuminosity = await Luminosity.findOne().sort({ Timestamp: -1 });
    res.status(200).json(latestLuminosity);
});

module.exports = { getLatestLuminosity };
