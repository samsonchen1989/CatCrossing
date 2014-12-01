using UnityEngine;
using System.Collections;

public class NpcQuestItemUI : MonoBehaviour
{

    #region Public gameObject to drop to in Inspector

    public UISprite questStateIcon;
    public UILabel questTitle;
    public NpcQuestUI questUI;

    #endregion

    public Quest npcQuest;
    static string[] QuestIcon = {
        "",
        "exclamation_mark",
        "question_mark_notdone",
        "question_mark_done",
        ""
    };

    // Use this for initialization
    void Start()
    {
        if (questStateIcon == null || questTitle == null) {
            Debug.LogError("Fail to get NGUI component.");
            return;
        }

        if (npcQuest == null) {
            Debug.LogError("No quest to show.");
            return;
        }

        questStateIcon.spriteName = QuestIcon[(int)npcQuest.progress];
        questTitle.text = npcQuest.title;
    }

    void OnClick()
    {
        switch (npcQuest.progress) {
            case QuestProgress.Eligible:
                questUI.DisplayQuestNewUI(npcQuest);
                break;
            case QuestProgress.Accepted:
                questUI.DisplayQuestUndoneUI(npcQuest);
                break;
            case QuestProgress.Complete:
                questUI.DisplayQuestCompleteUI(npcQuest);
                break;
            default:
                break;
        }
    }
}
