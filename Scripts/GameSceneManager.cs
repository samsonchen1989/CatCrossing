using UnityEngine;
using System.Collections;

public class GameSceneManager : MonoBehaviour
{
    // Maybe game state not proper here.
    private IStateBase activeState;

    private static GameSceneManager instance;

    public static GameSceneManager Instance
    {
        get {
            if (instance == null) {
                Debug.LogError("Fail to get GameSceneManager instance.");
            }

            return instance;
        }
    }

    public static string StartScene = "Scene0";
    public static string PlayScene = "Scene1";
    public static string LoadScene = "Loading";

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

    void OnDisable()
    {
        PhotonNetwork.Disconnect();
    }

    public void SwitchState(IStateBase newState)
    {
        activeState = newState;
    }

    public void Load(string name)
    {
        if (Application.loadedLevelName != name) {
            Application.LoadLevel(name);
        }
    }

    // "Login" button click delegate
    public void Login()
    {
        if (Application.loadedLevelName != PlayScene) {
            StartCoroutine(InnerLoad(PlayScene));
        }

        NetworkManager.Instance.JoinRoom(NetworkManager.RoomName);
    }

    // Quit function will be ignored in Editor or web application
    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator InnerLoad(string name)
    {
        Application.LoadLevel(LoadScene);
        // Wait for one frame
        yield return null;
        //yield return new WaitForSeconds(3f);

        Application.LoadLevel(name);
    }
}
