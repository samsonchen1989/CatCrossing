using UnityEngine;
using System.Collections;

public class GameSceneManager : MonoBehaviour
{
    // Maybe game state not proper here.
    private IStateBase activeState;

    private static GameSceneManager instance;
    public static string StartScene = "Scene0";
    public static string PlayScene = "Scene1";

    void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            DestroyImmediate(this.gameObject);
        }
    }

    void Start()
    {
        if (Application.loadedLevelName != StartScene) {
            Application.LoadLevel(StartScene);
        }
    }
    
    void Update()
    {
        if (activeState != null) {
            activeState.StateUpdate();
        }
    }

    public void SwitchState(IStateBase newState)
    {
        activeState = newState;
    }

    // "Play" button click delegate
    public void Play()
    {
        if (Application.loadedLevelName != PlayScene) {
            Application.LoadLevel(PlayScene);
        }
    }

    // Quit function will be ignored in Editor or web application
    public void Quit()
    {
        Application.Quit();
    }
}
