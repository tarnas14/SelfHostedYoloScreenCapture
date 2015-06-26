import http from 'http';
import express from 'express';
import bodyParser from 'body-parser';

let app = express();
app.use(bodyParser.json());

let server = http.createServer(app);
let portNumber = 4000;

console.log("Listening on port " + portNumber);
server.listen(portNumber);

app.get('/', (req, res) => {
    res.send('ok');
});

app.post('/', (req, res) => {
    res.end();
});