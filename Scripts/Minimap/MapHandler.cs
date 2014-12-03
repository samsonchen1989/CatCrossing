using UnityEngine;
using System.Collections;

public class MapHandler
{
    #region Constants and Fields

    private readonly IMapLoader mapLoader;
    private readonly AssetBundle bundle;
    private readonly MapSettings mapSettings;

    private readonly MapSegment[,] loadedSegments = new MapSegment[2, 2];

    private readonly int mapLayer;

    Vector3 mapOffset;
    Rect mapBounds;

    #endregion

    #region Properties

    MapSegment BottomLeft
    {
        get {
            return this.loadedSegments[0, 0];
        }
    }

    MapSegment BottomRight
    {
        get {
            return this.loadedSegments[1, 0];
        }
    }

    MapSegment TopLeft
    {
        get {
            return this.loadedSegments[0, 1];
        }
    }

    MapSegment TopRight
    {
        get {
            return this.loadedSegments[1, 1];
        }
    }
    
    #endregion

    #region Constructors and Destructor

    public MapHandler(IMapLoader mapLoader, AssetBundle mapAsset, MapSettings mapSettings, int mapLayer)
    {
        this.mapLoader = mapLoader;
        this.bundle = mapAsset;
        this.mapSettings = mapSettings;
        this.mapLayer = mapLayer;

        this.mapOffset = new Vector3(mapSettings.Length / 2, 260, mapSettings.Width / 2);
        this.mapBounds = new Rect();
    }

    #endregion

    #region Public Methods

    public void Start(Vector3 pos)
    {
        this.PrepareMapAt(pos);
    }

    // Make sure call this function periodically
    public void UpdateMap(Vector3 pos)
    {
        this.UpdateMapAt(pos);
    }

    public void Unload()
    {
        this.bundle.Unload(true);
    }

    #endregion

    #region Local Methods

    void PrepareMapAt(Vector3 pos)
    {
        this.mapBounds = this.GetMapBoundsForPos(pos);
        Debug.Log(string.Format("MapBounds, xMin:{0}, xMax:{1}, yMin:{2}, yMax:{3}", mapBounds.xMin, mapBounds.xMax, mapBounds.yMin, mapBounds.yMax));
        // Bottom left
        this.loadedSegments[0, 0] = new MapSegment() { gameObject = LoadAndCreateSegmentAt(mapBounds.xMin, mapBounds.yMin), state = SegmentState.Active};
        // Bottom right
        this.loadedSegments[1, 0] = new MapSegment() { gameObject = LoadAndCreateSegmentAt(mapBounds.xMax, mapBounds.yMin), state = SegmentState.Active};
        // Top left
        this.loadedSegments[0, 1] = new MapSegment() { gameObject = LoadAndCreateSegmentAt(mapBounds.xMin, mapBounds.yMax), state = SegmentState.Active};
        // Top right
        this.loadedSegments[1, 1] = new MapSegment() { gameObject = LoadAndCreateSegmentAt(mapBounds.xMax, mapBounds.yMax), state = SegmentState.Active};
    }

    void UpdateMapAt(Vector3 pos)
    {
        var newMapBounds = this.GetMapBoundsForPos(pos);
        bool changed = false;

        if (newMapBounds.xMin < mapBounds.xMin) {
            this.TopRight.Replace(TopLeft);
            this.BottomRight.Replace(BottomLeft);
            this.TopLeft.Reset();
            this.BottomLeft.Reset();

            changed = true;
        } else if (newMapBounds.xMax > mapBounds.xMax) {
            this.TopLeft.Replace(TopRight);
            this.BottomLeft.Replace(BottomRight);
            this.TopRight.Reset();
            this.BottomRight.Reset();

            changed = true;
        }

        if (newMapBounds.yMin < mapBounds.yMin) {
            this.TopLeft.Replace(BottomLeft);
            this.TopRight.Replace(BottomRight);
            this.BottomLeft.Reset();
            this.BottomRight.Reset();

            changed = true;
        } else if (newMapBounds.yMax > mapBounds.yMax) {
            this.BottomLeft.Replace(TopLeft);
            this.BottomRight.Replace(TopRight);
            this.TopLeft.Reset();
            this.TopRight.Reset();

            changed = true;
        }

        if (changed) {
            this.mapBounds = newMapBounds;
            this.HandleSegmentAt(BottomLeft, mapBounds.xMin, mapBounds.yMin);
            this.HandleSegmentAt(BottomRight, mapBounds.xMax, mapBounds.yMin);
            this.HandleSegmentAt(TopLeft, mapBounds.xMin, mapBounds.yMax);
            this.HandleSegmentAt(TopRight, mapBounds.xMax, mapBounds.yMax);
        }
    }

    void HandleSegmentAt(MapSegment segment, float x, float z)
    {
        if (segment.state == SegmentState.Destroyed) {
            this.mapLoader.StartAsyncMethod(WaitUntilSegmentLoadAt(segment, x, z));
        }
    }

    IEnumerator WaitUntilSegmentLoadAt(MapSegment segment, float x, float z)
    {
        segment.state = SegmentState.Loading;
        var segCoorPos = GetSegmentCoordForPos(x, z);
        var go = this.LoadSegmentAsyncAt((int)segCoorPos.x, (int)segCoorPos.y);

        segment.gameObject = CreateSegmentAt(segCoorPos, go);
        segment.state = SegmentState.Active;

        yield return null;
    }
    
    Rect GetMapBoundsForPos(Vector3 pos)
    {
        return GetMapBoundsForPos(pos.x, pos.z);
    }

    Rect GetMapBoundsForPos(float x, float z)
    {
        var currSegCoord = this.GetSegmentCoordForPos(x, z);
        Debug.Log(string.Format("currSegCoord:{0}, {1}.", currSegCoord.x, currSegCoord.y));

        Rect bounds = new Rect(currSegCoord.x, currSegCoord.y, mapSettings.Length, mapSettings.Width);
        if (bounds.xMax > mapSettings.XMax) {
            var val = bounds.xMax - mapSettings.XMax;
            bounds.xMin = this.GetSegmentCoordForPos(bounds.xMin - val, z).x;
            bounds.xMax = this.GetSegmentCoordForPos(bounds.xMax - val, z).x;
        } else if (x < currSegCoord.x) {
            bounds.xMin -= mapSettings.Length;
            bounds.xMax -= mapSettings.Length;
        }

        if (bounds.yMax > mapSettings.ZMax) {
            var val = bounds.yMax - mapSettings.ZMax;

            bounds.yMin = this.GetSegmentCoordForPos(x, bounds.yMin - val).y;
            bounds.yMax = this.GetSegmentCoordForPos(x, bounds.yMax - val).y;
        } else if (z < currSegCoord.y) {
            bounds.yMin -= mapSettings.Width;
            bounds.yMax -= mapSettings.Width;
        }

        return bounds;
    }

    Vector2 GetSegmentCoordForPos(Vector2 pos)
    {
        return this.GetSegmentCoordForPos(pos.x, pos.y);
    }

    Vector2 GetSegmentCoordForPos(float x, float z)
    {
        Debug.Log("x:" + x.ToString() + ", z:" + z.ToString());
        Debug.Log("map offset, x:" + mapOffset.x.ToString() + ", mapoffset z:" + mapOffset.z.ToString());
        Debug.Log("mapSettings, xMin:" + mapSettings.XMin.ToString() + ", xMax:" + mapSettings.XMax.ToString());
        var nX = Mathf.Clamp(x + mapOffset.x, mapSettings.XMin, mapSettings.XMax);
        var nZ = Mathf.Clamp(z + mapOffset.z, mapSettings.ZMin, mapSettings.ZMax);
        Debug.Log("nX:" + nX.ToString() + ", nZ:" + nZ.ToString());

        var pX = (int)(nX / mapSettings.Length);
        var pZ = (int)(nZ / mapSettings.Width);
        Debug.Log("pX:" + pX.ToString() + ", pZ:" + pZ.ToString());

        var aX = pX * mapSettings.Length;
        var aZ = pZ * mapSettings.Width;
        Debug.Log("aX:" + aX.ToString() + ", aZ:" + aZ.ToString());

        return new Vector2(aX, aZ);
    }

    GameObject CreateSegmentAt(Vector2 coord, GameObject segment)
    {
        var go = MonoBehaviour.Instantiate(segment) as GameObject;
        go.transform.position = new Vector3(coord.x - mapOffset.x, mapOffset.y, coord.y - mapOffset.z);
        go.layer = mapLayer;

        return go;
    }

    GameObject LoadAndCreateSegmentAt(float x, float z)
    {
        var segCoord = this.GetSegmentCoordForPos(x, z);
        var segment = this.LoadSegmentAt((int)segCoord.x, (int)segCoord.y);

        var go =  GameObject.Instantiate(segment) as GameObject;
        go.transform.position = new Vector3(x - mapOffset.x, mapOffset.y, z - mapOffset.z);
        go.layer = mapLayer;

        return go;
    }

    //AssetBundleRequest LoadSegmentAsyncAt(int x, int z)
    GameObject LoadSegmentAsyncAt(int x, int z)
    {
        Debug.Log("x:" + x + ", z:" + z);
        return Resources.Load(string.Format("Minimap/{0}-{1}.{2}", x, z, mapSettings.SegmentName), typeof(GameObject)) as GameObject;
        //return this.bundle.LoadAsync(string.Format("{0}-{1}.{2}", x, z, mapSettings.SegmentName), typeof(GameObject));
    }

    GameObject LoadSegmentAt(int x, int z)
    {
        return Resources.Load(string.Format("Minimap/{0}-{1}.{2}", x, z, mapSettings.SegmentName), typeof(GameObject)) as GameObject;
        //return bundle.Load(string.Format("{0}-{1}.{2}", x, z, mapSettings.SegmentName), typeof(GameObject)) as GameObject;
    }

    #endregion
}
