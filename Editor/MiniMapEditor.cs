using UnityEngine;
using UnityEditor;

using System.Collections;

public class MiniMapEditor : MonoBehaviour {
    [MenuItem("MiniMapUtils/Create Map Planes From Selection")]
    private static void CreateMapPlanesFromSelection()
    {
        var mSettings = new MapSettings("setting.txt");

        var length = mSettings.Length;
        var width = mSettings.Width;

        var mesh = CreatePlaneMesh(length, width);

        FolderUtils.CreateAssetFolder("Minimap/Mesh");
        AssetDatabase.CreateAsset(mesh, "Assets/Minimap/Mesh/plane.mesh");

        var selection = Selection.GetFiltered(typeof(Texture), SelectionMode.DeepAssets);
        if (selection != null) {
            for (int i = 0; i < selection.Length; i++) {
                var texture = selection[i] as Texture;
                if (texture != null) {
                    var mat = CreateMaterial(texture, texture.name);
                    // Create material files
                    FolderUtils.CreateAssetFolder("Minimap/Prefabs");
                    FolderUtils.CreateAssetFolder("Minimap/Prefabs/Materials");
                    AssetDatabase.CreateAsset(mat, string.Format("Assets/Minimap/Prefabs/Materials/{0}.mat", texture.name));
                    // Create prefab
                    var prefab = PrefabUtility.CreatePrefab(string.Format("Assets/Minimap/Prefabs/{0}.prefab", texture.name),
                                                            CreatePrefab(texture.name));
                    prefab.renderer.material = mat;
                    prefab.GetComponent<MeshFilter>().sharedMesh = mesh;
                    prefab.GetComponent<MeshRenderer>().castShadows = false;
                    prefab.GetComponent<MeshRenderer>().receiveShadows = false;
                }
            }

            AssetDatabase.Refresh();
        }
    }

    [MenuItem("MiniMapUtils/Export Selection to Map Asset")]
    private static void ExportSelectionToMapAsset()
    {
        var selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        if (selection != null) {
            var path = EditorUtility.SaveFilePanel("Save Asset", "", "New Data", "dat");
            if (false == string.IsNullOrEmpty(path)) {
                var settingAsset = (TextAsset) AssetDatabase.LoadAssetAtPath("Assets/Minimap/setting.txt", typeof(TextAsset));
                if (settingAsset == null) {
                    Debug.LogError("Setting file not found under minimap folder.");
                    return;
                }

                BuildPipeline.BuildAssetBundle(settingAsset, selection, path, BuildAssetBundleOptions.CompleteAssets |
                                                                              BuildAssetBundleOptions.UncompressedAssetBundle |
                                                                              BuildAssetBundleOptions.CollectDependencies);
                Selection.objects = selection;
            }
        }
    }

    private static Mesh CreatePlaneMesh(int length, int width)
    {
        var mesh = new Mesh();
        mesh.name = "low_poly_mesh";

        // Draw clock-wise, 6 points and two triangles, face up
        mesh.vertices = new Vector3[6] { new Vector3(0, 0, 0), new Vector3(0, width, 0), new Vector3(length, width, 0),
                                         new Vector3(length, width, 0), new Vector3(length, 0, 0), new Vector3(0, 0, 0) };
        mesh.uv = new Vector2[6] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1),
                                   new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 0) };
        mesh.triangles = new int[6] { 0, 1, 2, 3, 4, 5 };

        return mesh;
    }

    private static Material CreateMaterial(Texture texture, string name)
    {
        var material = new Material(Shader.Find("Unlit/Texture"));
        material.mainTexture = texture;
        material.name = name;

        return material;
    }

    private static GameObject CreatePrefab(string name)
    {
        var gameObject = new GameObject(name); 

        gameObject.AddComponent("MeshFilter");
        gameObject.AddComponent("MeshRenderer");
        // Face upwards
        gameObject.transform.Rotate(new Vector3(1, 0, 0), 90);

        return gameObject;
    }
}
