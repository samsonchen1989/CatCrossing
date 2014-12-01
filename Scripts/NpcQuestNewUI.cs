using UnityEngine;
using System.Collections;

public class NpcQuestNewUI : MonoBehaviour {

    #region Public gameObject to drop to in Inspector

    public UILabel questDesc;
    public UILabel questTarget;
    public UILabel questReward;
    public UILabel questTitle;

    #endregion

    public Quest currentQuest;

    // Use this for initialization
    void Start()
    {
        if (questDesc == null || questTarget == null || questReward == null || questTitle == null) {
            Debug.LogError("Fail to get new quest UI's label.");
            return;
        }

        RefreshUI();
    }

    private void RefreshUI()
    {
        if (currentQuest == null) {
            return;
        }

        questTitle.text = currentQuest.title;
        questDesc.text = currentQuest.description;
        questTarget.text = currentQuest.hint;
    }

    public void RefreshQuest(Quest quest)
    {
        if (quest != null) {
            this.currentQuest = quest;
            RefreshUI();
        }
    }

    public void AcceptCurrentQuest()
    {
        if (currentQuest != null) {
            QuestManager.Instance.AcceptQuest(currentQuest.id);
        }

        foreach (QuestGoal goal in currentQuest.questGoalList) {
            if (goal.GetQuestType() == QuestType.Visit) {
                int newNpc = ((QuestGoalVisit)goal).targetNpcId;
                // Contains visit content, change npc's quest list
                QuestDispatcher.Instance.ChangeQuestOwner(currentQuest, currentQuest.ownerId, newNpc);

                if (currentQuest.questGoalList.Count == 1) {
                    // Only one visit goal
                    QuestManager.Instance.CompleteQuest(currentQuest.id);
                }
            }
        }
    }
}
