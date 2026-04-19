using Enigma_Framework.Core.LogSystem;
using OpenTK.Audio.OpenAL;

namespace Enigma_Framework.Audio;

public static class AudioManager
{
    private static ALContext _context;
    private static ALDevice _device;



    private static readonly Dictionary<string, int> AudioBuffers = new();
    private static readonly Dictionary<string, int> AudioSources = new();

    public static void Initialize()
    {
        _device = ALC.OpenDevice(null);
        _context = ALC.CreateContext(_device, (int[])null!);

        ALC.MakeContextCurrent(_context);

        Logger.Log("Audio Manager Initialized using OpenAL.");
    }


    public static void LoadSound(AudioClip clip)
    {
        if (AudioBuffers.ContainsKey(clip.Name)) return;

        int buffer = AL.GenBuffer();

        var soundData = LoadWave(File.OpenRead(clip.Path),
            out var channels,
            out var bitsPerSample,
            out var sampleRate);

        var format = GetSoundFormat(channels, bitsPerSample);

        unsafe
        {
            fixed (byte* ptr = soundData)
            {
                AL.BufferData(buffer, format, (IntPtr)ptr, soundData.Length, sampleRate);
            }
        }

        AudioBuffers[clip.Name] = buffer;
    }


    public static void CreateSource(AudioSource audioSource)
    {
        if (AudioSources.ContainsKey(audioSource.Name)) return;
        var source = AL.GenSource();
        AudioSources[audioSource.Name] = source;
    }

    public static void Play(AudioSource audioSource)
    {
        if (audioSource.AudioClip == null)
        {
            return;
        }
        if (!AudioSources.ContainsKey(audioSource.Name) || !AudioBuffers.ContainsKey(audioSource.AudioClip.Name)) return;

        var source = AudioSources[audioSource.Name];
        var buffer = AudioBuffers[audioSource.AudioClip.Name];

        AL.Source(source, ALSourcei.Buffer, buffer);
        AL.Source(source, ALSourceb.Looping, audioSource.Loop);
        AL.SourcePlay(source);
    }

    public static void Cleanup()
    {
        foreach (var source in AudioSources.Values)
            AL.DeleteSource(source);

        foreach (var buffer in AudioBuffers.Values)
            AL.DeleteBuffer(buffer);

        ALC.MakeContextCurrent(ALContext.Null);
        ALC.DestroyContext(_context);
        ALC.CloseDevice(_device);
    }


    private static ALFormat GetSoundFormat(int channels, int bits)
    {
        if (channels == 1) return bits == 8 ? ALFormat.Mono8 : ALFormat.Mono16;
        if (channels == 2) return bits == 8 ? ALFormat.Stereo8 : ALFormat.Stereo16;
        throw new NotSupportedException("The specified sound format is not supported.");
    }

    private static byte[] LoadWave(Stream stream, out int channels, out int bits, out int rate)
    {
        using var reader = new BinaryReader(stream);
        var signature = new string(reader.ReadChars(4));
        if (signature != "RIFF") throw new NotSupportedException("Not a RIFF file");

        reader.ReadInt32();

        var format = new string(reader.ReadChars(4));
        if (format != "WAVE") throw new NotSupportedException("Not a WAVE file");

        int? foundChannels = null;
        int? foundBits = null;
        int? foundRate = null;
        byte[] audioData = null;

        while (reader.BaseStream.Position < reader.BaseStream.Length)
        {
            var identifier = new string(reader.ReadChars(4));
            var chunkSize = reader.ReadInt32();

            if (identifier == "fmt ")
            {
                reader.ReadInt16();
                foundChannels = reader.ReadInt16();
                foundRate = reader.ReadInt32();
                reader.ReadInt32();
                reader.ReadInt16();
                foundBits = reader.ReadInt16();

                if (chunkSize > 16) reader.ReadBytes(chunkSize - 16);
            }
            else if (identifier == "data")
            {
                audioData = reader.ReadBytes(chunkSize);
            }
            else
            {
                reader.BaseStream.Seek(chunkSize, SeekOrigin.Current);
            }
        }

        if (!foundChannels.HasValue || audioData == null)
            throw new NotSupportedException("Wave file missing fmt or data chunks.");

        channels = foundChannels.Value;
        bits = foundBits.Value;
        rate = foundRate.Value;

        return audioData;
    }
}