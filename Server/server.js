'use strict';

import http from 'http';
import express from 'express';
import bodyParser from 'body-parser';
import multiparty from 'multiparty';
import fs from 'fs';
import path from 'path';

let configJson = fs.readFileSync(path.resolve('./', 'config.json'));
let config = JSON.parse(configJson);

if(!config.path) {
    throw new Error('provide config json with specified path');
}

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
    _saveAnImage(req, function(imagePath) {
        console.log('Upload completed!');
        let response = { imagePath: imagePath };

        res.writeHead(200, {'content-type': 'application/json'});
        res.end(JSON.stringify(response));
    });
});

function _saveAnImage(req, successCallback) {
    let form = new multiparty.Form();
    let savePath = '';

    form.on('part', function(part) {
        if (!_isAFile(part)) {
            part.resume();
        }

        if (_isAFile(part)) {
            console.log('got file named ' + part.filename);

            let timestampFilename = Date.now().toString() + part.filename;

            savePath = path.resolve(config.path, timestampFilename);
            part.pipe(fs.createWriteStream(savePath));
        }

        part.on('error', function(error) {
            console.log('error', error);
        });
    });

    form.on('close', function() {
        successCallback(savePath);
    });

    form.parse(req);
}

function _isAFile(part) {
    return part.filename;
}