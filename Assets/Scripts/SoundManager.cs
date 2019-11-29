using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

/// <summary>
/// 전체적인 AudioClip들을 관리하기 위한 Singleton Class입니다.
/// </summary>
public class SoundManager : Singleton<SoundManager>
{
    const int channelNum = 32;
    FMOD.RESULT result;
    FMOD.ChannelGroup MasterChannelGroup;
    FMOD.ChannelGroup SFXChannelGroup;
    FMOD.Channel[] channelArr;
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
        result = system.init(channelNum, FMOD.INITFLAGS.NORMAL, System.IntPtr.Zero);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogAssertionFormat(string.Format("FMOD error! {0} : {1}", result, FMOD.Error.String(result)));
        }

        MasterChannelGroup = new FMOD.ChannelGroup();
        SFXChannelGroup = new FMOD.ChannelGroup();
        MasterChannelGroup.addGroup(SFXChannelGroup);
        channelArr = new FMOD.Channel[channelNum];
        FMOD.Channel.usingChannel = new bool[channelNum];
        for (int i =0 ; i < channelNum; i++)
        {
            // SFX 전용 channel은 channelIndex 0~19로 임의로 정하겠습니다. 변경이 필요합니다.
            channelArr[i] = new FMOD.Channel();
            channelArr[i].setChannelGroup(MasterChannelGroup);
            FMOD.Channel.usingChannel[i] = false;
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

    bool playing = false;
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

        // channelGroup.isPlaying으로 확인결과 false지만 해당 channel.isPlaying은 true로 나옴 channelGroup과 channel의 연관성을 살펴봐야할듯.
        MasterChannelGroup.isPlaying(out playing);
        if (playing)
        {  
            Debug.Log("It is plyaing but the sound is muted?");
        } else
        {
            Debug.Log("It is not playing.");
        }
    }

    FMOD.RESULT AddSound(string audioClip, FMOD.MODE mode)
    {
        if (!soundDict.ContainsKey(audioClip))
        {
            FMOD.Sound sound;
            string path = Application.streamingAssetsPath + "/" + audioClip;

            Debug.Log(path);

            FMOD.CREATESOUNDEXINFO info = new FMOD.CREATESOUNDEXINFO();
            info.cbsize = Marshal.SizeOf(typeof(FMOD.CREATESOUNDEXINFO));
            info.format = FMOD.SOUND_FORMAT.PCMFLOAT;

            result = system.createSound(path, mode, out sound);
            if (result != FMOD.RESULT.OK)
            {
                Debug.Log("The sound is not in dictionary but failed to createsound.");
                Debug.LogAssertionFormat(string.Format("FMOD error! {0} : {1}", result, FMOD.Error.String(result)));
                return result;
            }

            soundDict.Add(audioClip, sound);

            Debug.Log("The sound is not in dictionary and added successfuly.");
            return result;
        }
        else
        {
            Debug.Log("The sound is already in dictionary.");
            return FMOD.RESULT.FILE_ARLEADY_IN;
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

        int channelIndex = FMOD.Channel.findUnusingSFXChannel();
        if (channelIndex == -1)
        {
            Debug.Log("There is no extra channel now.");
            return;
        }

        // 현재는 SFX에 한해 2D sound, No Loop 옵션입니다. 변경 가능합니다.
        AddSound(audioClip, FMOD.MODE._2D | FMOD.MODE.LOOP_OFF);
        system.playSound(soundDict[audioClip], SFXChannelGroup, true, out channelArr[channelIndex]);
        channelArr[channelIndex].setVolume(playVolume);
        channelArr[channelIndex].setPitch(playPitch);

        // 아 이 코드 한줄 때문에 계속 실행이 안됬었습니다.!!!!!!!!!
        // channelArr[channelIndex].setFrequency(playSpeed/playPitch);

        channelArr[channelIndex].setPaused(false);
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
        string path = Application.streamingAssetsPath + "/";
        FMOD.Sound sound;
        string audioClip = "LaserSample1.wav";
        system.createSound(path + audioClip, FMOD.MODE.DEFAULT, out sound);
        soundDict.Add(audioClip, sound);
        audioClip = "LaserSample2.wav";
        system.createSound(path + audioClip, FMOD.MODE.DEFAULT, out sound);
        soundDict.Add(audioClip, sound);
        audioClip = "LaserSample3.wav";
        system.createSound(path + audioClip, FMOD.MODE.DEFAULT, out sound);
        soundDict.Add(audioClip, sound);
        audioClip = "BGMSample.wav";
        system.createSound(path + audioClip, FMOD.MODE.DEFAULT, out sound);
        soundDict.Add(audioClip, sound);
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
