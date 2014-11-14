﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Npc : MonoBehaviour
{

    public int npcID;
    public string npcDialog;
    LinkedList<Quest> questList = new LinkedList<Quest>();

    private bool clickable = false;
    private bool displayQuestUI = false;
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

    public void ClearQuestList()
    {
        questList.Clear();
    }

    public void SetQuestUIVisible(bool visible)
    {
        displayQuestUI = visible;
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
        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity);
        if (hit) {
            GameObject hitObject = hitInfo.transform.gameObject;
            if (hitObject && hitObject.tag == "NPC") {
                if (clickable && Input.GetMouseButtonUp(1)) {
                    displayQuestUI = true;
                }
            }
        }

        if (displayQuestUI && questListObject.activeInHierarchy == false) {
            // Display quest UI
            questListObject.SetActive(true);

            questUI.dialog.text = this.npcDialog;
            questUI.questList = this.questList;
            questUI.currentNpc = this;
            questUI.DisplayQuestListUI();
        }

        if (displayQuestUI == false && questListObject.activeInHierarchy) {
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
        displayQuestUI = false;
    }
}
