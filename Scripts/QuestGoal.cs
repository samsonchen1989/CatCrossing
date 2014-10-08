using UnityEngine;
using System.Collections;

public class QuestGoal {
	public bool complete;
	
	public QuestGoal(bool complete = false)
	{
		this.complete = complete;
	}	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

public class QuestGoalCollectItem : QuestGoal {
	public int itemId;
	public int currentNumber;
	public int targetNumber;

	public QuestGoalCollectItem(int itemId, int targetNumber, int currentNumber = 0, bool complete = false) : base(complete)
	{
		this.itemId = itemId;
		this.targetNumber = targetNumber;
		this.currentNumber = currentNumber;
	}
}

public class QuestGoalKill : QuestGoal
{
	public int enemyId;
	public int currentNumber;
	public int targetNumber;

	public QuestGoalKill(int enemyId, int targetNumber, int currentNumber = 0, bool complete = false) : base(complete)
	{
		this.enemyId = enemyId;
		this.targetNumber = targetNumber;
		this.currentNumber = currentNumber;
	}
}

public class QuestGoalVisit : QuestGoal
{
	public int targetNpcId;

	public QuestGoalVisit(int targetNpcId, bool complete = false) : base(complete)
	{
		this.targetNpcId = targetNpcId;
	}
}
