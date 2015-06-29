'use strict';

import http from 'http';
import express from 'express';
import bodyParser from 'body-parser';
import multiparty from 'multiparty';
import fs from 'fs';

let app = express();
app.use(bodyParser.json());

let server = http.createServer(app);
let portNumber = 4000;

console.log('Listening on port ' + portNumber);
server.listen(portNumber);

app.get('/', (req, res) => {
    res.writeHead(200, {'content-type': 'text/html'});
    res.end(
        '<form action="/upload" enctype="multipart/form-data" method="post">'+
            '<input type="text" name="title"><br>'+
            '<input type="file" name="upload" multiple="multiple"><br>'+
            '<input type="submit" value="Upload">'+
        '</form>'
    );
});

app.post('/upload', (req, res) => {
    let form = new multiparty.Form();

    form.on('part', function(part) {
        if (!_isAFile(part)) {
            part.resume();
        }

        if (_isAFile(part)) {
            console.log('got file named ' + part.filename);
            part.pipe(fs.createWriteStream('../Server/_upload/' + part.filename));
        }

        part.on('error', function(err) {
            console.log('error', err);
        });
    });

    form.on('close', function() {
      console.log('Upload completed!');
      res.writeHead(200, {'content-type': 'text/plain'});
      res.end('Received files');
    });

    form.parse(req);
});

function _isAFile(part) {
    return part.filename;
}