using System.Text.Json;

namespace Enigma_Framework.Core.AssetPipeline;

public static class MetaFile
{
    public static void Create(string path)
    {
        var meta = new MetaData()
        {
            Guid = Guid.NewGuid().ToString()
        };
        var json = JsonSerializer.Serialize(meta, new JsonSerializerOptions()
        {
            WriteIndented = true
        });
        File.WriteAllText(path, json);
    }

    public static MetaData Load(string path)
    {
        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<MetaData>(path);
    }
}