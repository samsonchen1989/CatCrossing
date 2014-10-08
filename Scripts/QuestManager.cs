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

    private Dictionary<int, Quest> questDatabase = new Dictionary<int, Quest>();

    public Dictionary<int, Quest> QuestDatabase {
        get {
            return questDatabase;
        }
    }

    private LinkedList<int> acceptedQuestID = new LinkedList<int>();
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

    // TODO, inti quest database from external file like xml/json file.
    void QuestDatabaseInit()
    {
        List<ItemStack> reward = new List<ItemStack>();
        reward.Add(ItemPrefabsDefinition.StackClone(0, 1));
        reward.Add(ItemPrefabsDefinition.StackClone(1, 1));

        List<QuestGoal> goals = new List<QuestGoal>();
        goals.Add(new QuestGoalCollectItem(0, 5));

        questDatabase.Add(0, new Quest("Hello new hero", 0, Quest.QuestProgress.Eligible, 
            "The world is in danger, my deer kitty! You need to level up! Now...Collect some grass for me, " +
            "I will reward you.",
            "You can collect them behind the house.",
            -1, goals, reward));

        goals = new List<QuestGoal>();
        goals.Add(new QuestGoalCollectItem(1, 10));

        questDatabase.Add(1, new Quest("Too cold", 1, Quest.QuestProgress.Eligible,
            "The milk is freezed! Collect 10 branches for me, not the git branches but real branches.",
            "Github cat has many branches.",
            -1, goals, null));

        goals = new List<QuestGoal>();
        goals.Add(new QuestGoalVisit(1));

        questDatabase.Add(2, new Quest("Visit Master Kitty", 2, Quest.QuestProgress.Eligible,
            "Visit Master Kitty behind the house.",
            "She has no mouse.",
            -1, goals, null));
    }

    bool AcceptQuest(int questID)
    {
        Quest quest = questDatabase [questID];
        if (quest == null) {
            Debug.Log("Fail to find this quest in QuestDatabase.");
            return false;
        }

        if (quest.progress != Quest.QuestProgress.Eligible) {
            Debug.Log("Quest already accepted or not available.");
            return false;
        }

        if (acceptedQuestID.Contains(questID)) {
            Debug.Log("Already in acceptedQuest.");
            return false;
        }

        // doneQuest list check? maybe no need.

        quest.progress = Quest.QuestProgress.Accepted;
        acceptedQuestID.AddLast(questID);

        return true;
    }

    bool AbandonQuest(int questID)
    {
        Quest quest = questDatabase [questID];
        if (quest == null) {
            Debug.Log("Fail to find this quest in QuestDatabase.");
            return false;
        }

        if (quest.progress != Quest.QuestProgress.Accepted) {
            Debug.Log("Quest hasn't been accepted.");
            return false;
        }

        if (!acceptedQuestID.Contains(questID)) {
            Debug.Log("Not in accepted list.");
            return false;
        }

        quest.progress = Quest.QuestProgress.Eligible;
        acceptedQuestID.Remove(questID);

        return true;
    }

    bool DoneQuest(int questID)
    {
        Quest quest = questDatabase [questID];
        if (quest == null) {
            Debug.Log("Fail to find this quest in QuestDatabase.");
            return false;
        }

        if (quest.progress != Quest.QuestProgress.Complete) {
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

        quest.progress = Quest.QuestProgress.Done;
        acceptedQuestID.Remove(questID);
        doneQuestID.AddLast(questID);

        return true;
    }

    public static Quest QuestClone(int id)
    {
        return instance.QuestDatabase [id].ShallowClone();
    }
}