using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator:MonoBehaviour {

    [SerializeField] Vector3 movementVector;
    [SerializeField] [Range(0, 1)] float oscillationSpeed;

    private float oscillationMultiplier;
    private Vector3 startingPosition;
    private float timeSinceStart;

    // Start is called before the first frame update
    void Start() {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update() {
        if(oscillationSpeed > Mathf.Epsilon) {
            timeSinceStart = Time.deltaTime + timeSinceStart;
            oscillationMultiplier = (float) Math.Sin((float) timeSinceStart / oscillationSpeed);
            transform.position = startingPosition + movementVector * oscillationMultiplier;
        }
    }
}