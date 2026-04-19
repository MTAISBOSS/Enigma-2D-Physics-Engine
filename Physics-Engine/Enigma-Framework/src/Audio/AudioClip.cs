namespace Enigma_Framework.Audio;

public class AudioClip
{
    public readonly string Name;
    public readonly string Path;

    public AudioClip(string name, string path)
    {
        Name = name;
        Path = path;
    }
}