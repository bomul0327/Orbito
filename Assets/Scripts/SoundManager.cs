using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 전체적인 AudioCli들을 관리하기 위한 Singleton Class입니다.
/// </summary>
public class SoundManager : Singleton<SoundManager>
{
    FMOD.RESULT result;
    FMOD.ChannelGroup channelGroup;
    FMOD.Channel[] channels;
    FMOD.System system;
    FMOD.Sound sound;
    void Start()
    {
        system = FMODUnity.RuntimeManager.CoreSystem;
    }

    /// <summary>
    /// AudioSource가 없다면 생성하고 실행시킵니다.
    /// </summary>
    /// <param name="audioClip">대상 audioClip 이름</param>
    /// <param name="playVolume">실행시킬 Volume값(0~1)</param>
    /// <param name="playPitch">실행시킬 Pitch값; Default : 1</param>
    /// <param name="playSpeed">실행시킬 Speed값; Default : 1</param>
    public void PlaySFX(string audioClip, float playVolume = 1.0f, float playPitch = 1.0f, float playSpeed = 1.0f)
    {

    }

    /// <summary>
    /// BGM을 변경합니다.
    /// </summary>
    /// <param name="audioClip">변경될 BGM audioClip 이름</param>
    public void SetBGM(string audioClip)
    {

    }

    /// <summary>
    /// 특정위치에서 SFX를 발생시키고 싶을때 사용합니다.
    /// </summary>
    /// <param name="audioClip">대상 audioClip 이름</param>
    /// <param name="position">SFX를 발생시킬 위치</param>
    public void PlayAt(string audioClip, Vector3 position)
    {

    }

    /// <summary>
    /// 더이상 필요없는 AudioClip을 제거합니다.
    /// </summary>
    /// <param name="audioClip">대상 audioClip 이름</param>
    public void Remove(string audioClip)
    {

    }

    /// <summary>
    /// 해당 소리를 일시정지 시킵니다.
    /// </summary>
    /// <param name="audioClip">대상 audioClip 이름</param>
    public void Pause(string audioClip)
    {

    }

    /// <summary>
    /// 일시정지된 소리를 다시 재생시킵니다.
    /// </summary>
    /// <param name="audioClip">대상 audioClip 이름</param>
    public void UnPause(string audioClip)
    {

    }

    /// <summary>
    /// 해당 소리를 음소거 시킵니다. 소리는 계속 재생됩니다
    /// </summary>
    /// <param name="audioClip">대상 audioClip 이름</param>
    public void Mute(string audioClip)
    {

    }

    /// <summary>
    /// 음소거된 소리의 음소거를 해제합니다.
    /// </summary>
    /// <param name="audioClip">대상 audioClip 이름</param>
    public void UnMute(string audioClip)
    {

    }

    /// <summary>
    /// 모든 소리를 음소거시킵니다.
    /// </summary>
    public void MuteAll()
    {

    }

    /// <summary>
    /// 모든 소리를 음소거 해제시킵니다.
    /// </summary>
    public void UnMuteAll()
    {

    }

    /// <summary>
    /// 해당 소리의 재생여부에 대한 bool값을 return합니다.
    /// </summary>
    /// <param name="audioClip">대상 audioClip 이름</param>
    /// <returns></returns>
    public bool isPlaying(string audioClip)
    {

        return false;
    }

    /// <summary>
    /// 해당 소리의 볼륨을 조절합니다.
    /// </summary>
    /// <param name="audioClip">대상 audioClip 이름</param>
    /// <param name="playVolume">원하는 Volume배율</param>
    public void SetVolume(string audioClip, float playVolume)
    {

    }

    /// <summary>
    /// 해당 소리의 높낮이를 조절합니다.
    /// </summary>
    /// <param name="audioClip">대상 audioClip 이름</param>
    /// <param name="playPitch">원하는 Pitch 배율</param>
    public void SetPitch(string audioClip, float playPitch)
    {

    }

    /// <summary>
    /// 전체 소리의 속도 배율을 조절합니다.
    /// </summary>
    /// <param name="playSpeed">원하는 속도 배율</param>
    public void SetSpeed(string audioClip, float playSpeed)
    {

    }
}
