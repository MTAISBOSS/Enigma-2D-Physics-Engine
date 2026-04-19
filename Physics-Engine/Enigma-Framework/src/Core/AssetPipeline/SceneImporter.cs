namespace Enigma_Framework.Core.AssetPipeline;

public class SceneImporter : IAssetImporter
{
    public void Import(string sourcePath, string guid)
    {
        var scene = File.ReadAllText(sourcePath);
        var libraryPath = Path.Combine("Library/ImportedAssets", guid + ".scene");
        File.WriteAllText(libraryPath,scene);
    }
}