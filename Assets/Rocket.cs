using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket:MonoBehaviour {

    Rigidbody rocketBody;

    // Start is called before the first frame update
    void Start() {
        rocketBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        ProcessInput();
    }

    private void ProcessInput() {
        if(Input.GetKey(KeyCode.Space)) {
            // Thrusting
            rocketBody.AddRelativeForce(Vector3.up);
        }

        if(Input.GetKey(KeyCode.A)) {
            // Rotate left
            transform.Rotate(Vector3.forward);
        }
        else if(Input.GetKey(KeyCode.D)) {
            // Rotate right
            transform.Rotate(Vector3.back);
        }
    }
}
