const mongoose = require('mongoose');

const relativeSchema = new mongoose.Schema({
    Topic: { type: String, required: true },
    Payload: { type: Number, required: true },
    Timestamp: { type: Date, required: true }
}, { collection: 'Relative' }); // Specify the exact collection name here

const Relative = mongoose.model('Relative', relativeSchema);

module.exports = Relative;