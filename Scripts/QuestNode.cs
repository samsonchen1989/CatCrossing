using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestNode
{
    int stepID;
    List<Quest> questList;

    public List<Quest> QuestList {
        get {
            return questList;
        }
    }

    public QuestNode(int stepID)
    {
        this.stepID = stepID;
        questList = new List<Quest>();
    }

    public void AddQuest(int questID)
    {
        questList.Add(QuestManager.QuestClone(questID));
    }
}
