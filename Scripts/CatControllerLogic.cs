﻿using UnityEngine;
using System.Collections;

public class CatControllerLogic : MonoBehaviour {
	enum CatState {
		Idle,
		Jump,
		Running
	}	

	#region variables (private)

	[SerializeField]
	private Animator animator;
	[SerializeField]
	private Rigidbody playerBody;
	[SerializeField]
	private Transform gameCamTransform;

	// Private global only
	private float speed = 0.0f;
	private float curDirection = 0.0f;
	private float leftX = 0.0f;
	private float leftY = 0.0f;
	private float speedDampTime = 0.05f;

	private Vector3 curNormal = Vector3.up; // smoothed terrain normal
	private bool onGround;

	private CatState state = CatState.Idle;

	private RaycastHit hit;

	private const float inputThreshold = 0.01f;	
	private const float groundDrag = 5.0f;
	private const float turnSpeed = 3.0f;

	#endregion

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		gameCamTransform = GameObject.FindWithTag("MainCamera").transform;

		if (animator.layerCount >= 2) {
			animator.SetLayerWeight(1, 1);
		}

		if (playerBody == null) {
			playerBody = GetComponent<Rigidbody>();
		}

		playerBody.freezeRotation = true;

		if (gameCamTransform == null) {
			Debug.LogError("Couldn't find game main camera");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (animator) {
			//pull values from controller/keyboard, A/D to rotate, W/S to move
			leftX = Input.GetAxis("Horizontal");
			leftY = Input.GetAxis("Vertical");

			speed = leftY;
			animator.SetFloat("Speed", speed, speedDampTime, Time.deltaTime);

			if (state == CatState.Jump) {
				animator.SetBool("Jump", true);
			} else {
				animator.SetBool("Jump", false);
			}
		}

		if (Input.GetMouseButton(1)) {
			curDirection = gameCamTransform.rotation.eulerAngles.y;
			//Debug.Log("curDirection:" + curDirection);
		}

		float rotationAmount = leftX * turnSpeed;
		curDirection = (curDirection + rotationAmount) % 360;
		
		Ray ray = new Ray(transform.position, -1 * Vector3.up);
		if (Physics.Raycast(ray, out hit)) {
			Debug.DrawLine(ray.origin, hit.point, Color.blue);
			//Debug.Log("hit distance:" + hit.distance);
			curNormal = Vector3.Lerp(curNormal, hit.normal, 4 * Time.deltaTime);
			Quaternion rotateGround = Quaternion.FromToRotation (Vector3.up, curNormal);

			transform.rotation = rotateGround * Quaternion.Euler(0, curDirection, 0);
			//Debug.Log("curDirection:" + curDirection);

			if (hit.distance < 0.1) {
				onGround = true;
			} else {
				onGround = false;
			}

			if (!onGround && state != CatState.Jump) {
				transform.position = hit.point;
			}
		}
	}

	void FixedUpdate()
	{
		if (onGround) {
			float moveAxis = Input.GetAxis("Vertical");
			Vector3 movement = moveAxis * playerBody.transform.forward;
			if (movement.magnitude > inputThreshold && moveAxis > 0) {
				playerBody.AddForce(movement.normalized * 5.0f, ForceMode.VelocityChange);
				state = CatState.Running;
			} else {
				playerBody.velocity = new Vector3(0.0f, playerBody.velocity.y, 0.0f);
			}
		}
	}
}
