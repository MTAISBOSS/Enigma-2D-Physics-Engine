namespace Enigma_Framework.Core.AssetPipeline;

public static class AssetImporter
{
    public static void ImportAsset(string path, string guid)
    {
        var ext = Path.GetExtension(path);

        IAssetImporter importer = ext switch
        {
            ".png" => new TextureImporter(),
            ".jpg" => new TextureImporter(),
            ".scene" => new SceneImporter(),
            _ => null
        };
        importer?.Import(path,guid);
    }
}