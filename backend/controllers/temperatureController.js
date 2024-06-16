const asyncHandler = require('express-async-handler');
const Temperature = require('../models/temperatureModel');

// Get all temperature data
const getAllTemperatures = asyncHandler(async (req, res) => {
    const temperatures = await Temperature.find({});
    res.json(temperatures);
});

// Get the latest temperature data
const getLatestTemperature = asyncHandler(async (req, res) => {
    const latestTemperature = await Temperature.findOne().sort({ Timestamp: -1 });
    res.json(latestTemperature);
});

// Get temperature data from the last hour
const getLastHourTemperatures = asyncHandler(async (req, res) => {
    const oneHourAgo = new Date(Date.now() - 60 * 60 * 1000);
    const temperatures = await Temperature.find({ Timestamp: { $gte: oneHourAgo } });
    res.json(temperatures);
});

// Get temperature data from the last 8 hours
const getLast8HoursTemperatures = asyncHandler(async (req, res) => {
    const eightHoursAgo = new Date(Date.now() - 8 * 60 * 60 * 1000);
    const temperatures = await Temperature.find({ Timestamp: { $gte: eightHoursAgo } });
    res.json(temperatures);
});

// Get temperature data from the last day
const getLastDayTemperatures = asyncHandler(async (req, res) => {
    const oneDayAgo = new Date(Date.now() - 24 * 60 * 60 * 1000);
    const temperatures = await Temperature.find({ Timestamp: { $gte: oneDayAgo } });
    res.json(temperatures);
});

// Get temperature data from the last week
const getLastWeekTemperatures = asyncHandler(async (req, res) => {
    const oneWeekAgo = new Date(Date.now() - 7 * 24 * 60 * 60 * 1000);
    const temperatures = await Temperature.find({ Timestamp: { $gte: oneWeekAgo } });
    res.json(temperatures);
});

// Get temperature data from the last month
const getLastMonthTemperatures = asyncHandler(async (req, res) => {
    const oneMonthAgo = new Date();
    oneMonthAgo.setMonth(oneMonthAgo.getMonth() - 1);
    const temperatures = await Temperature.find({ Timestamp: { $gte: oneMonthAgo } });
    res.json(temperatures);
});

module.exports = {
    getAllTemperatures,
    getLatestTemperature,
    getLastHourTemperatures,
    getLast8HoursTemperatures,
    getLastDayTemperatures,
    getLastWeekTemperatures,
    getLastMonthTemperatures
};