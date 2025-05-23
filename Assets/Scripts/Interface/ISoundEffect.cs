using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISoundEffectController
{

    public AudioClip GetSoundClip(SoundType soundType);
    public AudioSource GetAudioSource();
    public void ReleaseAudioSource(AudioSource audioSource);
    public void ChangerBGM(string BGMName);
    public void SetSoundEffectVolume(float volume);
    public void SetBGMVolume(float volume);    
}
