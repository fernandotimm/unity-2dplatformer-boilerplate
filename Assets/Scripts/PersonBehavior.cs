using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonBehavior : MonoBehaviour {
    
    public CharacterController2D controller;

	public float runSpeed = 40f;

	float horizontalMove = 0f;
	bool jump = false;

    void Start() {
        
    }

    void Update() {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

		if (Input.GetButtonDown("Jump")) {
			jump = true;
		}

        gameObject.GetComponent<Animator>().SetBool("Run", Mathf.Abs(horizontalMove) > 0);
    }

    void FixedUpdate() {
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
		jump = false;
    }

}
