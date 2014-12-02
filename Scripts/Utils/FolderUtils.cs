using UnityEngine;
using System.IO;

public static class FolderUtils
{
    //If folder path does not exist, create the folder
    public static void CreateAssetFolder(string path)
    {
        string fullPath = string.Format("{0}/{1}", Application.dataPath, path);
        if (!Directory.Exists(fullPath)) {
            Directory.CreateDirectory(fullPath);
        }
    }
}
