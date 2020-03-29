using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using static SceneDelegate;

public class InputManager:MonoBehaviour {
    private Rocket rocketObj;
    private SceneDelegate sceneDelegator;
    private AudioManager gameMasterAudioSource;

    void Awake() {
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start() {
        sceneDelegator = GameObject.Find("Scene Delegate").GetComponent<SceneDelegate>();
        gameMasterAudioSource = GameObject.Find("Game Audio").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update() {
        ProcessInput();
    }

    public void OnSceneLoad() {
        if(currentState != State.Menu) {
            // Only set rocket reference if game is playing, and for each scene load
            rocketObj = GameObject.Find("Rocket Ship").GetComponent<Rocket>();
        }
    }

    private void ProcessInput() {
        if(Debug.isDebugBuild) {
            ProcessDebugKeys();
        }

        if(currentState == State.Running) {
            // Process controls
            if(Input.GetKey(KeyCode.Space)) {
                rocketObj.RocketThrust();
            } else {
                rocketObj.RocketIdle();
            }

            if(Input.GetKey(KeyCode.A)) {
                rocketObj.RocketRotate(false);
            } else if(Input.GetKey(KeyCode.D)) {
                rocketObj.RocketRotate(true);
            }

            if(Input.GetKey(KeyCode.Escape)) {
                sceneDelegator.LoadMenuScene();
            }
        }
        else if(currentState == State.Menu) {
            // Load first game level
            if(Input.GetKey(KeyCode.Space)) {
                sceneDelegator.LoadNextGameScene();
            }
        } else {
            // Only called if in dying or winning game state
            rocketObj.setRocketAudioPitchGoal(0.5f);
        }
    }

    private void ProcessDebugKeys() {
        if(Input.GetKeyDown(KeyCode.L)) {
            // Go to next level
            sceneDelegator.LoadNextGameScene();
        }
        if(Input.GetKeyDown(KeyCode.C)) {
            rocketObj.toggleCollisions();
        }
        if(Input.GetKeyDown(KeyCode.M)) {
            // New music audio
            AudioManager activeGameAudioManager = gameMasterAudioSource.GetComponent<AudioManager>();
            activeGameAudioManager.StopMusic();
            activeGameAudioManager.PlayRandomMusic();
        }
    }
}
