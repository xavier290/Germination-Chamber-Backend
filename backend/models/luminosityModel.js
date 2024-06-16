const mongoose = require('mongoose');

const luminositySchema = new mongoose.Schema({
    Topic: { type: String, required: true },
    Payload: { type: String, required: true }, // Assuming Payload is a string like "Light" or "Dark"
    Timestamp: { type: Date, required: true }
}, { collection: 'Luminosity' }); // Specify the exact collection name

const Luminosity = mongoose.model('Luminosity', luminositySchema);

module.exports = Luminosity;
