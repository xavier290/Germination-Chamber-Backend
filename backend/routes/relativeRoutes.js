const express = require('express');
const router = express.Router();
const {
    getLatestRelative,
    getLastHourRelative,
    getLast8HoursRelative,
    getLastDayRelative,
    getLastWeekRelative,
    getLastMonthRelative,
    getAllRelative
} = require('../controllers/relativeController');

router.get('/', getAllRelative);
router.get('/latest', getLatestRelative);
router.get('/last-hour', getLastHourRelative);
router.get('/last-8-hours', getLast8HoursRelative);
router.get('/last-day', getLastDayRelative);
router.get('/last-week', getLastWeekRelative);
router.get('/last-month', getLastMonthRelative);

module.exports = router;