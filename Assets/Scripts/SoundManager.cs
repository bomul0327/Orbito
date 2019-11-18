using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// 전체적인 AudioSource를 관리하기 위한 Class입니다.
/// </summary>
public class SoundManager : Singleton<SoundManager>
{
    private string audioClipFolderPath = "Sounds\\";
    AudioMixerGroup speedAudioMixer;
    private Dictionary<string, AudioSource> soundDict;

    void Start()
    {
        soundDict = new Dictionary<string, AudioSource>();

        // EnemyAttack 종류의 Sound개수제한 (현재 20개)
        for(int i =1 ; i <= 20 ; i++ )
        {
            soundDict.Add("EnemyAttack"+i, gameObject.AddComponent<AudioSource>() );
        }

        // EnemyMoveMent 종류의 Sound개수제한 (현재 20개)
        for(int i =1 ; i <= 20 ; i++ )
        {
            soundDict.Add("EnemyMoveMent"+i, gameObject.AddComponent<AudioSource>() );
        }

        speedAudioMixer = Resources.Load<AudioMixerGroup>("AudioClipSpeedMixer");
    }

    /// <summary>
    /// 대상 AudioSource를 추가합니다.
    /// </summary>
    /// <param name="audioSource">대상 AudioClip 이름</param>
    public void Add(string audioSource)
    {
        if (!soundDict.ContainsKey(audioSource))
        {
            soundDict.Add(audioSource, gameObject.AddComponent<AudioSource>() );
        }
        soundDict[audioSource].outputAudioMixerGroup = speedAudioMixer;
        soundDict[audioSource].clip = Resources.Load<AudioClip>(audioClipFolderPath + audioSource);
    }

    /// <summary>
    /// AudioSource가 없다면 생성하고 실행시킵니다.
    /// </summary>
    /// <param name="audioSource">대상 AudioClip 이름</param>
    /// <param name="playVolume">실행시킬 Volume값(0~1)</param>
    /// <param name="playPitch">실행시킬 Pitch값; Default : 1</param>
    /// <param name="playSpeed">실행시킬 Speed값; Default : 1</param>
    public void Play(string audioSource, float playVolume, float playPitch, float playSpeed)
    {
        if (!soundDict.ContainsKey(audioSource))
        {
            Add(audioSource);
        }

        SetVolume(audioSource, playVolume);
        SetPitch(audioSource, playPitch);
        SetSpeed(playSpeed);

        if (!soundDict[audioSource].isPlaying)
        {
            soundDict[audioSource].Play();
        }
        else
        {
            Debug.Log("The clip is already playing.");
            Debug.Log("So only changed volume, pitch, speed.");
        }
    }

    /// <summary>
    /// Default Volume, Pitch, Speed로 AudioClip을 실행시킵니다.
    /// </summary>
    /// <param name="audioSource">대상 AudioClip 이름</param>    
    public void Play(string audioSource)
    {
        Play(audioSource, 1.0f, 1.0f, 1.0f);
    }

    /// <summary>
    /// 특정위치에서 소리를 발생시키고 싶을때 사용합니다.
    /// </summary>
    /// <param name="audioSource">대상 AudioClip 이름</param>
    /// <param name="position">소리를 발생시킬 위치</param>
    public void PlayAt(string audioSource, Vector3 position)
    {
        if (!soundDict.ContainsKey(audioSource))
        {
            Add(audioSource);
        }
        AudioSource.PlayClipAtPoint(soundDict[audioSource].clip, position);
    }

    /// <summary>
    /// BGM을 변경합니다. 현재는 fade-out fade-in 기능이 없이 변경됩니다.
    /// </summary>
    /// <param name="audioSource">BGM에 쓰일 대상 AudioClip 이름</param>
    public void SetBGM(string audioSource)
    {
        if ( soundDict.ContainsKey("BGM") )
        {
            soundDict["BGM"].clip = Resources.Load(audioClipFolderPath + audioSource) as AudioClip; 
        } else
        {
            soundDict.Add("BGM", gameObject.AddComponent<AudioSource>());
            soundDict["BGM"].clip = Resources.Load(audioClipFolderPath + audioSource) as AudioClip; 
            soundDict["BGM"].loop = true;
            soundDict["BGM"].playOnAwake = true;
        }
    }

    /// <summary>
    /// 소리를 일시정지 시킵니다.
    /// </summary>
    /// <param name="audioSource">대상 AudioClip 이름</param>
    public void Pause(string audioSource)
    {
        if (soundDict.ContainsKey(audioSource))
        {
            soundDict[audioSource].Pause();
        }
        else
        {
            Debug.Log("There is no " + audioSource);
        }
    }

    /// <summary>
    /// 일시정지된 소리를 다시 재생시킵니다.
    /// </summary>
    /// <param name="audioSource">대상 AudioClip 이름</param>
    public void UnPause(string audioSource)
    {
        if (soundDict.ContainsKey(audioSource))
        {
            soundDict[audioSource].UnPause();
        }
        else
        {
            Debug.Log("There is no " + audioSource);
        }
    }

    /// <summary>
    /// 더이상 필요없는 AudioSource를 제거합니다.
    /// </summary>
    /// <param name="audioSource">대상 Audioclip 이름</param>
    public void Remove(string audioSource)
    {
        if (soundDict.ContainsKey(audioSource))
        {
            Destroy(soundDict[audioSource]);
            soundDict.Remove(audioSource);
        }
        else
        {
            Debug.Log("There is no " + audioSource);
        }
    }

    /// <summary>
    /// 소리를 음소거 시킵니다.(소리는 계속 재생됩니다)
    /// </summary>
    /// <param name="audioSource">대상 Audioclip 이름</param>
    public void Mute(string audioSource)
    {
        if (soundDict.ContainsKey(audioSource))
        {
            soundDict[audioSource].mute = true;
        }
        else
        {
            Debug.Log("There is no " + audioSource);
        }
    }

    /// <summary>
    /// 음소거된 소리의 음소거를 해제합니다.
    /// </summary>
    /// <param name="audioSource">대상 Audioclip 이름</param>
    public void UnMute(string audioSource)
    {
        if (soundDict.ContainsKey(audioSource))
        {
            soundDict[audioSource].mute = false;
        }
        else
        {
            Debug.Log("There is no " + audioSource);
        }
    }

    /// <summary>
    /// 모든 소리를 음소거시킵니다.
    /// </summary>
    public void MuteAll()
    {
        foreach (var item in soundDict)
        {
            Mute(item.Key);
        }
    }

    /// <summary>
    /// 모든 소리를 음소거 해제시킵니다.
    /// </summary>
    public void UnMuteAll()
    {
        foreach (var item in soundDict)
        {
            UnMute(item.Key);
        }
    }

    /// <summary>
    /// 해당 소리의 재생여부에 대한 bool값을 return합니다.
    /// </summary>
    /// <param name="audioSource">대상 Audioclip 이름</param>
    /// <returns></returns>
    public bool isPlaying(string audioSource)
    {
        if (soundDict.ContainsKey(audioSource))
        {
            return soundDict[audioSource].isPlaying;
        } else
        {
            Debug.Log("There is no "+audioSource);
            return false;
        }
    }

    /// <summary>
    /// 해당 소리의 볼륨을 조절합니다.
    /// </summary>
    /// <param name="audioSource">대상 Audioclip 이름</param>
    /// <param name="playVolume">원하는 Volume배율</param>
    public void SetVolume(string audioSource, float playVolume)
    {
        if (soundDict.ContainsKey(audioSource))
        {
            soundDict[audioSource].volume = playVolume;
        }
        else
        {
            Debug.Log("There is no " + audioSource);
        }
    }

    /// <summary>
    /// 해당 소리의 높낮이를 조절합니다.
    /// </summary>
    /// <param name="audioSource">대상 Audioclip 이름</param>
    /// <param name="playPitch">원하는 Pitch 배율</param>
    public void SetPitch(string audioSource, float playPitch)
    {
        if (soundDict.ContainsKey(audioSource))
        {
            // 이부분은 Speed가 한번에 조정되는 기준으로 코딩돼 있기 때문에 Speed가 개별로 조절되어야 한다면 여기도 수정되어야 합니다.

            if (soundDict[audioSource].GetComponent<AudioMixerGroup>() == null)
            {
                soundDict[audioSource].outputAudioMixerGroup = speedAudioMixer;
            }

            soundDict[audioSource].outputAudioMixerGroup.audioMixer.GetFloat("Pitch", out float pitchBend);
            soundDict[audioSource].pitch = playPitch / pitchBend;
        }
        else
        {
            Debug.Log("There is no " + audioSource);
        }
    }

    /// <summary>
    /// 전체 소리의 속도 배율을 조절합니다.
    /// </summary>
    /// <param name="playSpeed">원하는 속도 배율</param>
    public void SetSpeed(float playSpeed)
    {
        // 현재는 하나의 AudioMixerGroup을 사용하기 때문에 Speed를 조정중인 모든 Sound의 Speed가 동시에 조절됩니다.
        // 전체 소리의 속도가 같이 조절되야 할지 개별의 Sound의 속도가 개별적으로 조절되야 할지에 따라 달라져야 합니다.

        foreach (var item in soundDict)
        {
            float prevPitch = item.Value.pitch;
            speedAudioMixer.audioMixer.GetFloat("Pitch", out float prevPitchBend);
            item.Value.pitch = prevPitch * prevPitchBend / playSpeed;
        }

        speedAudioMixer.audioMixer.SetFloat("Pitch", 1f / playSpeed);
    }

    public bool Contains(string audioSource)
    {
        return soundDict.ContainsKey(audioSource);
    }
}
