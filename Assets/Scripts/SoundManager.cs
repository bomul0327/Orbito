using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Singleton<SoundManager>
{
    private string audioClipFolderPath = "Sounds\\";
    // AudioMixerGroup speedAudioMixer = Resources.Load<AudioMixerGroup>("AudioClipSpeedMixer");
    private Dictionary<string, AudioSource> soundDict = new Dictionary<string, AudioSource>();

    void Add(string audioSource)
    {
        soundDict.Add(audioSource, gameObject.AddComponent<AudioSource>() as AudioSource );
        soundDict[audioSource].clip = Resources.Load<AudioClip>(audioClipFolderPath + audioSource);
    }
    /// <summary>
    /// playVolume은 0~1까지의 범위이며, 3 parameter 값은 1이 Default값입니다.
    /// </summary>
    public void Play(string audioSource, float playVolume, float playPitch, float playSpeed)
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
            Debug.Log("So only changed volume, pitch, speed.");
        }
    }

    /// <summary>
    /// 3 parameter 값은 1이 Default값입니다.
    /// </summary>
    public void Play(string audioSource)
    {
        Play(audioSource, 1.0f, 1.0f, 1.0f);
    }

    void BGM(string audioSource)
    {
        Play(audioSource);
        soundDict[audioSource].loop = true;
    }

    public void Replay(string audioSource)
    {
        if (soundDict.ContainsKey(audioSource))
        {
            if (soundDict[audioSource].isPlaying)
            {
                soundDict[audioSource].Stop();
                soundDict[audioSource].Play();
            } else
            {
                Debug.Log(audioSource + " is not playing, so that play it from begining");
                soundDict[audioSource].Play();
            }
        } else
        {
            Debug.Log("There is no "+audioSource);
        }
    }

    public void Stop(string audioSource)
    {
        if (soundDict.ContainsKey(audioSource))
        {
            soundDict[audioSource].Pause();
        } else 
        {
            Debug.Log("There is no "+audioSource);
        }
    }

    public void Resume(string audioSource)
    {
        if (soundDict.ContainsKey(audioSource))
        {
            soundDict[audioSource].UnPause();
        } else 
        {
            Debug.Log("There is no "+audioSource);
        }
    }

    public void Remove(string audioSource)
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

    public void UnMute(string audioSource)
    {
        if (soundDict.ContainsKey(audioSource))
        {
            soundDict[audioSource].mute = false;
        } else 
        {
            Debug.Log("There is no "+audioSource);
        }
    }

    public void MuteAll()
    {
        foreach (var item in soundDict)
        {
            Mute(item.Key);
        }
    }

    public void UnMuteAll()
    {
        foreach (var item in soundDict)
        {
            UnMute(item.Key);
        }
    }

    public void SetVolume(string audioSource, float playVolume)
    {
        if (soundDict.ContainsKey(audioSource))
        {
            soundDict[audioSource].volume = playVolume;
        } else 
        {
            Debug.Log("There is no "+audioSource);
        }
    }

    public void SetPitch(string audioSource, float playPitch)
    {
        if (soundDict.ContainsKey(audioSource))
        {
            // 이부분은 Speed가 한번에 조정되는 기준으로 코딩돼 있기 때문에 Speed가 개별로 조절되어야 한다면 여기도 수정되어야 합니다.

            soundDict[audioSource].outputAudioMixerGroup.audioMixer.GetFloat("pitchBend", out float pitchBend);
            soundDict[audioSource].pitch = playPitch / pitchBend;
        } else 
        {
            Debug.Log("There is no "+audioSource);
        }
    }

    public void SetSpeed(string audioSource, float playSpeed)
    {
        // 현재는 하나의 AudioMixerGroup을 사용하기 때문에 Speed를 조정중인 모든 Sound의 Speed가 동시에 조절됩니다.
        // 전체 소리의 속도가 같이 조절되야 할지 개별의 Sound의 속도가 개별적으로 조절되야 할지에 따라 달라져야 합니다.

        if (soundDict.ContainsKey(audioSource))
        {
            if (soundDict[audioSource].GetComponent<AudioMixerGroup>() == null)
            {
                // soundDict[audioSource].outputAudioMixerGroup = speedAudioMixer;
            }

            foreach (var item in soundDict)
            {
                float prevPitch = item.Value.pitch;

                // speedAudioMixer.audioMixer.GetFloat("pitchBend",out float prevPitchBend);

                // item.Value.pitch = prevPitch * prevPitchBend/playSpeed;
            }

            // speedAudioMixer.audioMixer.SetFloat("pitchBend", 1f/playSpeed);
        } else 
        {
            Debug.Log("There is no "+audioSource);
        }
    }
}
