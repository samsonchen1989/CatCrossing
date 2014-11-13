using UnityEngine;
using System.Collections;

public class NpcQuestNewUI : MonoBehaviour {

    #region Public gameObject to drop to in Inspector

    public UILabel questDesc;
    public UILabel questTarget;
    public UILabel questReward;
    public UILabel questTitle;

    #endregion

    public Quest quest;

    // Use this for initialization
    void Start()
    {
        if (questDesc == null || questTarget == null || questReward == null || questTitle == null) {
            Debug.LogError("Fail to get new quest UI's label.");
            return;
        }

        if (quest != null) {
            questTitle.text = quest.title;
            questDesc.text = quest.description;
            questTarget.text = quest.hint;
        }
    }
	
    // Update is called once per frame
    void Update()
    {
	
    }

    public void AcceptCurrentQuest()
    {
        if (quest != null) {
            QuestManager.Instance.AcceptQuest(quest.id);
        }
    }
}
