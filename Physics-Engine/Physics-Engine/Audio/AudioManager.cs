using System;
using System.Collections.Generic;
using System.IO;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using Physics_Engine.Core.Log_System;

namespace Physics_Engine.Audio;

public static class AudioManager
{
    private static AudioContext _context;

    private static readonly Dictionary<string, int> AudioBuffers = new Dictionary<string, int>();
    private static readonly Dictionary<string, int> AudioSources = new Dictionary<string, int>();

    public static void Initialize()
    {
        _context = new AudioContext();
        Logger.Log("Audio Manager Initialized using OpenTK.Audio.");
    }

    public static void LoadSound(string name, string filepath)
    {
        if (AudioBuffers.ContainsKey(name)) return;

        int buffer = AL.GenBuffer();

        byte[] soundData = LoadWave(File.OpenRead(filepath), out var channels, out var bitsPerSample,
            out var sampleRate);

        ALFormat format = GetSoundFormat(channels, bitsPerSample);

        AL.BufferData(buffer, format, soundData, soundData.Length, sampleRate);

        AudioBuffers[name] = buffer;
    }

    public static void CreateSource(string sourceName)
    {
        if (AudioSources.ContainsKey(sourceName)) return;
        int source = AL.GenSource();
        AudioSources[sourceName] = source;
    }

    public static void Play(string sourceName, string soundName, bool loop = false)
    {
        if (!AudioSources.ContainsKey(sourceName) || !AudioBuffers.ContainsKey(soundName)) return;

        int source = AudioSources[sourceName];
        int buffer = AudioBuffers[soundName];

        AL.Source(source, ALSourcei.Buffer, buffer);
        AL.Source(source, ALSourceb.Looping, loop);
        AL.SourcePlay(source);
    }

    public static void Cleanup()
    {
        foreach (var source in AudioSources.Values) AL.DeleteSource(source);
        foreach (var buffer in AudioBuffers.Values) AL.DeleteBuffer(buffer);

        _context?.Dispose();
    }

    private static ALFormat GetSoundFormat(int channels, int bits)
    {
        if (channels == 1) return bits == 8 ? ALFormat.Mono8 : ALFormat.Mono16;
        if (channels == 2) return bits == 8 ? ALFormat.Stereo8 : ALFormat.Stereo16;
        throw new NotSupportedException("The specified sound format is not supported.");
    }

    private static byte[] LoadWave(Stream stream, out int channels, out int bits, out int rate)
    {
        using BinaryReader reader = new BinaryReader(stream);
        string signature = new string(reader.ReadChars(4));
        if (signature != "RIFF") throw new NotSupportedException("Not a RIFF file");

        reader.ReadInt32();

        string format = new string(reader.ReadChars(4));
        if (format != "WAVE") throw new NotSupportedException("Not a WAVE file");

        int? foundChannels = null;
        int? foundBits = null;
        int? foundRate = null;
        byte[] audioData = null;

        while (reader.BaseStream.Position < reader.BaseStream.Length)
        {
            string identifier = new string(reader.ReadChars(4));
            int chunkSize = reader.ReadInt32();

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