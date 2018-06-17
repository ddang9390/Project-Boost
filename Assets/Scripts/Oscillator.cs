using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {

    [SerializeField] Vector3 movementVector;

    // TODO remove from inspector later
    [Range(0,1)]
    [SerializeField]
    float movementFactor; // 0 for not moved, 1 for fully moved

    bool increase = false;
    bool decrease = false;
    Vector3 startingPos;

	// Use this for initialization
	void Start () {
        startingPos = this.transform.position;	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 offset = movementVector * movementFactor;
        this.transform.position = startingPos + offset;
        if(movementFactor <= 0)
        {
            movementFactor += 0.01f;
            increase = true;
            decrease = false;
        }
        else if(movementFactor >= 1)
        {
            movementFactor -= 0.01f;
            increase = false;
            decrease = true;
        }
        else if (increase && !decrease)
        {
            movementFactor += 0.01f;
        }
        else if (decrease && !increase)
        {
            movementFactor -= 0.01f;
        }
	}
}
