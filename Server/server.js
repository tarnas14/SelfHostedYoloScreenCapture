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
let portNumber = config.port ? config.port : 4000;

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

app.get('/image/:imagename', (req, res) => {
    let imagePath = path.resolve('./_upload', req.params.imagename);
    res.sendFile(imagePath, (error) => {
        if (error) {
            console.log(error);
            res.status(error.status).end();
        }
    });
});

app.post('/upload', (req, res) => {
    _saveAnImage(req, function(imageName) {
        console.log('Upload completed!');
        let response = { imageName: imageName };

        res.writeHead(200, {'content-type': 'application/json'});
        res.end(JSON.stringify(response));
    });
});

function _saveAnImage(req, successCallback) {
    let form = new multiparty.Form();
    let timestampImageName = '';

    form.on('part', function(part) {
        if (!_isAFile(part)) {
            part.resume();
        }

        if (_isAFile(part)) {
            console.log('got file named ' + part.filename);

            let imageExtension = part.filename.substring(part.filename.lastIndexOf('.'));
            timestampImageName = Date.now().toString() + imageExtension;

            let savePath = path.resolve(config.path, timestampImageName);
            part.pipe(fs.createWriteStream(savePath));
        }

        part.on('error', function(error) {
            console.log('error', error);
        });
    });

    form.on('close', function() {
        successCallback(timestampImageName);
    });

    form.parse(req);
}

function _isAFile(part) {
    return part.filename;
}