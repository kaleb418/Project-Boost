using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket:MonoBehaviour {
    Rigidbody rocketBody;
    AudioSource audio;

    private double pitchGoal = 0.5f;
    private double deltaPitch = 0.05;

    [SerializeField] float thrustMultiplier = 700f;
    [SerializeField] float rotationMultiplier = 50f;

    // Start is called before the first frame update
    void Start() {
        rocketBody = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        ProcessInput();
        moveAudioPitch();
    }

    private void OnCollisionEnter(Collision collision) {
        switch(collision.gameObject.tag) {
            case "Friendly":
                // Do nothing
                print("Friendly");
                break;
            case "Fuel":
                // Fuel ship
                print("Fuel");
                break;
            case "Finish":
                // Win game - todo Win on landing, not hitting side
                print("Finish");
                break;
            default:
                // Die
                print("Death");
                break;
        }
    }

    private void moveAudioPitch() {
        if(Math.Abs(pitchGoal - audio.pitch) > deltaPitch) {
            audio.pitch += + (float) (Math.Sign(pitchGoal - audio.pitch) * deltaPitch);
        } else {
            audio.pitch = (float) pitchGoal;
        }
    }

    private void ProcessInput() {
        if(Input.GetKey(KeyCode.Space)) {
            // Thrusting
            rocketBody.AddRelativeForce(Vector3.up * thrustMultiplier * Time.deltaTime);
            pitchGoal = 3f;
        } else {
            pitchGoal = 0.5f;
        }

        rocketBody.freezeRotation = true;
        if(Input.GetKey(KeyCode.A)) {
            // Rotate left
            transform.Rotate(Vector3.forward * rotationMultiplier * Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.D)) {
            // Rotate right
            transform.Rotate(Vector3.back * rotationMultiplier * Time.deltaTime);
        }
        rocketBody.freezeRotation = false;
    }
}