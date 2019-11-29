using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

/// <summary>
/// 전체적인 AudioClip들을 관리하기 위한 Singleton Class입니다.
/// </summary>
public class SoundManager : Singleton<SoundManager>
{
    FMOD.RESULT result;
    FMOD.ChannelGroup channelGroup;
    FMOD.System system;
    Dictionary<string, FMOD.Sound> soundDict;
    void Start()
    {
        soundDict = new Dictionary<string, FMOD.Sound>();

        result = FMOD.Factory.System_Create(out system);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogAssertionFormat(string.Format("FMOD error! {0} {1}", result, FMOD.Error.String(result)));
        }

        // 현재 MaxChannel 개수는 임의로 넣었습니다. 아직 도입 과정이라 channel이 몇개 필요할지 모르겠네요.
        // 나머지는 기본값입니다.
        result = system.init(32, FMOD.INITFLAGS.NORMAL, System.IntPtr.Zero);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogAssertionFormat(string.Format("FMOD error! {0} : {1}", result, FMOD.Error.String(result)));
        }


        result = LoadOnStart();
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogAssertionFormat(string.Format("FMOD error! {0} : {1}", result, FMOD.Error.String(result)));
        }
    }

    ~SoundManager()
    {
        system.release();
    }

    /// <summary>
    /// Test용 Update함수입니다. SpaceBar를 누르면 example 2D SFX 사운드를 play합니다.
    /// 완성본에서는 지워질 함수입니다.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlaySFX("LaserSample1.wav");
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Load("Master");
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
        }
    }

    void AddSound(string audioClip, FMOD.MODE mode, out FMOD.Sound sound)
    {
        if (!soundDict.ContainsKey(audioClip))
        {
            string path = Application.streamingAssetsPath + "/" + audioClip;
            Debug.Log(path);
            FMOD.CREATESOUNDEXINFO info = new FMOD.CREATESOUNDEXINFO();
            info.cbsize = Marshal.SizeOf(typeof(FMOD.CREATESOUNDEXINFO));
            info.format = FMOD.SOUND_FORMAT.PCMFLOAT;

            result = system.createSound(path, mode, ref info, out sound);
            if (result != FMOD.RESULT.OK)
            {
                Debug.Log("The sound is not in dictionary but failed to createsound.");
                Debug.LogAssertionFormat(string.Format("FMOD error! {0} : {1}", result, FMOD.Error.String(result)));
            }

            soundDict.Add(audioClip, sound);

            Debug.Log("The sound is not in dictionary and added successfuly.");
        }
        else
        {
            sound = new FMOD.Sound();
            Debug.Log("The sound is already in dictionary.");
        }
    }

    /// <summary>
    /// 2D SFX사운드를 발생시킵니다.
    /// </summary>
    /// <param name="audioClip">대상 audioClip 이름</param>
    /// <param name="playVolume">실행시킬 Volume값(0~1)</param>
    /// <param name="playPitch">실행시킬 Pitch값; Default : 1</param>
    /// <param name="playSpeed">실행시킬 Speed값; Default : 1</param>
    public void PlaySFX(string audioClip, float playVolume = 1.0f, float playPitch = 1.0f, float playSpeed = 1.0f)
    {
        FMOD.Sound sound;
        AddSound(audioClip, FMOD.MODE.OPENMEMORY_POINT, out sound);
        FMOD.Channel channel;
        system.playSound(sound, channelGroup, false, out channel);
        channel.setVolume(playVolume);
        channel.setPitch(playPitch);
        channel.setFrequency(playSpeed/playPitch);

        // channel.setPaused(false);
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
        FMODUnity.RuntimeManager.PlayOneShot("event:/"+audioClip, position);
    }

    /// <summary>
    /// 더이상 필요없는 AudioClip을 제거합니다.
    /// </summary>
    /// <param name="audioClip">대상 audioClip 이름</param>
    public void Remove(string audioClip)
    {
;
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

    /// <summary>
    /// 게임시작시에 미리 memory에 올려놓을 자주쓰고 크기가 작은 audioclip들을 불러옵니다.
    /// </summary>
    /// <returns></returns>
    private FMOD.RESULT LoadOnStart()
    {
        // To do

        return FMOD.RESULT.OK;
    }

    /// <summary>
    /// Runtime시에 동적으로 audioClip을 memory에 올리기 위한 함수입니다.
    /// </summary>
    /// <param name="audioClip">대상 audioClip 이름</param>
    /// <returns></returns>
    public FMOD.RESULT Load(string audioClip)
    {
        // To do
        FMODUnity.RuntimeManager.LoadBank(audioClip);
        return FMOD.RESULT.OK;
    }

    /// <summary>
    /// Runtime시에 동적으로 audioClip을 memory에서 내리기 위한 함수입니다.
    /// </summary>
    /// <param name="audioClip">대상 audioClip 이름</param>
    /// <returns></returns>
    public FMOD.RESULT UnLoad(string audioClip)
    {
        // To do
        return FMOD.RESULT.OK;
    }
}
