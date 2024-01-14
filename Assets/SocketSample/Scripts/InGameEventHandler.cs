public class InGameEventHandler
{
    public delegate void ConnectWebSocket(string url);
    public static ConnectWebSocket OnInitiateConnection;
    public static void InitiateConnection(string url) => OnInitiateConnection?.Invoke(url);

    public delegate void WebSocketConnection();
    public static WebSocketConnection OnWSConnectionSuccess;
    public static void WSConnectionSuccess() => OnWSConnectionSuccess?.Invoke();
    public static WebSocketConnection OnInitiateDisconnection;
    public static void DisconnectSocket() => OnInitiateDisconnection?.Invoke();
    public static WebSocketConnection OnWSConnectionClosed;
    public static void WSConnectionClosed() => OnWSConnectionClosed?.Invoke();

    public delegate void WebSocketMessage(string message);
    public static WebSocketMessage OnMessageReceived;
    public static void MessageReceived(string message) => OnMessageReceived?.Invoke(message);
    public static WebSocketMessage OnSendMessage;
    public static void SendMessage(string message) => OnSendMessage?.Invoke(message);
}
