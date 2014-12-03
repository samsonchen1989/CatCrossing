using UnityEngine;
using System.Collections;

public enum SegmentState : byte
{
    Destroyed = 0,
    Loading = 1,
    Active = 2
}

public class MapSegment
{
    // Plane prefab created from MiniMapEditor
    public GameObject gameObject { get; set; }
    public SegmentState state { get; set; }

    public void Destroy(bool resetState)
    {
        if (gameObject != null) {
            GameObject.Destroy(gameObject);
            if (resetState) {
                state = SegmentState.Destroyed;
            }
        }
    }

    public void Reset()
    {
        this.gameObject = null;
        this.state = SegmentState.Destroyed;
    }

    public void Replace(MapSegment segment)
    {
        if (gameObject != null) {
            GameObject.Destroy(gameObject);
        }

        this.gameObject = segment.gameObject;
        this.state = segment.state;
    }
}
