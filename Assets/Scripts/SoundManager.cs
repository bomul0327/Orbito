using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    private string audioClipFolderPath = Directory.GetCurrentDirectory() + "\\Assets\\Sounds\\";
    private AudioSource BGM = new AudioSource();
    private AudioSource moveAudio = new AudioSource();
    private AudioSource attackAudio = new AudioSource();
    private AudioSource interactionAudio = new AudioSource();
    private AudioSource SFXAudio = new AudioSource();
    private AudioSource destructionAudio = new AudioSource();
    private AudioSource extraAudio = new AudioSource();
    private Dictionary<string, AudioSource> soundDict = new Dictionary<string, AudioSource>();
    void Add(string audioSource)
    {
        soundDict.Add(audioSource, new AudioSource());
        soundDict[audioSource].clip = JsonManager.Find(audioSource+"Path");
    }
    void Play(string audioSource, float playVolume, float playPitch, float playSpeed)
    {
        soundDict.Add(audioSource, new AudioSource());
        soundDict[audioSource].Play();
    }

    void Play(string audioSource)
    {

    }

    void Stop(string audioSource)
    {

    }

    void Resume(string audioSource)
    {

    }

    void Remove(string audioSource)
    {

    }

    void Mute()
    {

    }

    void SetVolume(string audioSource, float playVolume)
    {

    }

    void UpVolume(string audioSource)
    {
        
    }

    void DownVolume(string audioSource)
    {

    }

    void SetPitch(string audioSource, float playPitch)
    {

    }

    void UpPitch(string audioSource)
    {

    }
    
    void DownPitch(string audioSource)
    {

    }

    void SetSpeed(string audioSource, float playSpeed)
    {

    }

    void UpSpeed(string audioSource)
    {
        
    }

    void DownSpeed(string audioSource)
    {

    }

}
