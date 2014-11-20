using UnityEngine;
using System.Collections;

public class NpcQuestProgressUI : MonoBehaviour
{

    #region Public gameObject to drop to in Inspector
    
    public UILabel questName;
    public UILabel questProgress;
    public UILabel questTip;
    public UILabel questDetail;
    
    #endregion

    static string[] QuestProgressText = {
        "Not Available",
        "Available",
        "Accepted",
        "Complete",
        "Done"
    };
    private Quest trackQuest;

    // Use this for initialization
    void Start()
    {
        if (questName == null || questProgress == null || questTip == null || questDetail == null) {
            Debug.LogError("Fail to find quest progress ui's label.");
            return;
        }

        RefreshUI();
    }
    
    // Update is called once per frame
    void Update()
    {
    
    }

    public void RefreshUI()
    {
        if (trackQuest == null) {
            return;
        }

        questName.text = trackQuest.title;
        questTip.text = trackQuest.target;
        questProgress.text = QuestProgressText [(int)trackQuest.progress];

        string detail;
        foreach (QuestGoal goal in trackQuest.questGoalList) {
            //detail += goal
        }
    }

    public void RefreshQuest(Quest quest)
    {
        if (quest != null) {
            trackQuest = quest;
        }
    }
}
