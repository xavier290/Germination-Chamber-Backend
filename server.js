const express = require('express');
const connectDB = require('./backend/config/db');
const cors = require('cors');
const dotenv = require('dotenv').config();

connectDB();

const port = process.env.PORT || 5000

const app = express();

app.use((req, res, next) => {
    res.header('Access-Control-Allow-Origin', '*');
    next();
});

app.use(cors());
app.use(express.json());
app.use(express.urlencoded({ extended: false }));


app.use('/api/auth', require('./backend/routes/authRoutes'));
app.use('/api/temperature', require('./backend/routes/temperatureRoutes'));
app.use('/api/humidity', require('./backend/routes/humidityRoutes'));
app.use('/api/luminosity', require('./backend/routes/luminosityRoutes'));
app.use('/api/parameters', require('./backend/routes/parametersRoutes'));


app.listen(port, () => console.log(`Server started and listening on port ${port}`));