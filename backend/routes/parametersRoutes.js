// backend/routes/parametersRoutes.js
const express = require('express');
const router = express.Router();
const { updateParameters, getParameters } = require('../controllers/parametersController');

// Update Parameters
router.put('/:id', updateParameters);
router.get('/info/:id', getParameters);

module.exports = router;