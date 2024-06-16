const mongoose = require('mongoose');

const parametersSchema = new mongoose.Schema({
  MaxTemperature: { type: String },
  MinTemperature: { type: String },
  MaxHumidity: { type: String },
  MinHumidity: { type: String },
  HoursLuminosity: { type: String }
}, { collection: 'Parameters'});

module.exports = mongoose.model('Parameters', parametersSchema);