using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    Running,
    WallJumping,
    Attacking,
    Dashing,
    BulletTime,
    Rolling,
    Dead
}

public class AudioManager
{
    private Dictionary<SoundType, AudioClip> soundClips;
    public Queue<AudioSource> audioSourcePool;
    public Dictionary<String, AudioClip> BGMs;
    public AudioManager()
    {
        soundClips = new Dictionary<SoundType, AudioClip>();
        audioSourcePool = new Queue<AudioSource>(20);

        //Resources.LoadAll<AudioClip>("Sounds");
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

    public AudioClip GetBGM(String BGMName)
    {
        if (BGMs.ContainsKey(BGMName)) return BGMs[BGMName];
        else return null;
    }
}

public class SceneManager : MonoBehaviour, IEffectController, ISoundEffectController
{
    private AudioManager audioManager;
    private AudioSource BGMAudioSource;
    public GameObject playerDashDust;
    private string currentBGM;
    private float BGMVolume;
    private float SoundEffectVolume;

    private void Awake()
    {
        audioManager = new AudioManager();
        BGMAudioSource = new AudioSource();
        currentBGM = "";   //TODO: 播放BGM
        // BGMAudioSource.clip = audioManager.GetBGM(currentBGM);
        // BGMAudioSource.Play();
    }
    private void Start()
    {
    }

    public void CameraShake(Vector2 direction)
    {
        Game.instance.cameraManager.Shake(direction, 0.2f);
    }

    public void Freeze(float duration)
    {
        StartCoroutine(Game.instance.Freeze(duration));
    }

    public AudioClip GetSoundClip(SoundType soundType)
    {
        return audioManager.GetAudioClip(soundType);
    }

    public AudioSource GetAudioSource()
    {
        AudioSource audioSource = audioManager.GetAudioSource();
        audioSource.volume = SoundEffectVolume;
        return audioSource;
    }

    public void ReleaseAudioSource(AudioSource audioSource)
    {
        audioManager.ReleaseAudioSource(audioSource);
    }

    public void ChangerBGM(string BGMName)
    {
        if (currentBGM != BGMName)
        {
            if (BGMAudioSource.isPlaying) BGMAudioSource.Stop();
            currentBGM = BGMName;
            BGMAudioSource.clip = audioManager.GetBGM(currentBGM);
            BGMAudioSource.Play();
        }
    }

    public void SetSoundEffectVolume(float volume)
    {
        SoundEffectVolume = volume;
    }

    public void SetBGMVolume(float volume)
    {
        BGMVolume = volume;
    }

    public void StopBgm()
    {
        BGMAudioSource.Stop();
    }

    public GameObject PlayerDashFX(Vector3 position)
    {
        GameObject fx = Instantiate(playerDashDust, position, Quaternion.identity);
        return fx;
    }
}


