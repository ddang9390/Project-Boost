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
    [SerializeField] float levelLoadDelay = 1f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip winSound;

    [SerializeField] ParticleSystem thrusterParticle;
    [SerializeField] ParticleSystem explosionParticle;
    [SerializeField] ParticleSystem successParticle;

    bool collisionsEnabled = true;

    enum State { live, dying, transcend};
    State state = State.live;

    // Use this for initialization
    void Start () {
        rigidBody = this.GetComponent<Rigidbody>();
        audioSource = this.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Debug.isDebugBuild)
        {
            respondToDebugKeys();
        }
        if (state == State.live)
        {
            thrust();
            rotate();
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
                audioSource.PlayOneShot(mainEngine);
            }
            thrusterParticle.Play();
        }
        else if (audioSource.isPlaying)
        {
            audioSource.Stop();
            thrusterParticle.Stop();
        }
    }

    // Only works if 'debug' is true
    private void respondToDebugKeys()
    {

        if (Input.GetKeyDown(KeyCode.L))
        {
            loadNextScene();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            // toggle collision
            collisionsEnabled = !collisionsEnabled;
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(state != State.live || !collisionsEnabled){return;} // ignore collision if dead or transcending

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;

            case "Fuel":
                
                break;

            case "Goal":
                state = State.transcend;
                audioSource.Stop();
                audioSource.PlayOneShot(winSound);
                successParticle.Play();
                Invoke("loadNextScene", levelLoadDelay);
                break;

            default:
                state = State.dying;
                audioSource.Stop();
                explosionParticle.Play();
                audioSource.PlayOneShot(deathSound);
                Invoke("gameOver", levelLoadDelay);
                
                break;
        }
    }

    private void gameOver()
    {
        SceneManager.LoadScene(0);
    }

    private void loadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if(nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        state = State.live;   
        SceneManager.LoadScene(nextSceneIndex);
    }
}
