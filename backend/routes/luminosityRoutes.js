const express = require('express');
const router = express.Router();
const { getLatestLuminosity } = require('../controllers/luminosityController');

router.get('/latest', getLatestLuminosity);

module.exports = router;