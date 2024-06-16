const mongoose = require('mongoose');

const temperatureSchema = new mongoose.Schema({
    Topic: { type: String, required: true },
    Payload: { type: Number, required: true },
    Timestamp: { type: Date, required: true }
}, { collection: 'Temperature' });

const Temperature = mongoose.model('Temperature', temperatureSchema);

module.exports = Temperature;
