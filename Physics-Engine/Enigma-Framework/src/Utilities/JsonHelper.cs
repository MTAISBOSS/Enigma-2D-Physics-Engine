using System.Text.Json;

namespace Enigma_Framework.Utilities;

public static class JsonHelper
{
    public static JsonElement ToJsonElement(object obj)
    {
        return JsonSerializer.SerializeToElement(obj);
    }

}