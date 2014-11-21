using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NpcQuestTrackerUI : MonoBehaviour {

    #region Public gameObject to drop to in Inspector

    public GameObject questProgressPrefab;
    public GameObject questTable;

    #endregion

    List<GameObject> trackQuestList = new List<GameObject>();

    // Use this for initialization
    void Start () {
        if (questProgressPrefab == null || questTable == null) {
            Debug.LogError("Fail to find quest tracker ui's prefab or table.");
            return;
        }
    }
    
    // Update is called once per frame
    void Update () {
    
    }

    // QuestManager's AcceptedQuestID list has been changed, call
    // this function to re-init all UI
    public void ReInitQuestTrackerUI()
    {
        /*
        foreach (Transform child in questTable.transform) {
            GameObject.DestroyImmediate(child.gameObject);
        }
        */

        // Clear all table's child object first
        foreach (GameObject questItem in trackQuestList) {
            GameObject.DestroyImmediate(questItem);
        }

        trackQuestList.Clear();

        if (QuestManager.Instance.AcceptedQuestID.Count == 0) {
            DisableQuestTrackerUI();
            return;
        }

        if (this.gameObject.activeInHierarchy == false) {
            NGUITools.SetActive(this.gameObject, true);
        }

        foreach (int questID in QuestManager.Instance.AcceptedQuestID) {
            GameObject trackQuestItem = NGUITools.AddChild(questTable, questProgressPrefab);
            if (trackQuestItem != null) {
                NpcQuestProgressUI progressUI = trackQuestItem.GetComponent<NpcQuestProgressUI>();
                progressUI.RefreshQuest(QuestManager.QuestClone(questID));

                trackQuestList.Add(trackQuestItem);
            }
        }

        questTable.GetComponent<UITable>().repositionNow = true;
    }

    // QuestManager's AcceptedQuestID not changed, just refresh each
    // quest's progress
    public void RefreshQuestTrackerUI()
    {
        foreach (GameObject item in trackQuestList) {
            if (item != null) {
                NpcQuestProgressUI progressUI = item.GetComponent<NpcQuestProgressUI>();
                progressUI.RefreshUI();
            }
        }
    }

    public void DisableQuestTrackerUI()
    {
        NGUITools.SetActive(this.gameObject, false);
    }
}
