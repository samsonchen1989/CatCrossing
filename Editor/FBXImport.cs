using UnityEngine;
using UnityEditor;
using System.Collections;

public class FBXImport : AssetPostprocessor
{
    // Default is 0.01f, can change to 1f
    const float ImportScale = 1.0f;

    void OnPreprocessModel()
    {
        ModelImporter importer = assetImporter as ModelImporter;
        importer.globalScale = ImportScale;
    }
}
