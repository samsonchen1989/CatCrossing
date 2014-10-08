using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPC : MonoBehaviour
{

    public int npcID;
    LinkedList<Quest> questList = new LinkedList<Quest>();

    public void AddQuest(Quest quest)
    {
        if (quest == null) {
            Debug.Log("Quest null, add fails.");
            return;
        }

        questList.AddLast(quest);
    }

    public void ClearQuestList()
    {
        questList.Clear();
    }

    void Awake()
    {
        QuestDispatcher.Instance.NPCList.Add(npcID, this);
    }

    // Use this for initialization
    void Start()
    {
    
    }
	
    // Update is called once per frame
    void Update()
    {
	
    }
}
