using UnityEngine;
using System.Collections;

public class NpcQuestCompleteUI : MonoBehaviour
{
    #region Public gameObject to drop to in Inspector
    
    public UILabel questTitle;
    public UILabel questCompleteDesc;
    public UILabel questReward;
    
    #endregion

    private Quest currentQuest;

    // Use this for initialization
    void Start()
    {
        if (questTitle == null || questCompleteDesc == null || questReward == null) {
            Debug.LogError("Fail to find NpcQuestCompletePanel's label.");
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
        questCompleteDesc.text = currentQuest.completeDesc;
    }
    
    public void RefreshQuest(Quest quest)
    {
        if (quest != null) {
            currentQuest = quest;
            RefreshUI();
        }
    }

    public void DoneCurrentQuest()
    {
        if (currentQuest == null) {
            return;
        }

        QuestManager.Instance.DoneQuest(currentQuest.id);
    }
}
