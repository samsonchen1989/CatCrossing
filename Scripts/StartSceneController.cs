using UnityEngine;
using System.Collections;

public class StartSceneController : MonoBehaviour
{
    const int RotateYSpeed = 5;

    float yRotate;

    // Use this for initialization
    void Start()
    {
        yRotate = this.transform.rotation.eulerAngles.y;
    }
    
    // Update is called once per frame
    void Update()
    {
        // Rotate around Y axis when drag with left mouse button
        if (Input.GetMouseButton(0)) {
            yRotate = (yRotate + Input.GetAxis("Mouse X") * RotateYSpeed) % 360;
            this.gameObject.transform.rotation = Quaternion.Euler(0, yRotate, 0);
        }
    }
}
