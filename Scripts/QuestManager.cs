using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    #region Singleton Code

    private static QuestManager instance;

    public static QuestManager Instance {
        get {
            if (instance == null) {
                Debug.LogError("Fail to get instance of QuestManager.");
            }

            return instance;
        }
    }

    #endregion

    #region Public gameObject to drop to in Inspector

    public GameObject questTrackerUI;

    #endregion

    private NpcQuestTrackerUI trackerUI;

    private Dictionary<int, Quest> questDatabase = new Dictionary<int, Quest>();

    public Dictionary<int, Quest> QuestDatabase {
        get {
            return questDatabase;
        }
    }

    private LinkedList<int> acceptedQuestID = new LinkedList<int>();

    public LinkedList<int> AcceptedQuestID
    {
        get {
            return acceptedQuestID;
        }
    }

    private LinkedList<int> doneQuestID = new LinkedList<int>();

    void Awake()
    {
        if (instance == null) {
            instance = this;
        } else {
            Debug.LogError("Only one instance of QuestManager is allowed.");
        }

        QuestDatabaseInit();
    }

    void Start()
    {
        if (questTrackerUI == null) {
            Debug.LogError("Fail to get QuestManager's questTrackUI.");
            return;
        }

        trackerUI = questTrackerUI.GetComponent<NpcQuestTrackerUI>();
        if (trackerUI == null) {
            Debug.LogError("Fail to get quest track ui component.");
            return;
        }

        Inventory.Instance.InventoryChanged += OnInventoryChanged;
    }

    // TODO, inti quest database from external file like xml/json file.
    private void QuestDatabaseInit()
    {
        // Quest1 for test.
        List<ItemStack> reward = new List<ItemStack>();
        reward.Add(ItemPrefabsDefinition.StackClone(0, 1));
        reward.Add(ItemPrefabsDefinition.StackClone(1, 1));

        List<QuestGoal> goals = new List<QuestGoal>();
        goals.Add(new QuestGoalCollectItem(3, 5));

        questDatabase.Add(0, new Quest("Hello new hero", 0, QuestProgress.Eligible, 
            "The world is in danger, my deer kitty! You need to level up! Now...Collect some rock for me, " +
            "I will reward you.",
            "Collect 5 rocks.",
            "You can collect them around the house.",
            "OK, your kind heart is the first thing to be a hero.",
            -1, goals, reward));

        // Quest2 for test.
        goals = new List<QuestGoal>();
        goals.Add(new QuestGoalCollectItem(1, 10));

        questDatabase.Add(1, new Quest("Too cold", 1, QuestProgress.Eligible,
            "The milk is freezed! Collect 10 branches for me, not the git branches but real branches.",
            "Collect 10 branches.",
            "Github cat has many branches.",
            "Wait a moment, I will warm up these milk and you can have a taste too!",
            -1, goals, null));

        // Quest3 for test.
        goals = new List<QuestGoal>();
        goals.Add(new QuestGoalVisit(1));

        questDatabase.Add(2, new Quest("Visit Master Kitty", 2, QuestProgress.Eligible,
            "Ninja Kitty wants to tell you something about battle, visit her behind the house.",
            "Visit Ninja Kitty to know how to battle.",
            "She has no patience, now go.",
            "",
            -1, goals, null));
    }

    private void OnInventoryChanged()
    {
        foreach (int questID in acceptedQuestID) {
            bool complete = true;
            Quest quest = questDatabase[questID];
            foreach (QuestGoal goal in quest.questGoalList) {
                if (goal.CheckProgress() == false) {
                    complete = false;
                }
            }

            if (complete) {
                CompleteQuest(questID);
            }
        }

        trackerUI.RefreshQuestTrackerUI();
    }

    public bool AcceptQuest(int questID)
    {
        Quest quest = questDatabase[questID];
        if (quest == null) {
            Debug.Log("Fail to find this quest in QuestDatabase.");
            return false;
        }

        if (quest.progress != QuestProgress.Eligible) {
            Debug.Log("Quest already accepted or not available.");
            return false;
        }

        if (acceptedQuestID.Contains(questID)) {
            Debug.Log("Already in acceptedQuest.");
            return false;
        }

        // doneQuest list check? maybe no need.

        quest.progress = QuestProgress.Accepted;
        acceptedQuestID.AddLast(questID);
        trackerUI.ReInitQuestTrackerUI();

        return true;
    }

    public bool AbandonQuest(int questID)
    {
        Quest quest = questDatabase[questID];
        if (quest == null) {
            Debug.Log("Fail to find this quest in QuestDatabase.");
            return false;
        }

        if (quest.progress != QuestProgress.Accepted) {
            Debug.Log("Quest hasn't been accepted.");
            return false;
        }

        if (!acceptedQuestID.Contains(questID)) {
            Debug.Log("Not in accepted list.");
            return false;
        }

        quest.progress = QuestProgress.Eligible;
        acceptedQuestID.Remove(questID);
        trackerUI.ReInitQuestTrackerUI();

        return true;
    }

    public bool CompleteQuest(int questID)
    {
        Quest quest = questDatabase[questID];
        if (quest == null) {
            Debug.Log("Fail to find this quest in QuestDatabase.");
            return false;
        }
        
        if (quest.progress != QuestProgress.Accepted) {
            Debug.Log("Quest is not complete yet.");
            return false;
        }
        
        if (!acceptedQuestID.Contains(questID)) {
            Debug.Log("Quest hasn't been accepted.");
            return false;
        }
        
        if (doneQuestID.Contains(questID)) {
            Debug.Log("Already done.");
            return false;
        }

        quest.progress = QuestProgress.Complete;

        return true;
    }

    public bool DoneQuest(int questID)
    {
        Quest quest = questDatabase[questID];
        if (quest == null) {
            Debug.Log("Fail to find this quest in QuestDatabase.");
            return false;
        }

        if (quest.progress != QuestProgress.Complete) {
            Debug.Log("Quest is not complete yet.");
            return false;
        }

        if (!acceptedQuestID.Contains(questID)) {
            Debug.Log("Quest hasn't been accepted.");
            return false;
        }

        if (doneQuestID.Contains(questID)) {
            Debug.Log("Already done.");
            return false;
        }

        acceptedQuestID.Remove(questID);

        quest.progress = QuestProgress.Done;
        // Remove quest item from inventory
        foreach (QuestGoal goal in quest.questGoalList) {
            goal.DoneGoal();
        }

        // Refresh Quest Tracker UI
        trackerUI.ReInitQuestTrackerUI();

        // Remove from npc's quest list
        QuestDispatcher.Instance.DoneQuest(quest);

        doneQuestID.AddLast(questID);

        return true;
    }

    public static Quest QuestClone(int id)
    {
        // ShallowClone(MemberwiseClone) creates a new Quest with different
        // value type and the same reference type, not suitable here.
        //return instance.QuestDatabase[id].ShallowClone();
        return instance.QuestDatabase[id];
    }
}