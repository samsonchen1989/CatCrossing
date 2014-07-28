using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestPrefabsDefinition : MonoBehaviour
{
	#region Singleton Code

	private static QuestPrefabsDefinition instance;
	public static QuestPrefabsDefinition Instance {
		get {
			if (instance == null) {
				Debug.LogError("No QuestManager instance.");
			}

			return instance;
		}
	}

	void Awake()
	{
		if (instance == null) {
			instance = this;
		} else {
			Debug.LogError("Only one instance of QuestManager is allowed.");
		}
	}

	#endregion
}