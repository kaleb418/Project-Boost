using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator:MonoBehaviour {

    [SerializeField] Vector3 movementVector;
    [SerializeField] [Range(0, 1)] float oscillationSpeed;
    [SerializeField] bool scaleOscillation = false;

    private float oscillationMultiplier;
    private Vector3 startingPosition;
    private float timeSinceStart;

    // Start is called before the first frame update
    void Start() {
        if(scaleOscillation) {
            startingPosition = transform.localScale;
        } else {
            startingPosition = transform.position;
        }
    }

    // Update is called once per frame
    void Update() {
        if(oscillationSpeed > Mathf.Epsilon) {
            timeSinceStart = Time.deltaTime + timeSinceStart;
            oscillationMultiplier = (float) Math.Sin((float) timeSinceStart / oscillationSpeed);

            if(scaleOscillation) {
                transform.localScale = startingPosition + movementVector * oscillationMultiplier;
            } else {
                transform.position = startingPosition + movementVector * oscillationMultiplier;
            }
        }
    }
}