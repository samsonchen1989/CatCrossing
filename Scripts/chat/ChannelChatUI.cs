using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChannelChatUI : MonoBehaviour
{
    #region Inspector Assign component

    public GameObject labelPrefab;
    public UITable chatTable;
    public UIScrollBar scrollBar;
    public GameObject inputObject;

    #endregion

    private static ChannelChatUI instance;
    public static ChannelChatUI Instance
    {
        get {
            if (instance == null) {
                Debug.LogError("Fail to find channel chat ui");
            }

            return instance;
        }
    }


    const int MAX_LABEL_NUM = 25;

    LinkedList<GameObject> chatLabelList = new LinkedList<GameObject>();

    private bool isInput;

    void Awake()
    {
        if (instance != null) {
            Debug.LogError("Only one instance of channelChatUI is allowwed.");
            return;
        }

        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        if (chatTable == null || scrollBar == null || labelPrefab == null || inputObject == null) {
            Debug.LogError("Fail to find ChatUI's UI component.");
            return;
        }

        // Set max line to one
        inputObject.GetComponent<UIInput>().label.maxLineCount = 1;

        isInput = false;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return)) {
            if (isInput) {
                isInput = false;
                inputObject.SetActive(false);
                // Get input message and send to chat server
                string msg = inputObject.GetComponent<UIInput>().value;
                if (string.IsNullOrEmpty(msg)) {
                    return;
                } else {
                    ChatHandler.Instance.SendChatMessage(msg, Color.white);
                }
            } else {
                // Show input ui and ready for input
                inputObject.SetActive(true);
                inputObject.GetComponent<UIInput>().isSelected = true;
                inputObject.GetComponent<UIInput>().value = null;

                isInput = true;
            }
        }
    }

    public void InsertMessage(string message, Color color)
    {
        GameObject go = NGUITools.AddChild(chatTable.gameObject, labelPrefab);

        UILabel chatLabel = go.GetComponent<UILabel>();
        chatLabel.text = message;
        chatLabel.color = color;

        CheckLabelCount(go);

        chatTable.repositionNow = true;
        scrollBar.value = 1;
    }

    // Remove the first gameObject in queue if count larger than MAX_LABEL_NUM
    void CheckLabelCount(GameObject go)
    {
        chatLabelList.AddLast(go);

        if (chatLabelList.Count > MAX_LABEL_NUM) {
            GameObject.DestroyImmediate(chatLabelList.First.Value);
            chatLabelList.RemoveFirst();
        }
    }
}
