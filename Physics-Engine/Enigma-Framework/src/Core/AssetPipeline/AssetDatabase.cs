namespace Enigma_Framework.Core.AssetPipeline;

public static class AssetDatabase
{
    private static Dictionary<string, AssetInfo> assetsByGuid = new();
    private static Dictionary<string, string> guidByPath = new();
    public static IReadOnlyDictionary<string, AssetInfo> Assets => assetsByGuid;

    public static AssetInfo GetAsset(string guid)
    {
        return assetsByGuid[guid];
    }

    public static string GetGuid(string path)
    {
        return guidByPath[path];
    }

    public static void RegisterAsset(AssetInfo asset)
    {
        assetsByGuid[asset.Guid] = asset;
        guidByPath[asset.Path] = asset.Guid;
    }

    public static void ScanAssets(string root)
    {
        var files = Directory.GetFiles(root, "*.*", SearchOption.AllDirectories);
        foreach (var file in files)
        {
            if (file.EndsWith(".meta"))
            {
                continue;
            }

            var metaPath = file + ".meta";
            if (!File.Exists(metaPath))
            {
                MetaFile.Create(metaPath);
            }

            var meta = MetaFile.Load(metaPath);
            
            AssetDatabase.RegisterAsset(new AssetInfo()
            {
                Guid = meta.Guid,
                Path = file,
                AssetType = DetectType(file)
            });
        }
    }
    public static string DetectType(string file)
    {
        var ext = Path.GetExtension(file).ToLower();

        return ext switch
        {
            ".png" => "Texture",
            ".jpg" => "Texture",
            ".jpeg" => "Texture",

            ".scene" => "Scene",

            ".obj" => "Model",
            ".fbx" => "Model",

            ".cs" => "Script",

            ".wav" => "Audio",
            ".mp3" => "Audio",

            _ => "Unknown"
        };
    }

}