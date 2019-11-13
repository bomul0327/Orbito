using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class SoundManager : Singleton<SoundManager>
{
    private string audioClipFolderPath = Application.dataPath + "\\Sounds\\";
    private Dictionary<string, AudioSource> soundDict = new Dictionary<string, AudioSource>();
    void Add(string audioSource)
    {
        soundDict.Add(audioSource, gameObject.AddComponent<AudioSource>() as AudioSource );
        soundDict[audioSource].clip = (AudioClip) AssetDatabase.LoadAssetAtPath(audioClipFolderPath + audioSource, typeof(AudioClip));
    }
    void Play(string audioSource, float playVolume, float playPitch, float playSpeed)
    {
        if (!soundDict.ContainsKey(audioSource))
        {
            Add(audioSource);
        } 

        SetVolume(audioSource, playVolume);
        SetPitch(audioSource,playPitch);
        SetSpeed(audioSource,playSpeed);

        if (!soundDict[audioSource].isPlaying)
        {
            soundDict[audioSource].Play();
        } else
        {
            Debug.Log("The clip is already playing.");
            Debug.Log("So only changed volume, pitch, speed.")
        }
    }

    void Play(string audioSource)
    {
        Play(audioSource, 0.5f, 0.5f, 0.5f);
    }

    void Replay(string audioSource)
    {
        if (soundDict[audioSource].isPlaying)
        {
            float prevVolume = soundDict[audioSource].volume;
            float prevPitch = soundDict[audioSource].pitch;
            
            // 아래의 prevSpeed는 더미데이터입니다
            float prevSpeed = 1.0f;
            // Speed 멤버가 없기때문에 다른 방식으로 구현해야함
            // float prevSpeed = soundDict[audioSource].speed;

            soundDict[audioSource].Stop();
            Play(audioSource, prevVolume, prevPitch, prevSpeed);
        }
    }

    void Stop(string audioSource)
    {
        if (soundDict.ContainsKey(audioSource))
        {
            soundDict[audioSource].Pause();
        } else 
        {
            Debug.Log("There is no "+audioSource);
        }
    }

    void Resume(string audioSource)
    {
        if (soundDict.ContainsKey(audioSource))
        {
            soundDict[audioSource].UnPause();
        } else 
        {
            Debug.Log("There is no "+audioSource);
        }
    }

    void Remove(string audioSource)
    {
        if (soundDict.ContainsKey(audioSource))
        {
            soundDict.Remove(audioSource);
        } else 
        {
            Debug.Log("There is no "+audioSource);
        }
    }

    void Mute(string audioSource)
    {
        if (soundDict.ContainsKey(audioSource))
        {
            soundDict[audioSource].mute = true;
        } else 
        {
            Debug.Log("There is no "+audioSource);
        }
    }

    void UnMute(string audioSource)
    {
        if (soundDict.ContainsKey(audioSource))
        {
            soundDict[audioSource].mute = false;
        } else 
        {
            Debug.Log("There is no "+audioSource);
        }
    }

    void MuteAll()
    {
        foreach (var item in soundDict)
        {
            Mute(item.Key);
        }
    }

    void UnMuteAll()
    {
        foreach (var item in soundDict)
        {
            UnMute(item.Key);
        }
    }

    void SetVolume(string audioSource, float playVolume)
    {
        if (soundDict.ContainsKey(audioSource))
        {
            soundDict[audioSource].volume = playVolume;
        } else 
        {
            Debug.Log("There is no "+audioSource);
        }
    }

    void UpVolume(string audioSource)
    {
        SetVolume(audioSource, soundDict[audioSource].volume + 0.1f);
    }

    void DownVolume(string audioSource)
    {
        SetVolume(audioSource, soundDict[audioSource].volume - 0.1f);
    }

    void SetPitch(string audioSource, float playPitch)
    {
        if (soundDict.ContainsKey(audioSource))
        {
            soundDict[audioSource].pitch = playPitch;
        } else 
        {
            Debug.Log("There is no "+audioSource);
        }
    }

    void UpPitch(string audioSource)
    {
        SetPitch(audioSource, soundDict[audioSource].pitch + 0.1f);
    }
    
    void DownPitch(string audioSource)
    {
        SetPitch(audioSource, soundDict[audioSource].pitch - 0.1f);
    }

    void SetSpeed(string audioSource, float playSpeed)
    {
        if (soundDict.ContainsKey(audioSource))
        {
            soundDict[audioSource].mute = true;
        } else 
        {
            Debug.Log("There is no "+audioSource);
        }
    }

    void UpSpeed(string audioSource)
    {
    }

    void DownSpeed(string audioSource)
    {

    }

}
