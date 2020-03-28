using System;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator:MonoBehaviour {

    [SerializeField] Vector3 movementVector;
    [SerializeField] [Range(0, 1)] float oscillationSpeed;
    enum OscillationType {
        Linear,
        Scaled
    }
    [SerializeField] OscillationType oscillationType = OscillationType.Linear;

    private float oscillationMultiplier;
    private Vector3 startingPosition;
    private float timeSinceStart;

    // Start is called before the first frame update
    void Start() {
        if(oscillationType == OscillationType.Scaled) {
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

            if(oscillationType == OscillationType.Scaled) {
                transform.localScale = startingPosition + movementVector * oscillationMultiplier;
            } else {
                transform.position = startingPosition + movementVector * oscillationMultiplier;
            }
        }
    }
}