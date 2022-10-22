using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

	public CharacterController2D controller;

	float horizontalMove = 0f;
	bool jump = false;

	// Update is called once per frame
	void Update()
	{

		// pretend that this is the "Go Ghost" button
		if (Input.GetButtonDown("Fire1"))
		{
			controller.GoGhost();
		}

		// Move our character
		horizontalMove = Input.GetAxisRaw("Horizontal");

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