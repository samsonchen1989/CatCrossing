using UnityEngine;
using System.Collections;
using System.IO;

public class AutoScreenShot : MonoBehaviour
{
    public float snapDelay = 1.0f;
    // Camera aspect
    public float aspect = 1.0f;
    // Texture quality
    public int superSize = 2;
    // Texture name
    public string segmentName = "segment";

    private Transform camTransform;

    // Use this for initialization
    IEnumerator Start()
    {
        camTransform = this.transform;

        if (snapDelay < 0.1f) {
            this.snapDelay = 0.1f;
        }

        // Disable other camera before take snapshot
        camera.enabled = true;
        camera.aspect = aspect;

        float xHalfUnit = camera.orthographicSize * aspect;
        float zHalfUnit = camera.orthographicSize;

        float xIncrement = xHalfUnit * 2;
        float zIncrement = zHalfUnit * 2;

        float xTerrainMax = Terrain.activeTerrain.terrainData.size.x;
        float zTerrainMax = Terrain.activeTerrain.terrainData.size.z;

        FolderUtils.CreateAssetFolder("Minimap/Textures");

        for (float x = 0; x < xTerrainMax + xHalfUnit; x += xIncrement) {
            for (float z = 0; z < zTerrainMax + zHalfUnit; z += zIncrement) {
                moveCam(x, z);

                // Take screenshot
                Application.CaptureScreenshot(string.Format("Assets/Minimap/Textures/{0}-{1}.{2}.png", x, z, segmentName));

                //wait a couple seconds
                yield return new WaitForSeconds(snapDelay);
            }
        }

        // Add other useful info to a file
        using (StreamWriter writer = new StreamWriter("Assets/Minimap/setting.txt"))
        {
            writer.WriteLine(string.Format("name=\"{0}\"", segmentName));
            writer.WriteLine(string.Format("length={0}", xIncrement));
            writer.WriteLine(string.Format("width={0}", zIncrement));
            writer.WriteLine(string.Format("xMin={0}", 0));
            writer.WriteLine(string.Format("xMax={0}", xTerrainMax));
            writer.WriteLine(string.Format("zMin={0}", 0));
            writer.WriteLine(string.Format("zMax={0}", zTerrainMax));
        }
    }

    private void moveCam(float xHalfUnit, float zHalfUnit)
    {
        camTransform.position = new Vector3(xHalfUnit, camTransform.position.y, zHalfUnit);
    }
    
    // Update is called once per frame
    void Update()
    {
    
    }
}
