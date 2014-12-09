using System.IO;

using UnityEngine;

// Minimap setting file reader
public class MapSettings
{
    private int length, width;
    private int xMin, xMax, zMin, zMax;

    public string SegmentName {get; private set;}

    public int Length { get {return length;} }
    public int Width { get {return width;} }

    public int XMin { get {return xMin;} }
    public int XMax { get {return xMax;} }
    public int ZMin { get {return zMin;} }
    public int ZMax { get {return zMax;} }

    public MapSettings(string fileName)
    {
        TextAsset textAsset = Resources.Load(string.Format("Minimap/{0}", fileName)) as TextAsset;
        if (textAsset == null) {
            throw new System.IO.FileNotFoundException("Minimap setting file not found.");
        }

        using(var reader = new StringReader(textAsset.text))
        {
            do
            {
                var line = reader.ReadLine();
                line = line.Replace(" ", string.Empty).ToLower();

                var split = line.Split('=');
                if (split.Length < 1) {
                    throw new System.IO.FileLoadException("Minimap setting data is corrupted.");
                }

                var first = split[0];
                var second = split[1];

                switch(first)
                {
                    case "name":
                        if (second[0] == '\"') {
                            this.SegmentName = second.Substring(1, second.Length - 2);
                            if (string.IsNullOrEmpty(SegmentName)) {
                                throw new System.IO.FileLoadException("Minimap setting data is corrupted.");
                            }
                        }
                        break;
                    case "length":
                        if (int.TryParse(second, out this.length) == false) {
                            throw new System.IO.FileLoadException("Minimap setting data is corrupted.");
                        }
                        break;
                    case "width":
                        if (int.TryParse(second, out this.width) == false) {
                            throw new System.IO.FileLoadException("Minimap setting data is corrupted.");
                        }
                        break;
                    case "xmin":
                        if (int.TryParse(second, out this.xMin) == false) {
                            throw new System.IO.FileLoadException("Minimap setting data is corrupted.");
                        }
                        break;
                    case "xmax":
                        if (int.TryParse(second, out this.xMax) == false) {
                            throw new System.IO.FileLoadException("Minimap setting data is corrupted.");
                        }
                        break;
                    case "zmin":
                        if (int.TryParse(second, out this.zMin) == false) {
                            throw new System.IO.FileLoadException("Minimap setting data is corrupted.");
                        }
                        break;
                    case "zmax":
                        if (int.TryParse(second, out this.zMax) == false) {
                            throw new System.IO.FileLoadException("Minimap setting data is corrupted.");
                        }
                        break;
                    default:
                        break;
                }
            } while(reader.Peek() != -1);
        }
    }
}
