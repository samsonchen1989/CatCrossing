using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Quest
{
    public string title;
    public int id;
    public QuestProgress progress;
    public string description;
    public string hint;
    public int nextQuestId;

    // Npc from who to get this quest
    public int ownerId;

    public enum QuestProgress
    {
        NotEligible,
        Eligible,
        Accepted,
        Complete,
        Done
    }

    public List<QuestGoal> questGoalList;
    public List<ItemStack> rewardList;

    public Quest(string title, int id, QuestProgress progress, string description, string hint, int nextQuestId,
	             List<QuestGoal> questGoalList, List<ItemStack> rewardList)
    {
        this.title = title;
        this.id = id;
        this.progress = progress;
        this.description = description;
        this.hint = hint;
        this.nextQuestId = nextQuestId;

        this.questGoalList = questGoalList;
        this.rewardList = rewardList;
    }

    public Quest ShallowClone()
    {
        return (Quest)this.MemberwiseClone();
    }
}
