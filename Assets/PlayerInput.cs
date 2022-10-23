using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

		// pretend that this is the restart button
		if (Input.GetButtonDown("Fire3"))
        {
			Scene scene = SceneManager.GetActiveScene(); 
			SceneManager.LoadScene(scene.name);
		}
	}

	void FixedUpdate()
	{

	}
}