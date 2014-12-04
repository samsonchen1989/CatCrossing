using UnityEngine;
using System.Collections;

public delegate void DelegateCall();

public class FuncCallSpeedTest : MonoBehaviour {

    public DelegateCall del;

    SendMessageCheck checkTool;
    float time = 0f;

	// Use this for initialization
	void Start() {
        checkTool = GetComponent<SendMessageCheck>();
        if (checkTool == null) {
            Debug.LogError("no checktool.");
        }
	}
	
	// Update is called once per frame
	void Update() {
        time = Time.realtimeSinceStartup;

        Debug.Log("Start:" + time);
        for (int i = 0; i < 10000; i++) {
            checkTool.DirectCall();
        }

        Debug.Log("DirectCall Time pass:" + (Time.realtimeSinceStartup - time).ToString());
        time = Time.realtimeSinceStartup;

        for (int i = 0; i < 10000; i++) {
            checkTool.SendMessage("SendMessageCall");
        }

        Debug.Log("SendMessageCall Time pass:" + (Time.realtimeSinceStartup - time).ToString());
        time = Time.realtimeSinceStartup;

        for (int i = 0; i < 10000; i++) {
            del();
        }

        Debug.Log("DelegateCall Time pass:" + (Time.realtimeSinceStartup - time).ToString());
	}
}
