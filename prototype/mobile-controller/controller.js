const statusEl = document.getElementById('status');
const protocol = window.location.protocol === 'https:' ? 'wss:' : 'ws:';
const ws = new WebSocket(`${protocol}//${window.location.host}/controller-ws`);

ws.onopen = () => {
    statusEl.innerText = 'LINKED';
    statusEl.style.color = '#00ff00';
    triggerHaptic(50);
};

ws.onclose = () => {
    statusEl.innerText = 'SEVERED';
    statusEl.style.color = '#ff0000';
};

function sendAction(type, value) {
    if (ws.readyState === WebSocket.OPEN) {
        ws.send(JSON.stringify({ type, value }));
    }
}

function triggerHaptic(duration) {
    if (navigator.vibrate) {
        navigator.vibrate(duration);
    }
}

// Action Buttons
document.querySelectorAll('.btn').forEach(btn => {
    btn.addEventListener('touchstart', (e) => {
        e.preventDefault();
        const action = btn.getAttribute('data-action');
        sendAction('button_down', action);
        triggerHaptic(30);
        btn.style.background = 'rgba(255,255,255,0.2)';
        btn.style.color = '#ffffff';
        // Add a small scale effect for feedback
        let transformStr = btn.style.transform || "";
        if(!transformStr.includes("scale(0.9)")) {
             btn.style.transform = transformStr + " scale(0.9)";
        }
    });
    
    btn.addEventListener('touchend', (e) => {
        e.preventDefault();
        const action = btn.getAttribute('data-action');
        sendAction('button_up', action);
        btn.style.background = 'transparent';
        btn.style.color = 'rgba(255, 255, 255, 0.5)';
        btn.style.transform = btn.style.transform.replace(" scale(0.9)", "");
    });
});

// Joystick Logic
const stick = document.getElementById('joystick-stick');
const base = document.getElementById('joystick-base');
const zone = document.getElementById('joystick-zone');

let isDragging = false;
let baseRect;

zone.addEventListener('touchstart', (e) => {
    e.preventDefault();
    isDragging = true;
    baseRect = base.getBoundingClientRect();
    updateJoystick(e.touches[0]);
    triggerHaptic(15);
});

zone.addEventListener('touchmove', (e) => {
    e.preventDefault();
    if (isDragging) {
        updateJoystick(e.touches[0]);
    }
});

zone.addEventListener('touchend', (e) => {
    e.preventDefault();
    isDragging = false;
    stick.style.transform = `translate(-50%, -50%)`;
    sendAction('joystick', { x: 0, y: 0 });
});

function updateJoystick(touch) {
    const centerX = baseRect.left + baseRect.width / 2;
    const centerY = baseRect.top + baseRect.height / 2;
    
    let dx = touch.clientX - centerX;
    let dy = touch.clientY - centerY;
    
    const maxDistance = baseRect.width / 2;
    const distance = Math.sqrt(dx * dx + dy * dy);
    
    if (distance > maxDistance) {
        dx = (dx / distance) * maxDistance;
        dy = (dy / distance) * maxDistance;
    }
    
    stick.style.transform = `translate(calc(-50% + ${dx}px), calc(-50% + ${dy}px))`;
    
    // Normalize to -1.0 to 1.0
    const nx = dx / maxDistance;
    const ny = -(dy / maxDistance); // Invert Y so up is positive
    
    sendAction('joystick', { x: parseFloat(nx.toFixed(2)), y: parseFloat(ny.toFixed(2)) });
}
