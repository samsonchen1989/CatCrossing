using UnityEngine;
using System.Collections;

public class NpcQuestUndoneUI : MonoBehaviour
{
    #region Public gameObject to drop to in Inspector

    public UILabel questTitle;
    public UILabel questHint;

    #endregion

    private Quest currentQuest;

    // Use this for initialization
    void Start()
    {
        if (questHint == null || questTitle == null) {
            Debug.LogError("Fail to find questUndoUI's label.");
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
        questHint.text = currentQuest.hint;
    }

    public void RefreshQuest(Quest quest)
    {
        if (quest != null) {
            currentQuest = quest;
            RefreshUI();
        }
    }
}
