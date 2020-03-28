using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDelegate:MonoBehaviour {
    public enum State {
        Menu,
        Running,
        SceneWon,
        SceneLost
    }
    public static State currentState;

    private bool hasBeenPermanenced = false;

    private void Awake() {
        if(!GameObject.Find("Scene Delegate").GetComponent<SceneDelegate>().hasPermanentStatus()) {
            // Only set DontDestroy once, even on future scene loads
            DontDestroyOnLoad(this);
            setPermanentStatus();
        }
    }

    // Start is called before the first frame update
    void Start() {
        currentState = State.Menu;
    }

    // Update is called once per frame
    void Update() {
        ProcessInput();
    }

    public static void LoadNextScene() {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings) {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    public static void LoadPreviousScene() {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;
        if(nextSceneIndex == 0) {
            // Prevent going back to menu
            nextSceneIndex = 1;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    public void setPermanentStatus() {
        hasBeenPermanenced = true;
    }

    public bool hasPermanentStatus() {
        return hasBeenPermanenced;
    }

    private void ProcessInput() {
        if(currentState == State.Menu) {
            if(Input.GetKey(KeyCode.Space)) {
                LoadNextScene();
            }
        }
    }
}
