using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestNode
{
    int stepID;
    List<Quest> questNodeList;

    public List<Quest> QuestNodeList {
        get {
            return questNodeList;
        }
    }

    public QuestNode(int stepID)
    {
        this.stepID = stepID;
        questNodeList = new List<Quest>();
    }

    public void AddQuest(int questID)
    {
        questNodeList.Add(QuestManager.QuestClone(questID));
    }
}
