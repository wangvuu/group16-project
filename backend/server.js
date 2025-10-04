const express = require('express');
const app = express();
const userRoutes = require('./routes/user');

app.use(express.json());

// dùng routes
app.use('/', userRoutes);

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
