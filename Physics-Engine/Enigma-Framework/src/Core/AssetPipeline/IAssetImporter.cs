namespace Enigma_Framework.Core.AssetPipeline;

public interface IAssetImporter
{
    void Import(string sourcePath, string guid);
}