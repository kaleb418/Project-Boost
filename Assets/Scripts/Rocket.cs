using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket:MonoBehaviour {
    private Rigidbody rocketBody;
    private AudioSource rocketAudioSource;

    private enum State {
        Running,
        SceneWon,
        SceneLost
    }
    private State currentState;

    private double pitchGoal = 0.5f;
    private double deltaPitch = 0.05;

    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] float thrustMultiplier = 700f;
    [SerializeField] float rotationMultiplier = 50f;

    [SerializeField] AudioClip engineAudio;
    [SerializeField] AudioClip advanceAudio;
    [SerializeField] AudioClip explodeAudio;

    [SerializeField] ParticleSystem engineParticles;
    [SerializeField] ParticleSystem advanceParticles;
    [SerializeField] ParticleSystem explodeParticles;

    // Start is called before the first frame update
    void Start() {
        currentState = State.Running;
        SetComponents();
    }

    private void SetComponents() {
        rocketBody = GetComponent<Rigidbody>();
        rocketAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        ProcessInput();
        HandleAudio();
    }

    private void OnCollisionEnter(Collision collision) {
        if(currentState != State.Running) { return; }

        switch(collision.gameObject.tag) {
            case "Friendly":
                // Do nothing
                break;
            case "Fuel":
                // Fuel ship - todo Add fuel aspect
                break;
            case "Finish":
                // Win level - todo Win on landing, not hitting side
                AdvanceSequence();
                break;
            default:
                // Die
                DeathSequence();
                break;
        }
    }

    private void AdvanceSequence() {
        currentState = State.SceneWon;

        rocketAudioSource.Stop();
        rocketAudioSource.PlayOneShot(advanceAudio);

        engineParticles.Stop();
        advanceParticles.Play();

        Invoke("LoadNextScene", levelLoadDelay);
    }

    private void DeathSequence() {
        currentState = State.SceneLost;

        rocketAudioSource.Stop();
        rocketAudioSource.PlayOneShot(explodeAudio);

        engineParticles.Stop();
        explodeParticles.Play();

        Invoke("LoadFirstScene", levelLoadDelay);
    }

    private void LoadNextScene() {
        SceneManager.LoadScene(1);
    }

    private void LoadFirstScene() {
        SceneManager.LoadScene(0);
    }

    private void HandleAudio() {
        if(!rocketAudioSource.isPlaying && currentState == State.Running) {
            rocketAudioSource.PlayOneShot(engineAudio);
        }
        if(Math.Abs(pitchGoal - rocketAudioSource.pitch) > deltaPitch) {
            rocketAudioSource.pitch += + (float) (Math.Sign(pitchGoal - rocketAudioSource.pitch) * deltaPitch);
        } else {
            rocketAudioSource.pitch = (float) pitchGoal;
        }
    }

    private void ProcessInput() {
        rocketBody.freezeRotation = true;
        if(currentState == State.Running) {
            if(Input.GetKey(KeyCode.Space)) {
                // Thrusting
                rocketBody.AddRelativeForce(Vector3.up * thrustMultiplier * Time.deltaTime);
                pitchGoal = 3f;
                engineParticles.Play();
            } else {
                engineParticles.Stop();
                pitchGoal = 0.5f;
            }

            if(Input.GetKey(KeyCode.A)) {
                // Rotate left
                transform.Rotate(Vector3.forward * rotationMultiplier * Time.deltaTime);
            }
            else if(Input.GetKey(KeyCode.D)) {
                // Rotate right
                transform.Rotate(Vector3.back * rotationMultiplier * Time.deltaTime);
            }
        } else {
            pitchGoal = 0.5f;
        }
        rocketBody.freezeRotation = false;
    }
}