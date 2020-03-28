using System;
using UnityEngine;
using static SceneDelegate;

public class Rocket:MonoBehaviour {
    private Rigidbody rocketBody;
    private AudioSource rocketAudioSource;

    private AudioManager gameMasterAudioSource;
    private SceneDelegate sceneDelegator;

    private bool collisionsEnabled = true;
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
        SetComponents();
        currentState = State.Running;
    }

    // Update is called once per frame
    void Update() {
        HandleAudio();
        rocketBody.angularVelocity = Vector3.zero;
    }

    public void toggleCollisions() {
        collisionsEnabled = !collisionsEnabled;
    }

    public void setRocketAudioPitchGoal(float audioPitch) {
        pitchGoal = audioPitch;
    }

    public void RocketRotate(bool thrustRight) {
        // Rotate left
        transform.Rotate((thrustRight ? Vector3.back : Vector3.forward) * rotationMultiplier * Time.deltaTime);
    }

    public void RocketThrust() {
        // Thrusting
        rocketBody.AddRelativeForce(Vector3.up * thrustMultiplier * Time.deltaTime);
        setRocketAudioPitchGoal(3f);
        engineParticles.Play();
    }

    public void RocketIdle() {
        // Not thrusting
        engineParticles.Stop();
        setRocketAudioPitchGoal(0.5f);
    }

    private void SetComponents() {
        rocketBody = GetComponent<Rigidbody>();
        rocketAudioSource = GetComponent<AudioSource>();
        gameMasterAudioSource = GameObject.Find("Game Audio").GetComponent<AudioManager>();
        sceneDelegator = GameObject.Find("Scene Delegate").GetComponent<SceneDelegate>();
        rocketAudioSource.volume = 0.3f;
    }

    private void OnCollisionEnter(Collision collision) {
        if(currentState != State.Running || !collisionsEnabled) { return; }
        switch(collision.gameObject.tag) {
            case "Friendly":
                // Do nothing
                break;
            case "Finish":
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
        rocketAudioSource.pitch = 1f;
        rocketAudioSource.volume = 1f;
        advanceParticles.Play();

        Invoke("CallLoadNextScene", levelLoadDelay);
    }

    private void DeathSequence() {
        currentState = State.SceneLost;

        rocketAudioSource.Stop();
        rocketAudioSource.volume = 1f;
        rocketAudioSource.PlayOneShot(explodeAudio);

        engineParticles.Stop();
        transform.localScale = new Vector3(0, 0, 0);
        explodeParticles.Play();

        Invoke("CallLoadPreviousScene", levelLoadDelay);
    }

    private void CallLoadNextScene() {
        sceneDelegator.LoadNextGameScene();
    }

    private void CallLoadPreviousScene() {
        sceneDelegator.LoadPreviousGameScene();
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
}