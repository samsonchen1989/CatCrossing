using UnityEngine;
using System.Collections;

public class MapLoader : MonoBehaviour, IMapLoader
{
    public Transform player;

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

    // Use this for initialization
    void Start()
    {
        var bundle = AssetBundle.CreateFromFile(string.Format("{0}/{1}", System.IO.Directory.GetCurrentDirectory(), "Data/mapData.dat"));
        var mapSettings = new MapSettings("setting.txt");
        this.mapHandler = new MapHandler(this, null, mapSettings, LayerMask.NameToLayer("MiniMap"));
    
        this.MoveCam(player.position);
        this.mapHandler.Start(player.position);
    }
    
    // Update is called once per frame
    void Update()
    {
        this.MoveCam(player.position);

        this.timer += Time.deltaTime;
        if (timer > mapCheck) {
            this.mapHandler.UpdateMap(player.position);
            this.timer = 0;
        }
    }

    void MoveCam(Vector3 position)
    {
        this.transform.position = new Vector3(position.x, transform.position.y, position.z);
    }
}
