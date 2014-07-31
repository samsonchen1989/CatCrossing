using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
	#region Singleton Code

	private static QuestManager instance;
	public static QuestManager Instance
	{
		get {
			if (instance == null) {
				Debug.LogError("Fail to get instance of QuestManager.");
			}

			return instance;
		}
	}

	#endregion

	private Dictionary<int, Quest> questDatabase = new Dictionary<int, Quest>();
	public Dictionary<int, Quest> QuestDatabase
	{
		get {
			return questDatabase;
		}
	}

	void Awake()
	{
		if (instance == null) {
			instance = this;
		} else {
			Debug.LogError("Only one instance of QuestManager is allowed.");
		}

		QuestDatabaseInit();
	}

	// Todo, inti quest database from external file like xml file.
	void QuestDatabaseInit() {
		List<ItemStack> reward = new List<ItemStack>();
		reward.Add(ItemPrefabsDefinition.StackClone(0, 1));
		reward.Add(ItemPrefabsDefinition.StackClone(1, 1));

		List<QuestGoal> goals = new List<QuestGoal>();
		goals.Add(new QuestGoal(0, 5));
		goals.Add(new QuestGoal(1, 3));

		questDatabase.Add(0, new Quest("Hello, new hero", 0, Quest.QuestProgress.Eligible, 
		    "The world is in danger, my deer kitty! You need to level up! Now collect some grass and branch for me, " +
		    "I will reward you.",
		    "You can collect them behind the house.",
		    -1, goals, reward));
	}

}