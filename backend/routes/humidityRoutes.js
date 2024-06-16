const express = require('express');
const router = express.Router();
const {
    getAllHumidity,
    getLatestHumidity,
    getLastHourHumidity,
    getLast8HoursHumidity,
    getLastDayHumidity,
    getLastWeekHumidity,
    getLastMonthHumidity
} = require('../controllers/humidityController');

router.get('/', getAllHumidity);
router.get('/latest', getLatestHumidity);
router.get('/last-hour', getLastHourHumidity);
router.get('/last-8-hours', getLast8HoursHumidity);
router.get('/last-day', getLastDayHumidity);
router.get('/last-week', getLastWeekHumidity);
router.get('/last-month', getLastMonthHumidity);

module.exports = router;