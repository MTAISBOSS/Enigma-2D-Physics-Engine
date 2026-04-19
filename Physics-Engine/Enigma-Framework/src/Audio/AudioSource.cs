namespace Enigma_Framework.Audio;
public class AudioSource
{
    public string Name;
    public bool Loop;
    public bool PlayOnStart;
    public AudioClip AudioClip;

    public AudioSource(string name)
    {
        Name = name;
    }
}