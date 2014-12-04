using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Npc : MonoBehaviour
{
    #region Public gameObject to drop to in Inspector

    public GameObject questListObject;

    public GameObject questionMark;
    public GameObject questionGreyMark;
    public GameObject exclamationMark;
    
    #endregion

    public int npcID;
    public string npcName;
    public string npcDialog;
    LinkedList<Quest> questList = new LinkedList<Quest>();

    private bool clickable = false;
    private NpcQuestUI questUI;

    private GameObject questMark;
    private float yRotate;
    private const float RotateYSpeed = 2.0f;
    private Vector3 Offset = new Vector3(0f, 0.15f, 0.1f);

    private NpcQuestState currentState;

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

        if (questionMark == null || questionGreyMark == null || exclamationMark == null) {
            Debug.LogError("Fail to find quest mark prefabs.");
            return;
        }

        questUI = questListObject.GetComponent<NpcQuestUI>();
        if (questUI == null) {
            Debug.LogError("Fail to get npcQuestListUI.");
            return;
        }

        questMark = null;
        currentState = NpcQuestState.QuestNone;

        yRotate = 0f;
    }
	
    // Update is called once per frame
    void Update()
    {
        if (MouseRaycastManager.Instance.hitObjectType == HitObjectType.NPC) {
            if (clickable && InputManager.GetMouseButtonUp(1)) {
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

        CheckNpcQuestState();

        // Rotate questMark if have one
        if (questMark != null) {
            yRotate = (yRotate + RotateYSpeed) % 360;
            questMark.transform.rotation = Quaternion.Euler(new Vector3(0f, yRotate, 0f));
        }
    }

    private void CheckNpcQuestState()
    {
        NpcQuestState state = NpcQuestState.QuestNone;

        // Check current state first
        if (questList.Count == 0) {
            state = NpcQuestState.QuestNone;
        } else {
            foreach (Quest quest in questList) {
                if (quest.progress == QuestProgress.Eligible) {
                    state = NpcQuestState.QuestEligible;
                    break;
                }

                if (quest.progress == QuestProgress.Accepted) {
                    state = NpcQuestState.QuestUncomplete;
                }
            }

            foreach (Quest quest in questList) {
                if (quest.progress == QuestProgress.Complete) {
                    state = NpcQuestState.QuestComplete;
                    break;
                }
            }
        }

        if (currentState != state) {
            // Npc Quest State changed
            if (questMark != null) {
                GameObject.DestroyImmediate(questMark);
            }

            currentState = state;

            // Iniate new quest mark
            switch(currentState)
            {
                case NpcQuestState.QuestUncomplete:
                    // Add QuestMark to npc, add localPosition offset
                    questMark = NGUITools.AddChild(gameObject, questionGreyMark);
                    questMark.transform.localPosition += Offset;
                    break;
                case NpcQuestState.QuestComplete:
                    questMark = NGUITools.AddChild(gameObject, questionMark);
                    questMark.transform.localPosition += Offset;
                    break;
                case NpcQuestState.QuestEligible:
                    questMark = NGUITools.AddChild(gameObject, exclamationMark);
                    questMark.transform.localPosition += Offset;
                    break;
            }
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
