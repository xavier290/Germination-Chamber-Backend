const mongoose = require('mongoose');

const humiditySchema = new mongoose.Schema({
    Topic: { type: String, required: true },
    Payload: { type: Number, required: true },
    Timestamp: { type: Date, required: true }
}, { collection: 'Humidity' }); // Specify the exact collection name here

const Humidity = mongoose.model('Humidity', humiditySchema);

module.exports = Humidity;