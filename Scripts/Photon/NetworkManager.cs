using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
    private static NetworkManager instance;
    public static NetworkManager Instance
    {
        get {
            if (instance == null) {
                Debug.LogError("Fail to find networkManager instance.");
            }

            return instance;
        }
    }

    private bool isJoinedRoom;
    private bool playerIniated;

    public bool PlayerIniated
    {
        get {
            return playerIniated;
        }
    }

    private Vector3 bornLocation = new Vector3(718.4f, 1f, 662.1f);
    private Quaternion bornRotate = Quaternion.Euler(0, -176.54f, 0);
    public static string RoomName = "master";

    void Awake()
    {
        if (instance != null) {
            Debug.LogError("Only one instance of networkManager is allowwed.");
            return;
        }

        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        isJoinedRoom = false;
        playerIniated = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!playerIniated && isJoinedRoom && Application.loadedLevelName == GameSceneManager.PlayScene) {
            GameObject player = PhotonNetwork.Instantiate("Player", bornLocation, bornRotate, 0);
            player.GetComponent<CatControllerLogic>().enabled = true;
            player.GetComponent<CatThirdPersonCamera>().enabled = true;
            
            playerIniated = true;
            Messenger<GameObject>.Invoke("PlayerBorn", player);
        }
    }

    public void JoinRoom(string name)
    {
        PhotonNetwork.JoinRoom(name);
    }

    public bool ConnectUsingSettings(string version)
    {
        return PhotonNetwork.ConnectUsingSettings(version);
    }

    #region Server push event

    public void OnFailedToConnectToPhoton()
    {
        
    }

    public void OnPhotonJoinRoomFailed()
    {
        Debug.Log("Join master fail, try to creat one.");
        PhotonNetwork.CreateRoom(RoomName);
    }
    
    public void OnJoinedRoom()
    {
        Debug.Log("Joined room master!");
        isJoinedRoom = true;
    }

    #endregion
}
