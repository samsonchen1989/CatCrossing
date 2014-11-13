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

    //Init quest list of current scene from external file like json/xml.
    void InitQuestDispatcher()
    {
        QuestNode oneStepQuest = new QuestNode(0);
        oneStepQuest.AddQuest(0);
        oneStepQuest.AddQuest(1);

        allQuests.Add(oneStepQuest);
    }

    void DispatchSceneQuest()
    {
        if (allQuests [currentStep] == null) {
            Debug.Log("No quest in current step.");
            return;
        }

        //check if all former quests are complete
 
        foreach (Quest quest in allQuests[currentStep].QuestNodeList) {
            npcList[quest.ownerId].AddQuest(quest);
        }
    }
}