using UnityEngine;
using System.Collections;

public class NpcQuestProgressUI : MonoBehaviour
{

    #region Public gameObject to drop to in Inspector
    
    public UILabel questName;
    public UILabel questProgress;
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
        if (questName == null || questProgress == null || questDetail == null) {
            Debug.LogError("Fail to find quest progress ui's label.");
            return;
        }

        RefreshUI();
    }

    public void RefreshUI()
    {
        if (trackQuest == null) {
            return;
        }

        questName.text = trackQuest.title;
        questProgress.text = QuestProgressText[(int)trackQuest.progress];

        questDetail.text = null;

        foreach (QuestGoal goal in trackQuest.questGoalList) {
            questDetail.text += goal.ToString() + "\n";
        }
    }

    public void RefreshQuest(Quest quest)
    {
        if (quest != null) {
            trackQuest = quest;
        }
    }
}
