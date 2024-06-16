const express = require('express');
const router = express.Router();
const {
    getAllTemperatures,
    getLatestTemperature,
    getLastHourTemperatures,
    getLast8HoursTemperatures,
    getLastDayTemperatures,
    getLastWeekTemperatures,
    getLastMonthTemperatures
} = require('../controllers/temperatureController');

// Route to get all temperatures
router.get('/', getAllTemperatures);

// Route to get the latest temperature
router.get('/latest', getLatestTemperature);

// Route to get temperatures from the last hour
router.get('/last-hour', getLastHourTemperatures);

// Route to get temperatures from the last 8 hours
router.get('/last-8-hours', getLast8HoursTemperatures);

// Route to get temperatures from the last day
router.get('/last-day', getLastDayTemperatures);

// Route to get temperatures from the last week
router.get('/last-week', getLastWeekTemperatures);

// Route to get temperatures from the last month
router.get('/last-month', getLastMonthTemperatures);

module.exports = router;