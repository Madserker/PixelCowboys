using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class movement : MonoBehaviour
	{
	public float speed = 6.0f;
	public float run = 2.0f;
	public float jumpSpeed = 8.0f;
	public float gravity = 20.0f;

	private Animator anim;

	private Vector3 moveDirection = Vector3.zero;
	private CharacterController controller;

	private float lastJump = 0.0f;


	public float mouseSensitivity = 100.0f;
	public float clampAngle = 80.0f;

	private float rotY = 0.0f; // rotation around the up/y axis
	private float rotX = 0.0f; // rotation around the right/x axis



	void Start()
	{
		controller = GetComponent<CharacterController>();
		anim = GetComponent<Animator>();

		// let the gameObject fall down
		//gameObject.transform.position = new Vector3(0, 5, 0);

		Vector3 rot = transform.localRotation.eulerAngles;
		rotY = rot.y;
		rotX = rot.x;
	}

	void Update()
	{

		gameObject.name = "Local";

		if (controller.isGrounded)
		{
			// We are grounded, so recalculate
			// move direction directly from axes

			moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection = moveDirection * speed;

			if (Input.GetButton("Jump"))
			{
				moveDirection.y = jumpSpeed;
				lastJump = jumpSpeed;
			}
			if (Input.GetKey(KeyCode.LeftShift)) {
				moveDirection.x *= run;
				moveDirection.z *= run;
			}


		}
		else {
			if (lastJump > 0) {
				moveDirection = new Vector3(Input.GetAxis("Horizontal"), lastJump, Input.GetAxis("Vertical"));
				moveDirection = transform.TransformDirection(moveDirection);
				moveDirection.x = moveDirection.x * (speed / 2);
				moveDirection.z = moveDirection.z * (speed / 2);
			}

		}



		// Apply gravity
		lastJump = lastJump - (gravity * Time.deltaTime);
		moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);

		// Move the controller
		controller.Move(moveDirection * Time.deltaTime);


		//ROTATION==================================================
		float mouseX = Input.GetAxis("Mouse X");
		float mouseY = -Input.GetAxis("Mouse Y");

		rotY += mouseX * mouseSensitivity * Time.deltaTime;
		rotX += mouseY * mouseSensitivity * Time.deltaTime;

		rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

		Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
		transform.rotation = localRotation;
	}
}