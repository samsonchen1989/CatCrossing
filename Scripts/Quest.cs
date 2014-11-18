using System.Collections;
using System.Collections.Generic;

public enum QuestProgress
{
    NotEligible,
    Eligible,
    Accepted,
    Complete,
    Done
}

[System.Serializable]
public class Quest
{
    public string title;
    public int id;
    public QuestProgress progress;
    public string description;
    public string hint;
    public string target;
    public string completeDesc;
    public int nextQuestId;

    // Npc from who to get this quest
    public int ownerId;
    public List<QuestGoal> questGoalList;
    public List<ItemStack> rewardList;

    public Quest(string title, int id, QuestProgress progress, string description, string target, string hint,
                 string completeDesc, int nextQuestId, List<QuestGoal> questGoalList, List<ItemStack> rewardList)
    {
        this.title = title;
        this.id = id;
        this.progress = progress;
        this.description = description;
        this.target = target;
        this.hint = hint;
        this.completeDesc = completeDesc;
        this.nextQuestId = nextQuestId;

        this.questGoalList = questGoalList;
        this.rewardList = rewardList;
    }

    public Quest ShallowClone()
    {
        return (Quest)this.MemberwiseClone();
    }
}
