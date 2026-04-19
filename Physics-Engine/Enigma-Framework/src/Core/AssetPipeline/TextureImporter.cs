namespace Enigma_Framework.Core.AssetPipeline;

public class TextureImporter : IAssetImporter
{
    public void Import(string sourcePath, string guid)
    {
        var image = File.ReadAllBytes(sourcePath);
        var libraryPath = Path.Combine("Library/ImportedAssets", guid + ".texture");
        File.WriteAllBytes(libraryPath,image);
    }
}