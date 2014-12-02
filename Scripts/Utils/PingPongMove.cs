using UnityEngine;
using System.Collections;

public class PingPongMove : MonoBehaviour {

    // Direction to move
    public Vector3 moveDir = Vector3.zero;

    // Speed to move, units per second
    public float speed = 0.0f;

    // Distance to travel in world units
    public float travelDistance = 0.0f;

    // Cached transform
    private Transform thisTransform = null;

	// Use this for initialization
	IEnumerator Start () {
        thisTransform = transform;
        while(true) {
            moveDir = moveDir * -1;
            // Start movement, yield return StartCoroutine will wait until Travel finished
            yield return StartCoroutine(Travel());
        }
	}
	
    // Travel full distance in direction, from current position
    IEnumerator Travel()
    {
        // Distance travelled so far
        float distanceTravelled = 0;

        // Move
        while(distanceTravelled < travelDistance) {
            // Get new position based on speed and direction
            Vector3 disToTravel = moveDir * speed * Time.deltaTime;
            // Update position
            thisTransform.position += disToTravel;
            distanceTravelled += disToTravel.magnitude;

            yield return null;
        }
    }
}
