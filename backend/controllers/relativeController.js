const asyncHandler = require('express-async-handler');
const Humidity = require('../models/relativeModel');

// @desc    Get all humidity data
// @route   GET /api/humidity
// @access  Public
const getAllRelative = asyncHandler(async (req, res) => {
    const humidityData = await Humidity.find();
    res.status(200).json(humidityData);
});

// @desc    Get latest humidity data
// @route   GET /api/humidity/latest
// @access  Public
const getLatestRelative = asyncHandler(async (req, res) => {
    const latestHumidity = await Humidity.findOne().sort({ Timestamp: -1 });
    res.status(200).json(latestHumidity);
});

// @desc    Get humidity data from the last hour
// @route   GET /api/humidity/last-hour
// @access  Public
const getLastHourRelative = asyncHandler(async (req, res) => {
    const oneHourAgo = new Date(new Date().getTime() - 60 * 60 * 1000);
    const humidityData = await Humidity.find({ Timestamp: { $gte: oneHourAgo } });
    res.status(200).json(humidityData);
});

// @desc    Get humidity data from the last 8 hours
// @route   GET /api/humidity/last-8-hours
// @access  Public
const getLast8HoursRelative = asyncHandler(async (req, res) => {
    const eightHoursAgo = new Date(new Date().getTime() - 8 * 60 * 60 * 1000);
    const humidityData = await Humidity.find({ Timestamp: { $gte: eightHoursAgo } });
    res.status(200).json(humidityData);
});

// @desc    Get humidity data from the last day
// @route   GET /api/humidity/last-day
// @access  Public
const getLastDayRelative = asyncHandler(async (req, res) => {
    const oneDayAgo = new Date(new Date().getTime() - 24 * 60 * 60 * 1000);
    const humidityData = await Humidity.find({ Timestamp: { $gte: oneDayAgo } });
    res.status(200).json(humidityData);
});

// @desc    Get humidity data from the last week
// @route   GET /api/humidity/last-week
// @access  Public
const getLastWeekRelative = asyncHandler(async (req, res) => {
    const oneWeekAgo = new Date(new Date().getTime() - 7 * 24 * 60 * 60 * 1000);
    const humidityData = await Humidity.find({ Timestamp: { $gte: oneWeekAgo } });
    res.status(200).json(humidityData);
});

// @desc    Get humidity data from the last month
// @route   GET /api/humidity/last-month
// @access  Public
const getLastMonthRelative = asyncHandler(async (req, res) => {
    const oneMonthAgo = new Date(new Date().setMonth(new Date().getMonth() - 1));
    const humidityData = await Humidity.find({ Timestamp: { $gte: oneMonthAgo } });
    res.status(200).json(humidityData);
});

module.exports = {
    getLatestRelative,
    getLastHourRelative,
    getLast8HoursRelative,
    getLastDayRelative,
    getLastWeekRelative,
    getLastMonthRelative,
    getAllRelative
};