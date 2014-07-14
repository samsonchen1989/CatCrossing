using UnityEngine;
using System.Collections;

public class InteractiveItem : MonoBehaviour {

	public Texture2D mousePickTexture;
	public float maxInteractDis = 3.6f;

	// Use this for initialization
	void Start ()
	{
		mousePickTexture = Resources.Load<Texture2D>("Cursor/Pick");
	}
	
	// Update is called once per frame
	void Update ()
	{
		RaycastHit hitInfo = new RaycastHit();
		bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
		if (hit && hitInfo.transform.parent) {
			GameObject parentObject = hitInfo.transform.parent.gameObject;
			if (parentObject && parentObject.tag == "Interactive Item") {
				GameObject hitItem = hitInfo.transform.gameObject;
				// Just check for right mouse button up action
				if (hitItem && Input.GetMouseButtonUp(1)) {
					if (hitInfo.distance < maxInteractDis) {
						Destroy(hitItem);
					} else {
						Debug.Log("too far");
					}
				}

				Cursor.SetCursor(mousePickTexture, new Vector2(0, 0), CursorMode.Auto);
			}
		} else {
			Cursor.SetCursor(null, new Vector2(0, 0), CursorMode.Auto);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log("Enter interactive item.");
	}

	void OnTriggerExit(Collider other) {
		Debug.Log("Exit interactive item.");
	}

	void OnTriggerStay(Collider other) {
		Debug.Log("Stay interactive item.");
	}
}
