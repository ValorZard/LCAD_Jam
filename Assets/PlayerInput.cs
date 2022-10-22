using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

	public CharacterController2D controller;

	public float runSpeed = 40f;

	float horizontalMove = 0f;
	bool jump = false;

	// Update is called once per frame
	void Update()
	{
		// Move our character
		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

		if (Input.GetButtonDown("Jump"))
		{
			jump = true;
		}
		else
		{
			jump = false;
		}

		controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
		// jump = false;
	}

	void FixedUpdate()
	{

	}
}