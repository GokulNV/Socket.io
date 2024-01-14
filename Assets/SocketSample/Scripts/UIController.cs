using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject connectionPanel;
    [SerializeField] private GameObject debugPanel;
    [SerializeField] private TMP_Text debugTextPrefab;
    [SerializeField] private TMP_InputField urlInputField;
    [SerializeField] private TMP_InputField messageInputField;
    [SerializeField] private Button connectButton;
    [SerializeField] private Button sendMessageButton;
    [SerializeField] private Button disconnectButton;
    [SerializeField] private Transform contentTransform;

    private readonly List<GameObject> debugObjectList = new();

    private void OnEnable()
    {
        connectButton.onClick.AddListener(OnClickConnectButton);
        disconnectButton.onClick.AddListener(OnClickDisconnectButton);
        sendMessageButton.onClick.AddListener(OnClickSendMessageButton);

        InGameEventHandler.OnWSConnectionSuccess += OnConnectionSuccess;
        InGameEventHandler.OnWSConnectionClosed += OnConnectionClosed;
        InGameEventHandler.OnMessageReceived += OnMessageReceived;
    }

    private void OnDisable()
    {
        connectButton.onClick.RemoveListener(OnClickConnectButton);
        disconnectButton.onClick.RemoveListener(OnClickDisconnectButton);
        sendMessageButton.onClick.RemoveListener(OnClickSendMessageButton);

        InGameEventHandler.OnWSConnectionSuccess -= OnConnectionSuccess;
        InGameEventHandler.OnWSConnectionClosed -= OnConnectionClosed;
        InGameEventHandler.OnMessageReceived -= OnMessageReceived;
    }

    #region Button Listeners

    private void OnClickConnectButton()
    {
        if (string.IsNullOrEmpty(urlInputField.text))
            return;

        InGameEventHandler.InitiateConnection(urlInputField.text);
        urlInputField.text = "";
    }

    private void OnClickSendMessageButton()
    {
        if (string.IsNullOrEmpty(messageInputField.text))
            return;

        var input = messageInputField.text;
        InGameEventHandler.SendMessage(input);

        UnityMainThread.wkr.AddJob(() =>
        {
            SpawnAndShowDebug("Send:  " + input);
        });
        messageInputField.text = "";
    }

    private void OnClickDisconnectButton()
    {
        InGameEventHandler.DisconnectSocket();
    }

    #endregion

    #region Event Callbacks

    private void OnConnectionSuccess()
    {
        debugPanel.SetActive(true);
        connectionPanel.SetActive(false);

        UnityMainThread.wkr.AddJob(() =>
        {
            SpawnAndShowDebug("Connection established...");
        });
    }

    private void OnConnectionClosed()
    {
        foreach (var item in debugObjectList)
        {
            Destroy(item);
        }
        debugObjectList.Clear();
        connectionPanel.SetActive(true);
        debugPanel.SetActive(false);
    }

    private void OnMessageReceived(string message)
    {
        UnityMainThread.wkr.AddJob(() =>
        {
            SpawnAndShowDebug("Received:  " + message);
        });
    }

    #endregion

    private void SpawnAndShowDebug(string message)
    {
        var spawnedText = Instantiate(debugTextPrefab, contentTransform);
        spawnedText.text = message;
        spawnedText.transform.SetAsFirstSibling();
        debugObjectList.Add(spawnedText.gameObject);
    }
}
