using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FloatingTextSpawner : MonoBehaviour
{
    private GameObject nameLabel;

    private Dictionary<int, GameObject> labelDic = new Dictionary<int, GameObject>();

    void OnEnable()
    {
        Messenger<GameObject>.AddListener(MyEventType.OTHER_PLAYER_JOIN, AddNameLabel);
        Messenger<GameObject>.AddListener(MyEventType.PLAYER_BORN, AddNameLabel);

        Messenger<int>.AddListener(MyEventType.OTHER_PLAYER_LEAVE, RemoveNameLabel);
    }

    void OnDisable()
    {
        Messenger<GameObject>.RemoveListener(MyEventType.OTHER_PLAYER_JOIN, AddNameLabel);
        Messenger<GameObject>.RemoveListener(MyEventType.PLAYER_BORN, AddNameLabel);

        Messenger<int>.RemoveListener(MyEventType.OTHER_PLAYER_LEAVE, RemoveNameLabel);
    }

    // Add name label for player and other joined player
    void AddNameLabel(GameObject player)
    {
        GameObject name = NGUITools.AddChild(this.gameObject, nameLabel);
        FloatingText text = name.GetComponent<FloatingText>();
        text.Target = player;
        text.WorldCamera = NGUITools.FindCameraForLayer(player.layer);
        text.Text = player.GetComponent<PlayerStatus>().name;

        int id = player.GetComponent<PhotonView>().ownerId;
        labelDic.Add(id, name);
    }

    void RemoveNameLabel(int ownerID)
    {
        if (!labelDic.ContainsKey(ownerID)) {
            return;
        }

        GameObject label = labelDic[ownerID];
        if (label != null) {
            GameObject.DestroyImmediate(label);
            labelDic.Remove(ownerID);
        }
    }

    // Use this for initialization
    void Start()
    {
        nameLabel = Resources.Load("NameLabel") as GameObject;
        if (nameLabel == null) {
            Debug.LogError("Fail to find prefab nameLabel");
        }
    }
    
    // Update is called once per frame
    void Update()
    {
    
    }
}
