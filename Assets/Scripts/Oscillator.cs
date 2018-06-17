using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {

    [SerializeField] Vector3 movementVector;
    [SerializeField] float period = 2f;

    [Range(0,1)]
    [SerializeField]
    float movementFactor; // 0 for not moved, 1 for fully moved

    Vector3 startingPos;

	// Use this for initialization
	void Start () {
        startingPos = this.transform.position;	
	}
	
	// Update is called once per frame
	void Update () {                            //Mathf.Epsilon is smallest float number
        if (period <= Mathf.Epsilon){ return; } //protect against zero period

        float cycles = Time.time / period; // grows continually from 0
        const float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycles * tau); // goes from -1 to 1

        movementFactor = rawSinWave / 2f + 0.5f;
        Vector3 offset = movementVector * movementFactor;
        this.transform.position = startingPos + offset;
        
        
        
	}
}
