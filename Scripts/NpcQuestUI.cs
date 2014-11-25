using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NpcQuestUI : MonoBehaviour
{

    #region Public gameObject to drop to in Inspector

    public UILabel dialog;
    public GameObject questTable;
    public GameObject questItemPrefab;

    public GameObject questListPanel;
    public GameObject questNewPanel;
    public GameObject questUndonePanel;
    public GameObject questCompletePanel;

    #endregion

    public LinkedList<Quest> questList;
    public List<GameObject> questItemList = new List<GameObject>();
    public Npc currentNpc;

    // Whether display or not is depending on NpcQuestUI itself, not NPC, especially there may be many NPCs.
    public bool displayQuestUI;

    // Use this for initialization
    void Start()
    {
        if (questTable == null || questItemPrefab == null) {
            Debug.LogError("Fail to get questTable or questItemPrefab.");
            return;
        }

        if (questList == null || questList.Count == 0) {
            Debug.Log("No quest.");
            return;
        }

        if (questListPanel == null || questNewPanel == null || questCompletePanel == null || questUndonePanel == null) {
            Debug.LogError("Fail to get quest panel");
            return;
        }
    }
    
    // Update is called once per frame
    void Update()
    {

    }

    public void DisplayQuestListUI()
    {
        NGUITools.SetActive(questNewPanel, false);
        NGUITools.SetActive(questUndonePanel, false);
        NGUITools.SetActive(questCompletePanel, false);
        NGUITools.SetActive(questListPanel, true);

        foreach (Quest quest in questList) {
            GameObject questItem = NGUITools.AddChild(questTable, questItemPrefab);
            if (questItem != null) {
                NpcQuestItemUI itemUI = questItem.GetComponent<NpcQuestItemUI>();
                itemUI.npcQuest = quest;
                itemUI.questUI = this;

                // Add child gameObject to list, used to remove them later.
                // foreach (Transform child in questTable.transform) GameObject.Destroy(child.gameObject)
                // seems buggy here.
                questItemList.Add(questItem);
            }
        }

        questTable.GetComponent<UITable>().repositionNow = true;
    }

    public void DisplayQuestNewUI(Quest quest)
    {
        NpcQuestNewUI questNew = questNewPanel.GetComponent<NpcQuestNewUI>();
        if (questNew != null && quest != null) {
            questNew.RefreshQuest(quest);
        }

        NGUITools.SetActive(questListPanel, false);
        NGUITools.SetActive(questUndonePanel, false);
        NGUITools.SetActive(questCompletePanel, false);
        NGUITools.SetActive(questNewPanel, true);
    }

    public void DisplayQuestUndoneUI(Quest quest)
    {
        NpcQuestUndoneUI questUndo = questUndonePanel.GetComponent<NpcQuestUndoneUI>();
        if (questUndo != null && quest != null) {
            questUndo.RefreshQuest(quest);
        }

        NGUITools.SetActive(questListPanel, false);
        NGUITools.SetActive(questNewPanel, false);
        NGUITools.SetActive(questCompletePanel, false);
        NGUITools.SetActive(questUndonePanel, true);
    }

    public void DisplayQuestCompleteUI(Quest quest)
    {
        NpcQuestCompleteUI questComplete = questCompletePanel.GetComponent<NpcQuestCompleteUI>();
        if (questComplete != null && quest != null) {
            questComplete.RefreshQuest(quest);
        }

        NGUITools.SetActive(questListPanel, false);
        NGUITools.SetActive(questNewPanel, false);
        NGUITools.SetActive(questUndonePanel, false);
        NGUITools.SetActive(questCompletePanel, true);
    }
    
    // Hide quest ui
    public void DisableQuestUI()
    {
        if (currentNpc != null) {
            displayQuestUI = false;
        }

        // Remove added quest items under questTable immediately
        foreach (GameObject child in questItemList) {
            GameObject.DestroyImmediate(child);
        }

        questItemList.Clear();
    }
}
