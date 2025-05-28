using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    private Dictionary<SoundType, AudioClip> soundClips;
    public Queue<AudioSource> audioSourcePool;
    public Dictionary<string, AudioClip> BGMs;
    public List<string> BGMNames;
    private void Awake()
    {
        soundClips = new Dictionary<SoundType, AudioClip>();
        audioSourcePool = new Queue<AudioSource>();

        // Create a pool of AudioSources
        for (int i = 0; i < 20; i++)
        {
            AudioSource audioSource = CreateAudioSource();
            audioSourcePool.Enqueue(audioSource);
        }

        BGMs = new Dictionary<string, AudioClip>();
        BGMNames = new List<string>();

        AudioClip[] bgms = Resources.LoadAll<AudioClip>("Audio/BGMS");
        foreach (var bgm in bgms)
        {
            BGMs.Add(bgm.name, bgm);
            BGMNames.Add(bgm.name);
        }

        InitSoundClips();
    }

    public AudioClip GetAudioClip(SoundType soundType)
    {
        if (soundClips.ContainsKey(soundType)) return soundClips[soundType];
        else return null;
    }

    public AudioSource GetAudioSource()
    {
        if (audioSourcePool.Count > 0) return audioSourcePool.Dequeue();
        else return null;
    }

    public void ReleaseAudioSource(AudioSource audioSource)
    {
        audioSourcePool.Enqueue(audioSource);
    }

    public void AddSoundClip(SoundType soundType, AudioClip audioClip)
    {
        if (!soundClips.ContainsKey(soundType)) soundClips.Add(soundType, audioClip);
    }

    public AudioClip GetBGM(string BGMName)
    {
        if (BGMs.ContainsKey(BGMName)) return BGMs[BGMName];
        else return null;
    }


    public AudioSource CreateAudioSource()
    {
        GameObject audioSourceObject = new GameObject("AudioSource");
        audioSourceObject.transform.SetParent(this.transform);
        AudioSource audioSource = audioSourceObject.AddComponent<AudioSource>();
        return audioSource;
    }

    void OnDisable()
    {
        // Clean up the audio source pool when the AudioManager is disabled
        foreach (var audioSource in audioSourcePool)
        {
            Destroy(audioSource.gameObject);
        }
        audioSourcePool.Clear();
        soundClips.Clear();
        BGMs.Clear();
        BGMNames.Clear();
    }

    private void InitSoundClips()
    {
        AudioClip dashClip = Resources.Load<AudioClip>("Audio/PlayerSoundEffect/Dash");
        AddSoundClip(SoundType.Dashing, dashClip);

        AudioClip attackClip1 = Resources.Load<AudioClip>("Audio/PlayerSoundEffect/Attack1");
        AddSoundClip(SoundType.Attacking1, attackClip1);

        AudioClip attackClip2 = Resources.Load<AudioClip>("Audio/PlayerSoundEffect/Attack2");
        AddSoundClip(SoundType.Attacking2, attackClip2);

        AudioClip bouncePadClip = Resources.Load<AudioClip>("Audio/MapSoundEffect/BouncePad");
        AddSoundClip(SoundType.BouncePad, bouncePadClip);

        AudioClip bouncePlatformClip = Resources.Load<AudioClip>("Audio/MapSoundEffect/BouncePlatform");
        AddSoundClip(SoundType.BouncePlatform, bouncePlatformClip);

        AudioClip gemCollectClip = Resources.Load<AudioClip>("Audio/MapSoundEffect/GemCollect");
        AddSoundClip(SoundType.GemCollect, gemCollectClip);

        AudioClip breakClip = Resources.Load<AudioClip>("Audio/MapSoundEffect/Break");
        AddSoundClip(SoundType.Break, breakClip);
    }
}