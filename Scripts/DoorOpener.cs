using UnityEngine;
using System.Collections;

public class DoorOpener : MonoBehaviour {

	public GameObject doorObject;
	public bool doorOpened = false;

	void OnTriggerEnter(Collider other)
	{
		if (!doorOpened) {
			Animation openAni = doorObject.animation;
			if (openAni) {
				openAni.Play();
				doorOpened = true;
			}
		}
	}
}
