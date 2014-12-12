using UnityEngine;
using System.Collections;

public class FloatingText : MonoBehaviour
{
    public GameObject Target { get; set; }

    public Camera WorldCamera { get; set; }
    private Camera nguiCamera;

    public  UILabel label;

    public Vector3 offset = new Vector3(0f, 0.72f, 0f);

    public string Text
    {
        get {
            return label.text;
        }

        set {
            label.text = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        //label = GetComponent<UILabel>();
        if (label == null) {
            Debug.LogError("Fail to find ui label.");
        }

        nguiCamera = NGUITools.FindCameraForLayer(gameObject.layer);
    }
    
    // Update is called once per frame
    void LateUpdate()
    {
        if (Target == null) {
            return;
        }

        Vector3 pos = WorldCamera.WorldToViewportPoint(Target.transform.position + offset);
        if (CheckInCamera(pos)) {
            label.enabled = true;
            pos = nguiCamera.ViewportToWorldPoint(pos);
            pos.z = 0f;
            transform.position = pos;
        } else {
            label.enabled = false;
        }

    }

    private bool CheckInCamera(Vector3 viewport)
    {
        //Debug.Log("name label viewport z:" + viewport.z);
        if (viewport.x >= 0 && viewport.x <= 1 && viewport.y >= 0 && viewport.y <=1 && viewport.z > 0) {
            return true;
        }

        return false;
    }
}
