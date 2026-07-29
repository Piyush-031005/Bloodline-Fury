using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class PhoneController : MonoBehaviour
{
    [Header("Connection Settings")]
    public string serverUrl = "ws://localhost:3000/receiver-ws";

    [Header("Input State (Read-Only)")]
    public Vector2 JoystickInput;
    public bool ButtonX;
    public bool ButtonY;
    public bool ButtonA;
    public bool ButtonB;

    private ClientWebSocket ws;
    private CancellationTokenSource cts;
    
    // Thread-safe queue for incoming JSON messages
    private ConcurrentQueue<string> messageQueue = new ConcurrentQueue<string>();

    [Serializable]
    private class JoystickValue
    {
        public float x;
        public float y;
    }

    private async void Start()
    {
        await ConnectToServer();
    }

    private async Task ConnectToServer()
    {
        ws = new ClientWebSocket();
        cts = new CancellationTokenSource();

        try
        {
            Debug.Log($"Connecting to Node.js server at {serverUrl}...");
            await ws.ConnectAsync(new Uri(serverUrl), cts.Token);
            Debug.Log("<color=green>Connected to Node.js Server!</color> Waiting for phone inputs...");

            // Start receiving messages in the background
            _ = ReceiveLoop();
        }
        catch (Exception e)
        {
            Debug.LogError($"Connection failed: {e.Message}");
        }
    }

    private async Task ReceiveLoop()
    {
        var buffer = new byte[1024 * 4];

        while (ws.State == WebSocketState.Open && !cts.IsCancellationRequested)
        {
            try
            {
                var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), cts.Token);
                
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                    Debug.LogWarning("Server closed connection.");
                }
                else
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    messageQueue.Enqueue(message);
                }
            }
            catch (Exception e)
            {
                if (!cts.IsCancellationRequested)
                {
                    Debug.LogError($"Error receiving message: {e.Message}");
                }
            }
        }
    }

    private void Update()
    {
        // Process all queued messages on the main Unity thread
        while (messageQueue.TryDequeue(out string message))
        {
            ProcessMessage(message);
        }
    }

    private void ProcessMessage(string message)
    {
        // Simple string parsing to avoid NewtonSoft dependencies inside base Unity
        if (message.Contains("\"type\":\"joystick\""))
        {
            try
            {
                int valIndex = message.IndexOf("\"value\":") + 8;
                string valJson = message.Substring(valIndex, message.Length - valIndex - 1);
                JoystickValue jVal = JsonUtility.FromJson<JoystickValue>(valJson);
                JoystickInput = new Vector2(jVal.x, jVal.y);
            }
            catch { /* Ignore parse error */ }
        }
        else if (message.Contains("\"type\":\"button_down\""))
        {
            string btn = ExtractButton(message);
            SetButtonState(btn, true);
        }
        else if (message.Contains("\"type\":\"button_up\""))
        {
            string btn = ExtractButton(message);
            SetButtonState(btn, false);
        }
    }

    private string ExtractButton(string message)
    {
        if (message.Contains("\"value\":\"X\"")) return "X";
        if (message.Contains("\"value\":\"Y\"")) return "Y";
        if (message.Contains("\"value\":\"A\"")) return "A";
        if (message.Contains("\"value\":\"B\"")) return "B";
        return "";
    }

    private void SetButtonState(string button, bool state)
    {
        switch (button)
        {
            case "X": ButtonX = state; break;
            case "Y": ButtonY = state; break;
            case "A": ButtonA = state; break;
            case "B": ButtonB = state; break;
        }
    }

    private void OnDestroy()
    {
        if (ws != null)
        {
            cts.Cancel();
            ws.Dispose();
        }
    }
}
