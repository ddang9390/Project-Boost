using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {
    Rigidbody rigidBody;
    AudioSource audio;

	// Use this for initialization
	void Start () {
        rigidBody = this.GetComponent<Rigidbody>();
        audio = this.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        ProcessInput();
	}

    private void ProcessInput()
    {
        // Can thrust while rotating
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up);
            if (!audio.isPlaying) // so it doesn't layer on top of each other
            {
                audio.Play();
            }
        }
        else if (audio.isPlaying)
        {
            audio.Stop();
        }
        // Rotate ship
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back);
        }
    }
}
