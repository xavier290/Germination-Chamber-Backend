// backend/controllers/parametersController.js
const Parameters = require('../models/parametersModel');

// @desc    Update parameters
// @route   PUT /api/parameters/:id
// @access  Public
const updateParameters = async (req, res) => {
  try {
    const updatedParameters = await Parameters.findByIdAndUpdate(
      req.params.id,
      req.body,
      { new: true }
    );

    if (!updatedParameters) {
      return res.status(404).json({ message: 'Parameters not found' });
    }

    res.status(200).json(updatedParameters);
  } catch (error) {
    res.status(500).json({ message: 'Failed to update parameters', error });
  }
};

module.exports = { updateParameters };