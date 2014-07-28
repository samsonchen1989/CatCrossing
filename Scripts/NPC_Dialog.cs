using UnityEngine;
using System.Collections;

public class NPC_Dialog : MonoBehaviour {

	public string[] dialogButton;
	public string[] dialogText;

	private bool clickable = false;
	private bool displayDialog = false;
	private bool activateQuest = false;

	public bool hasDoneQuest = false;

	private Vector3 screenPosition;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		screenPosition = Camera.main.WorldToScreenPoint(this.transform.position);

		RaycastHit hitInfo = new RaycastHit();
		bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity);
		if (hit) {
			GameObject hitObject = hitInfo.transform.gameObject;
			if (hitObject && hitObject.name == "NPC1") {
				if (clickable && Input.GetMouseButtonUp(1)) {
					displayDialog = true;
				}
			}
		}
	}

	void OnGUI()
	{
		//GUI.skin = talkSkin;
		GUILayout.BeginArea(new Rect(screenPosition.x - 20, screenPosition.y + 40, 400, 400));
		if (displayDialog && !activateQuest) {
			GUILayout.Label(dialogText[0]);
			GUILayout.Label(dialogText[1]);
			if (GUILayout.Button(dialogButton[0])) {
				activateQuest = true;
				displayDialog = false;
				hasDoneQuest = false;
			}

			if (GUILayout.Button(dialogButton[1])) {
				displayDialog = false;
			}
		}

		if (displayDialog && activateQuest && hasDoneQuest) {
			GUILayout.Label(dialogText[2]);
			if (GUILayout.Button(dialogButton[2])) {
				displayDialog = false;
			}
		}
		GUILayout.EndArea();
	}

	void OnTriggerEnter(Collider other)
	{
		clickable = true;
	}

	void OnTriggerExit(Collider other)
	{
		clickable = false;
		displayDialog = false;
	}
}
