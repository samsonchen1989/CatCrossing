using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Npc : MonoBehaviour
{
    public int npcID;
    public string npcDialog;
    LinkedList<Quest> questList = new LinkedList<Quest>();

    private bool clickable = false;
    private NpcQuestUI questUI;

    public GameObject questListObject;

    public void AddQuest(Quest quest)
    {
        if (quest == null) {
            Debug.Log("Quest null, add fails.");
            return;
        }

        questList.AddLast(quest);
    }

    public void RemoveQuest(Quest quest)
    {
        questList.Remove(quest);
    }

    public void ClearQuestList()
    {
        questList.Clear();
    }

    void Awake()
    {
        QuestDispatcher.Instance.NpcList.Add(npcID, this);
    }

    // Use this for initialization
    void Start()
    {
        if (questListObject == null) {
            Debug.LogError("Fail to get questListObject.");
            return;
        }

        questUI = questListObject.GetComponent<NpcQuestUI>();
        if (questUI == null) {
            Debug.LogError("Fail to get npcQuestListUI.");
            return;
        }
    }
	
    // Update is called once per frame
    void Update()
    {
        if (MouseRaycastManager.Instance.hitObjectType == HitObjectType.NPC) {
            if (clickable && Input.GetMouseButtonUp(1)) {
                questUI.displayQuestUI = true;
            }
        }

        if (questUI.displayQuestUI && questListObject.activeInHierarchy == false) {
            // Display quest UI
            questListObject.SetActive(true);

            questUI.dialog.text = this.npcDialog;
            questUI.questList = this.questList;
            questUI.currentNpc = this;
            questUI.DisplayQuestListUI();
        }

        if (questUI.displayQuestUI == false && questListObject.activeInHierarchy) {
            questUI.DisableQuestUI();
            questListObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        clickable = true;
    }
    
    void OnTriggerExit(Collider other)
    {
        clickable = false;
        questUI.displayQuestUI = false;
    }
}
