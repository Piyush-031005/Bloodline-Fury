const express = require('express');
const http = require('http');
const WebSocket = require('ws');
const path = require('path');
const os = require('os');

const app = express();
const server = http.createServer(app);
const wss = new WebSocket.Server({ server });

// Serve static files
app.use('/controller', express.static(path.join(__dirname, '../mobile-controller')));
app.use('/', express.static(path.join(__dirname, '../pc-receiver')));

// WebSocket logic
let receiverWs = null;

wss.on('connection', (ws, req) => {
    if (req.url === '/receiver-ws') {
        receiverWs = ws;
        console.log('PC Receiver connected');
        
        ws.on('close', () => {
            console.log('PC Receiver disconnected');
            receiverWs = null;
        });
    } else if (req.url === '/controller-ws') {
        console.log('Mobile Controller connected');
        
        ws.on('message', (message) => {
            // Forward inputs from controller to receiver
            if (receiverWs && receiverWs.readyState === WebSocket.OPEN) {
                receiverWs.send(message.toString());
            }
        });

        ws.on('close', () => {
            console.log('Mobile Controller disconnected');
        });
    } else {
        ws.close();
    }
});

const PORT = 3000;
server.listen(PORT, '0.0.0.0', () => {
    console.log(`\n=========================================`);
    console.log(` SERVER RUNNING ON HTTP://0.0.0.0:${PORT}`);
    console.log(`=========================================\n`);
    console.log(`[1] Open PC Receiver on this machine at:`);
    console.log(`    http://localhost:${PORT}/\n`);
    
    // Find local IP for the phone
    const interfaces = os.networkInterfaces();
    let localIP = 'localhost';
    for (const name of Object.keys(interfaces)) {
        for (const iface of interfaces[name]) {
            if (iface.family === 'IPv4' && !iface.internal) {
                localIP = iface.address;
                break;
            }
        }
    }
    
    console.log(`[2] Connect Phone (same WiFi) to:`);
    console.log(`    http://${localIP}:${PORT}/controller\n`);
    console.log(`Waiting for connections...`);
});
