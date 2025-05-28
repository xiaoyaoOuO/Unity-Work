using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    Running,
    WallJumping,
    Attacking1,
    Attacking2,
    AttackSuccess,
    Dashing,
    BulletTime,
    Rolling,
    Dead,
    BouncePad,
    BouncePlatform,
    GemCollect,
    Break,
}


public class SceneManager : MonoBehaviour, IEffectController, ISoundEffectController
{
    public AudioManager audioManager;
    private AudioSource BGMAudioSource;
    public GameObject playerDashDust;
    public GameObject playerJumpFX;
    public GameObject playerLandFX;
    public GameObject playerWallSlideFX;
    public GameObject playerWallJumpFX;
    public GameObject playerDashTrailFX;
    private string currentBGM;
    private float BGMVolume { set { BGMAudioSource.volume = value; } get { return BGMAudioSource.volume; } }
    private float SoundEffectVolume;

    private void Awake()
    {
        SoundEffectVolume = 1.0f; // Default sound effect volume
    }
    private void Start()
    {
        audioManager = GetComponentInChildren<AudioManager>();
        BGMAudioSource = audioManager.CreateAudioSource();
        Debug.Log("AudioManager initialized with " + audioManager.BGMNames.Count + " BGM tracks.");
        currentBGM = audioManager.BGMNames[0];   //TODO: 播放BGM
        // Debug.Log("Current BGM: " + currentBGM);
        // Debug.Log(BGMAudioSource);
        BGMAudioSource.clip = audioManager.GetBGM(currentBGM);
        BGMAudioSource.volume = BGMVolume;
        BGMAudioSource.loop = true;
        BGMAudioSource.Play();
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

    public void PlayerJumpFX(Vector3 position)
    {
        Instantiate(playerJumpFX, position, Quaternion.identity);
    }

    public void PlayerLandFX(Vector3 position)
    {
        // Debug.Log("Player landed at position: " + position);
        Instantiate(playerLandFX, position, Quaternion.identity);
    }

    public GameObject PlayerWallSlideFX(Vector3 position)
    {
        GameObject fx = Instantiate(playerWallSlideFX, position, Quaternion.identity);
        return fx;
    }

    public void PlayerWallJumpFX(Vector3 position)
    {
        Instantiate(playerWallJumpFX, position, Quaternion.identity);
    }

    public GameObject PlayerDashTrailFX(Vector3 position)
    {
        GameObject fx = Instantiate(playerDashTrailFX, position, Quaternion.identity);
        return fx;
    }
}


