using WebSocketSharp;
using UnityEngine;
using System;

public class WebSocketManager : MonoBehaviour
{
    public string url;
    private WebSocket ws;

    private void OnEnable()
    {
        InGameEventHandler.OnInitiateConnection += InitiateConnection;
        InGameEventHandler.OnInitiateDisconnection += InitiateDisconnection;
        InGameEventHandler.OnSendMessage += OnSendMessage;
    }

    private void OnDisable()
    {
        InGameEventHandler.OnInitiateConnection -= InitiateConnection;
        InGameEventHandler.OnInitiateDisconnection -= InitiateDisconnection;
        InGameEventHandler.OnSendMessage -= OnSendMessage;
    }

    #region Event Callbacks

    private void InitiateConnection(string url)
    {
        ws = new WebSocket(url);
        ws.OnOpen += OnConnectionSuccess;
        ws.OnClose += OnConnectionClosed;
        ws.Connect();
    }

    private void InitiateDisconnection()
    {
        ws.Close();
    }

    private void OnSendMessage(string message)
    {
        ws.Send(message);
    }

    #endregion

    #region Socket Callbacks

    private void OnConnectionSuccess(object sender, EventArgs e)
    {
        ws.OnMessage += OnMessageReceived;
        InGameEventHandler.WSConnectionSuccess();
    }

    private void OnMessageReceived(object sender, MessageEventArgs e)
    {
        InGameEventHandler.MessageReceived(e.Data);
    }

    private void OnConnectionClosed(object sender, CloseEventArgs e)
    {
        InGameEventHandler.WSConnectionClosed();
    }

    #endregion
}