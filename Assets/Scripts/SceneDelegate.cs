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

    private InputManager inputManager;

    void Awake() {
        DontDestroyOnLoad(this);

        SceneManager.sceneLoaded += OnSceneLoad;
    }

    // Start is called before the first frame update
    void Start() {
        // Load menu scene
        LoadMenuScene();
        currentState = State.Menu;
    }

    // Update is called once per frame
    void Update() {
    }

    // Called on scene load
    public void OnSceneLoad(Scene scene, LoadSceneMode mode) {
        inputManager = GameObject.Find("Input Manager").GetComponent<InputManager>();
        inputManager.OnSceneLoad();
    }

    public void LoadMenuScene() {
        currentState = State.Menu;
        SceneManager.LoadScene(1);
    }

    public void LoadNextGameScene() {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings) {
            // Load menu
            nextSceneIndex = 2; // Goes back to level 1
        }
        currentState = State.Running;
        SceneManager.LoadScene(nextSceneIndex);
    }

    public void LoadPreviousGameScene() {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;
        if(nextSceneIndex == 1) {
            // Prevent going back to menu
            nextSceneIndex = 2;
        }
        currentState = State.Running;
        SceneManager.LoadScene(nextSceneIndex);
    }
}
