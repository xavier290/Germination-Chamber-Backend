// backend/routes/parametersRoutes.js
const express = require('express');
const router = express.Router();
const { updateParameters } = require('../controllers/parametersController');

// Update Parameters
router.put('/:id', updateParameters);

module.exports = router;