using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon.Chat;

public class ChatHandler : MonoBehaviour, IChatClientListener
{

    public string chatAppId;
    public string[] channelToJoin;

    public ChatClient chatClient;

    private string localName;

    private static ChatHandler instance;
    public static ChatHandler Instance
    {
        get {
            if (instance == null) {
                Debug.LogError("Fail to find ChatHandler instance.");
            }

            return instance;
        }
    }

    void Awake()
    {
        if (instance != null) {
            Debug.Log("Only one instance of ChatHandler is allowwed.");
            DestroyImmediate(gameObject);
        }

        instance = this;
    }

    void OnDisable()
    {
        if (chatClient != null) {
            chatClient.Disconnect();
        }
    }

    public void ConnectChatServer(string name)
    {
        chatClient = new ChatClient(this);
        localName = name;
        chatClient.Connect(chatAppId, "0.1", name, null);
    }

    // Use this for initialization
    void Start()
    {
        if (string.IsNullOrEmpty(chatAppId) || channelToJoin.Length == 0) {
            Debug.LogError("Please init chat app id or channel to join.");
            return;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (chatClient != null) {
            chatClient.Service();
        }
    }

    public void SendChatMessage(string msg, Color color)
    {
        foreach(string channel in channelToJoin) {
            Debug.Log("channel:" + channel + ", msg:" + msg);
            chatClient.PublishMessage(channel, msg);
        }

        string message = string.Format("{0}:{1}", localName, msg);
        ChannelChatUI.Instance.InsertMessage(message, color);
    }

    public void OnConnected()
    {
        chatClient.Subscribe(channelToJoin);
        chatClient.SetOnlineStatus(ChatUserStatus.Online);
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        Debug.Log("OnGetMessages, channel name:" + channelName);
        string msg = "";
        for (int i = 0; i < senders.Length; i++) {
            if (senders[i] == localName) {
                return;
            }

            msg = senders[i] + ":" + messages[i];
            if (ChannelChatUI.Instance != null) {
                ChannelChatUI.Instance.InsertMessage(msg, Color.white);
            }
        }
    }

    public void OnSubscribed(string[] channels, bool[] result)
    {
        //SendMessage("Online", Color.yellow);
    }
    
    public void OnUnsubscribed(string[] channels)
    {
        
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {

    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {

    }

    public void OnDisconnected()
    {
        
    }
    
    public void OnChatStateChange(ChatState state)
    {
        
    }
}
