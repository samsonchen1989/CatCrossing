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
	private float curDirection = 0.0f;
	private float leftX = 0.0f;
	private float leftY = 0.0f;
	private float speedDampTime = 0.05f;

	private CharacterController controller;
	private float speed = 6.0f;
	private float jumpSpeed = 15.0f;
	private float gravity = 60.0f;
	private bool canJump = true;

	private bool onGround;
	private Vector3 curNormal = Vector3.up; // smoothed terrain normal
	private Vector3 moveDirection = Vector3.zero;

	private CatState state = CatState.Idle;
	private RaycastHit hit;

	private const float inputThreshold = 0.01f;	
	private const float groundDrag = 5.0f;
	private const float turnSpeed = 2.0f;

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

		controller = GetComponent<CharacterController>();
	}

	// Update is called once per frame
	void Update () {
		if (animator) {
			//pull values from controller/keyboard, A/D to rotate, W/S to move
			leftX = Input.GetAxis("Horizontal");
			leftY = Input.GetAxis("Vertical");

			// W,S to move character
			animator.SetFloat("Speed", leftY, speedDampTime, Time.deltaTime);
			
			if (state == CatState.Jump) {
				animator.SetBool("Jump", true);
			} else {
				animator.SetBool("Jump", false);
			}
		}
		
		if (Input.GetMouseButton(1)) {
			curDirection = gameCamTransform.rotation.eulerAngles.y;
		}
		
		float rotationAmount = leftX * turnSpeed;
		curDirection = (curDirection + rotationAmount) % 360;

		transform.rotation = Quaternion.Euler(0, curDirection, 0);
		
		moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical"));
		moveDirection = transform.TransformDirection(moveDirection);
		moveDirection *= speed;

		if (Input.GetButton("Jump") && canJump) {
			moveDirection.y = jumpSpeed;
			canJump = false;
		}

		Ray ray = new Ray(transform.position, -1 * Vector3.up);
		if (Physics.Raycast(ray, out hit)) {
			if (hit.distance <= 0.01) {
				canJump = true;
			}
		}

		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);
	}

	/* If you prefer to use Rigidbody and Capsule Collider to control the player,
	 * you can use code below, add force to push player.
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
			//Debug.Log("Hit distance:" + hit.distance);
			curNormal = Vector3.Lerp(curNormal, hit.normal, 4 * Time.deltaTime);
			//Quaternion rotateGround = Quaternion.FromToRotation (Vector3.up, curNormal);
			//transform.rotation = rotateGround * Quaternion.Euler(0, curDirection, 0);
			transform.rotation = Quaternion.Euler(0, curDirection, 0);
			Debug.Log("Set rotation");

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
	*/

	/*
	void FixedUpdate()
	{
		if (onGround) {
			float moveAxis = Input.GetAxis("Vertical");
			Vector3 movement = moveAxis * playerBody.transform.forward;
			if (movement.magnitude > inputThreshold && moveAxis > 0) {
				playerBody.AddForce(movement.normalized * 5.0f, ForceMode.VelocityChange);
				Debug.Log("Add force");
				state = CatState.Running;
			} else {
				playerBody.velocity = new Vector3(0.0f, playerBody.velocity.y, 0.0f);
			}
		}
	}
	*/
}