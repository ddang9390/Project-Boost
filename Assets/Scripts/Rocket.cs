using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {
    Rigidbody rigidBody;
    AudioSource audioSource;

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

    enum State { live, dying, transcend};
    State state = State.live;

    // Use this for initialization
    void Start () {
        rigidBody = this.GetComponent<Rigidbody>();
        audioSource = this.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (state == State.live)
        {
            thrust();
            rotate();
        }
        else
        {
            audioSource.Stop();
        }
	}

    private void rotate()
    {
        rigidBody.freezeRotation = true; // take manual control of rotation

        float rotationSpeed = rcsThrust * Time.deltaTime;
        // Rotate ship
        if (Input.GetKey(KeyCode.A))
        {
            
            transform.Rotate(Vector3.forward * rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back * rotationSpeed);
        }

        rigidBody.freezeRotation = false; // resume control of physics
    }

    private void thrust()
    {

        float thrustSpeed = mainThrust * Time.deltaTime;
        // Can thrust while rotating
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * thrustSpeed);
            if (!audioSource.isPlaying) // so it doesn't layer on top of each other
            {
                audioSource.Play();
            }
        }
        else if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(state != State.live){return;} // ignore collision if dead or transcending

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;

            case "Fuel":
                
                break;

            case "Goal":
                state = State.transcend;
                Invoke("loadNextScene", 1f); // parameterise time
                break;

            default:
                state = State.dying;
                Invoke("gameOver", 1f);
                
                break;
        }

    }

    private void gameOver()
    {
        SceneManager.LoadScene(0);
    }

    private void loadNextScene()
    {
        state = State.live;
        SceneManager.LoadScene(1);
    }
}
