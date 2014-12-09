using UnityEngine;
using System.Collections;

public class MapLoader : MonoBehaviour, IMapLoader
{
    public Transform playerTrans;

    MapHandler mapHandler;

    float mapCheck = 5f;
    float timer = 0;

    #region IMapLoader implementation
    public void StartAsyncMethod(IEnumerator method)
    {
        this.StartCoroutine(method);
    }
    #endregion

    public void Unload()
    {
        this.mapHandler.Unload();
        this.mapHandler = null;
    }

    void OnEnable()
    {
        Messenger<GameObject>.AddListener(MyEventType.PLAYER_BORN, OnPlayerBorn);
    }

    void OnDisable()
    {
        Messenger<GameObject>.RemoveListener(MyEventType.PLAYER_BORN, OnPlayerBorn);
    }

    void OnPlayerBorn(GameObject player)
    {
        playerTrans = player.transform;
        this.MoveCam(playerTrans.position);
        this.mapHandler.Start(playerTrans.position);
    }

    // Use this for initialization
    void Start()
    {
        /* No use if do not have unity pro.
        var bundle = AssetBundle.CreateFromFile(string.Format("{0}/{1}", System.IO.Directory.GetCurrentDirectory(),
                                                "Data/mapData.dat"));
        */
        var mapSettings = new MapSettings("setting");
        this.mapHandler = new MapHandler(this, null, mapSettings, LayerMask.NameToLayer("MiniMap"));
    }
    
    // Update is called once per frame
    void Update()
    {
        if (playerTrans == null) {
            return;
        }

        this.MoveCam(playerTrans.position);

        this.timer += Time.deltaTime;
        if (timer > mapCheck) {
            this.mapHandler.UpdateMap(playerTrans.position);
            this.timer = 0;
        }
    }

    void MoveCam(Vector3 position)
    {
        this.transform.position = new Vector3(position.x, transform.position.y, position.z);
    }
}
