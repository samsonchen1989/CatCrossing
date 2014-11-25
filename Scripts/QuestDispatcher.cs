using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestDispatcher : MonoBehaviour
{
    #region Singleton

    private static QuestDispatcher instance;

    public static QuestDispatcher Instance {
        get {
            if (instance == null) {
                Debug.LogError("Fail to find Instance of QuestDispatcher.");
            }

            return instance;
        }
    }

    #endregion

    int currentStep = 0;

    List<QuestNode> allQuests = new List<QuestNode>();

    //All npc are registered here themselves.
    Dictionary<int, Npc> npcList = new Dictionary<int, Npc>();

    public Dictionary<int, Npc> NpcList {
        get {
            return npcList;
        }
    }

    void Awake()
    {
        if (instance == null) {
            instance = this;
        } else {
            Debug.LogError("Only one instance of QuestGiver is allowed!");
        }

        InitQuestDispatcher();
    }

    void Start()
    {
        currentStep = 0;
        DispatchSceneQuest();
    }

    void Update()
    {

    }

    // Todo, Init quest list of current scene from external file like json/xml.
    void InitQuestDispatcher()
    {
        // Add quest for test, after quest0 and quest1 are done,
        // quest2 will be triggered and added to its quest owner.
        QuestNode oneStepQuest = new QuestNode(0);
        oneStepQuest.AddQuest(0);
        oneStepQuest.AddQuest(1);

        allQuests.Add(oneStepQuest);

        oneStepQuest = new QuestNode(1);
        oneStepQuest.AddQuest(2);
        allQuests.Add(oneStepQuest);
    }

    void DispatchSceneQuest()
    {
        if (allQuests[currentStep] == null) {
            Debug.Log("No quest in current step.");
            return;
        }

        //check if all former quests are complete
 
        foreach (Quest quest in allQuests[currentStep].QuestList) {
            npcList[quest.ownerId].AddQuest(quest);
        }
    }

    // If quest goal contains "Visit" content, remove this quest from
    // former NPC and add to new NPC.
    public void ChangeQuestOwner(Quest quest, int formerNpc, int newNpc)
    {
        if (npcList[formerNpc] == null || npcList[newNpc] == null) {
            Debug.LogError("No such npc id.");
            return;
        }

        npcList[formerNpc].RemoveQuest(quest);
        npcList[newNpc].AddQuest(quest);
    }

    public void DoneQuest(Quest quest)
    {
        npcList[quest.ownerId].RemoveQuest(quest);
        // Check whether current step's quests are all done,
        // if true, then dispatch next step's quests
        foreach (Quest stepQuest in allQuests[currentStep].QuestList) {
            if (stepQuest.progress != QuestProgress.Done) {
                return;
            }
        }

        if (currentStep < allQuests.Count) {
            currentStep++;
        }

        Debug.Log("Dispatch next step quest, currentStep:" + currentStep);
        DispatchSceneQuest();
    }
}