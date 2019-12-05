using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
// Marshall library이용을 위해 향후 필요 가능성 있음.
using System.Runtime.InteropServices;

/// <summary>
/// 전체적인 audioFile들을 관리하기 위한 Singleton Class입니다.
/// </summary>
public class SoundManager : Singleton<SoundManager>
{
    const int maxChannelNum = 32;
    const int BGMChannelIndex = 31;
    // soundPath를 사용하면 관리하기는 편리해지지만 많은 string concat을 실시하게 됩니다.
    // string concat을 지양하려면 json속 모든 path앞에 추가로 달아주면됩니다.
    static string soundPath = Application.streamingAssetsPath + "/";
    FMOD.System system;
    FMOD.ChannelGroup MasterChannelGroup;
    FMOD.ChannelGroup SFXChannelGroup;
    FMOD.SoundGroup soundGroup;
    FMOD.Channel[] channelArr;
    string currentBGM;
    static FMOD.RESULT result;
    JObject soundPathJson;
    Dictionary<string, FMOD.Sound> soundDict;
    void Start()
    {
        soundDict = new Dictionary<string, FMOD.Sound>();
        currentBGM = null;

        result = FMOD.Factory.System_Create(out system);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogAssertionFormat(string.Format("FMOD error! {0} {1}", result, FMOD.Error.String(result)));
        }

        // 한번에 다룰 최대 음원의 개수입니다.
        result = system.setSoftwareChannels(64);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogAssertionFormat(string.Format("FMOD error! {0} : {1}", result, FMOD.Error.String(result)));
        }

        
        // 현재 MaxChannel 개수는 임의로 넣었습니다. 아직 도입 과정이라 channel이 몇개 필요할지 모르겠네요.
        // 나머지는 기본값입니다.
        result = system.init(maxChannelNum, FMOD.INITFLAGS.VOL0_BECOMES_VIRTUAL, System.IntPtr.Zero);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogAssertionFormat(string.Format("FMOD error! {0} : {1}", result, FMOD.Error.String(result)));
        }

        // Channel 초기설정 part입니다.
        result = system.getMasterChannelGroup(out MasterChannelGroup);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogAssertionFormat(string.Format("FMOD error! {0} : {1}", result, FMOD.Error.String(result)));
        }

        result = system.createChannelGroup("SFX", out SFXChannelGroup);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogAssertionFormat(string.Format("FMOD error! {0} : {1}", result, FMOD.Error.String(result)));
        }

        result = MasterChannelGroup.addGroup(SFXChannelGroup);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogAssertionFormat(string.Format("FMOD error! {0} : {1}", result, FMOD.Error.String(result)));
        }

        FMOD.Channel.usingChannel = new bool[maxChannelNum];
        channelArr = new FMOD.Channel[maxChannelNum];
        for (int i =0 ; i < 20; i++)
        {
            // SFX 전용 channel은 channelIndex 0~19로 임의로 정하겠습니다. 변경이 필요합니다.
            system.getChannel(i, out channelArr[i]);
            channelArr[i].setChannelGroup(SFXChannelGroup);
            FMOD.Channel.usingChannel[i] = false;
        }
        
        for (int i = 20 ; i < maxChannelNum-1; i++)
        {
            // 그 외의 소리에 사용될 채널들입니다.
            system.getChannel(i, out channelArr[i]);
            channelArr[i].setChannelGroup(MasterChannelGroup);
            FMOD.Channel.usingChannel[i] = false;
        }

            // BGM용 채널은 마지막에 넣었습니다.
            system.getChannel(maxChannelNum-1, out channelArr[maxChannelNum-1]);
            channelArr[maxChannelNum-1].setChannelGroup(MasterChannelGroup);
            FMOD.Channel.usingChannel[maxChannelNum-1] = false;

        // Sound 초기설정 part입니다.
        result = system.getMasterSoundGroup(out soundGroup);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogAssertionFormat(string.Format("FMOD error! {0} : {1}", result, FMOD.Error.String(result)));
        }


        // test용이라 compile 에러나서 주석처리했습니다.
        // Load("LevelExampleSFX");
    }
    ~SoundManager()
    {
        system.close();
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
            SetBGM("BGMSample.wav");
        }
    }

    /// <summary>
    /// 2D SFX사운드를 발생시킵니다.
    /// </summary>
    /// <param name="audioFile">대상 audioFile 이름</param>
    /// <param name="playVolume">실행시킬 Volume값(0~1)</param>
    /// <param name="playPitch">실행시킬 Pitch값; Default : 1</param>
    /// <param name="playSpeed">실행시킬 Speed값; Default : 1</param>
    public void PlaySFX(string audioFile, float playVolume = 1.0f, float playPitch = 1.0f, float playSpeed = 1.0f)
    {

        int channelIndex = FMOD.Channel.findUnusingSFXChannel();
        if (channelIndex == -1)
        {
            Debug.Log("There is no extra SFX channel now.");
            return;
        }

        if (!soundDict.ContainsKey(audioFile))
        {
            Debug.Log("Load the sound file before play.");
            return;
        } else
        {
            result = system.playSound(soundDict[audioFile], SFXChannelGroup, true, out channelArr[channelIndex]);
            if (result != FMOD.RESULT.OK)
            {
                Debug.LogAssertionFormat(string.Format("FMOD error! {0} : {1}", result, FMOD.Error.String(result)));
            }

            result = channelArr[channelIndex].setVolume(playVolume);
            if (result != FMOD.RESULT.OK)
            {
                Debug.LogAssertionFormat(string.Format("FMOD error! {0} : {1}", result, FMOD.Error.String(result)));
            }

            result = channelArr[channelIndex].setPitch(playPitch);
            if (result != FMOD.RESULT.OK)
            {
                Debug.LogAssertionFormat(string.Format("FMOD error! {0} : {1}", result, FMOD.Error.String(result)));
            }
            
            // 아 이 코드 한줄 때문에 계속 실행이 안됬었습니다.!!!!!!!!!
            // channelArr[channelIndex].setFrequency(playSpeed/playPitch);

            result = channelArr[channelIndex].setPaused(false);
            if (result != FMOD.RESULT.OK)
            {
                Debug.LogAssertionFormat(string.Format("FMOD error! {0} : {1}", result, FMOD.Error.String(result)));
            }
        }
    }

    /// <summary>
    /// BGM을 변경합니다. BGM은 기본적으로 stream으로 읽어옵니다.
    /// </summary>
    /// <param name="audioFile">변경될 BGM audioFile 이름</param>
    public void SetBGM(string audioFile, float playVolume = 1.0f, float playPitch = 1.0f, float playSpeed = 1.0f)
    {
        Load(audioFile, false);
        // Fade in Fade out 기능이 추가되어야 합니다.
        result = system.playSound(soundDict[audioFile], MasterChannelGroup, true, out channelArr[maxChannelNum-1]);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogAssertionFormat(string.Format("FMOD error! {0} : {1}", result, FMOD.Error.String(result)));
        }

        if (!currentBGM.Equals(null))
        {
            UnLoad(currentBGM);
        }
    }

    /// <summary>
    /// 특정위치에서 3D sound를 발생시키고 싶을때 사용합니다.
    /// </summary>
    /// <param name="audioFile">대상 audioFile 이름</param>
    /// <param name="position">SFX를 발생시킬 위치</param>
    public void PlayAt(string audioFile, Vector3 position)
    {

    }


    /// <summary>
    /// 해당 소리를 일시정지 시킵니다.
    /// </summary>
    /// <param name="audioFile">대상 audioFile 이름</param>
    public void Pause(string audioFile)
    {
        result = FindChannelOfSound(audioFile).setPaused(true);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogAssertionFormat(string.Format("FMOD error! {0} : {1}", result, FMOD.Error.String(result)));
        }
    }

    /// <summary>
    /// 일시정지된 소리를 다시 재생시킵니다.
    /// </summary>
    /// <param name="audioFile">대상 audioFile 이름</param>
    public void UnPause(string audioFile)
    {
        result = FindChannelOfSound(audioFile).setPaused(false);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogAssertionFormat(string.Format("FMOD error! {0} : {1}", result, FMOD.Error.String(result)));
        }
    }

    /// <summary>
    /// 해당 소리를 음소거 시킵니다. 소리는 계속 재생됩니다.
    /// </summary>
    /// <param name="audioFile">대상 audioFile 이름</param>
    public void Mute(string audioFile)
    {
        result = FindChannelOfSound(audioFile).setMute(true);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogAssertionFormat(string.Format("FMOD error! {0} : {1}", result, FMOD.Error.String(result)));
        }
    }

    /// <summary>
    /// 음소거된 소리의 음소거를 해제합니다.
    /// </summary>
    /// <param name="audioFile">대상 audioFile 이름</param>
    public void UnMute(string audioFile)
    {
        result = FindChannelOfSound(audioFile).setMute(false);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogAssertionFormat(string.Format("FMOD error! {0} : {1}", result, FMOD.Error.String(result)));
        }
    }

    /// <summary>
    /// 모든 소리를 음소거시킵니다.
    /// </summary>
    public void MuteAll()
    {
        foreach(var c in channelArr)
        {
            result = c.setMute(true);
            if (result != FMOD.RESULT.OK)
            {
                Debug.LogAssertionFormat(string.Format("FMOD error! {0} : {1}", result, FMOD.Error.String(result)));
            }
        }
    }

    /// <summary>
    /// 모든 소리를 음소거 해제시킵니다.
    /// </summary>
    public void UnMuteAll()
    {
        foreach(var c in channelArr)
        {
            result = c.setMute(true);
            if (result != FMOD.RESULT.OK)
            {
                Debug.LogAssertionFormat(string.Format("FMOD error! {0} : {1}", result, FMOD.Error.String(result)));
            }
        }
    }

    /// <summary>
    /// 해당 소리의 재생여부에 대한 bool값을 return합니다.
    /// </summary>
    /// <param name="audioFile">대상 audioFile 이름</param>
    /// <returns></returns>
    public bool isPlaying(string audioFile)
    {
        bool isPlaying;
        result = FindChannelOfSound(audioFile).isPlaying(out isPlaying);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogAssertionFormat(string.Format("FMOD error! {0} : {1}", result, FMOD.Error.String(result)));
        }
        return isPlaying;
    }

    /// <summary>
    /// 해당 소리의 볼륨을 조절합니다.
    /// </summary>
    /// <param name="audioFile">대상 audioFile 이름</param>
    /// <param name="playVolume">원하는 Volume배율</param>
    public void SetVolume(string audioFile, float playVolume)
    {
        result = FindChannelOfSound(audioFile).setVolume(playVolume);
    }

    /// <summary>
    /// 해당 소리의 높낮이를 조절합니다.
    /// </summary>
    /// <param name="audioFile">대상 audioFile 이름</param>
    /// <param name="playPitch">원하는 Pitch</param>
    public void SetPitch(string audioFile, float playPitch)
    {
        
    }

    /// <summary>
    /// 해당 소리의 속도를 조절합니다.
    /// </summary>
    /// <param name="audioFile">대상 audioFile 이름</param>
    /// <param name="playSpeed">상대 배율값입니다. Default : 1, Range[0.01~100]</param>
    public void SetSpeed(string audioFile, float playSpeed)
    {
        result = soundDict[audioFile].setMusicSpeed(playSpeed);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogAssertionFormat(string.Format("FMOD error! {0} : {1}", result, FMOD.Error.String(result)));
        }
    }

    /// <summary>
    /// 게임시작시에 미리 memory에 올려놓을 자주쓰고 크기가 작은 audioFile들을 불러옵니다.
    /// </summary>
    /// <param name="audioFile">([Level]+[Number]+[SoundType])의 형식을 지켜주세요. Ex) LevelOneSFX, LevelTwodEffect</param>
    /// <returns></returns>
    public void Load(string audioFile, bool stayOnMemory)
    {
        // 예외처리를 어디까지 해줘야 할까요? 
        soundPathJson = JsonManager.Find(audioFile);
        FMOD.Sound soundBuffer;
        FMOD.MODE mode;
            mode = (FMOD.MODE) System.Enum.Parse(typeof(FMOD.MODE), soundPathJson["FMOD.MODE"].ToString());
        // 아직 폴더구조가 정리가 안된 상태에서의 soundPath입니다.
        if (stayOnMemory)
        {
            result = system.createStream(soundPath + soundPathJson["Path"].ToString(), mode, out soundBuffer);
            if (result != FMOD.RESULT.OK)
            {
                Debug.Log(audioFile + "을(를) 불러오지 못했습니다.");
                Debug.LogAssertionFormat(string.Format("FMOD error! {0} : {1}", result, FMOD.Error.String(result)));
            }
            else
            {
                soundDict.Add(audioFile, soundBuffer);
            }
        }
        else
        {
            result = system.createSound(soundPath + soundPathJson["Path"].ToString(), mode, out soundBuffer);
            if (result != FMOD.RESULT.OK)
            {
                Debug.Log(audioFile + "을(를) 불러오지 못했습니다.");
                Debug.LogAssertionFormat(string.Format("FMOD error! {0} : {1}", result, FMOD.Error.String(result)));
            }
            else
            {
                soundDict.Add(audioFile, soundBuffer);
            }
        }
    }

    /// <summary>
    /// Runtime시에 동적으로 audioFile을 memory에서 내리기 위한 함수입니다.
    /// </summary>
    /// <param name="audioFile">대상 audioFile 이름</param>
    /// <returns></returns>
    public void UnLoad(string audioFile)
    {
        if (soundDict.ContainsKey(audioFile))
        {
            result = soundDict[audioFile].release();
            if (result != FMOD.RESULT.OK)
            {
                Debug.LogAssertionFormat(string.Format("FMOD error! {0} : {1}", result, FMOD.Error.String(result)));
            } else
            {
                soundDict.Remove(audioFile);
            }
        }
        else
        {
            Debug.LogAssertionFormat(string.Format("FMOD error! {0} : {1}", result, FMOD.Error.String(FMOD.RESULT.FILE_NOT_IN)));
        }
    }

    /// <summary>
    /// 해당 소리에 Loop option을 추가합니다.
    /// </summary>
    /// <param name="audioFile">대상 audioFile 이름</param>
    public void LoopOn(string audioFile)
    {
        result = soundDict[audioFile].setMode(FMOD.MODE.LOOP_NORMAL);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogAssertionFormat(string.Format("FMOD error! {0} : {1}", result, FMOD.Error.String(result)));
        }
    }

    FMOD.Channel FindChannelOfSound(string audioFile)
    {
        FMOD.Sound soundbuffer;
        foreach(var c in channelArr)
        {
            c.getCurrentSound(out soundbuffer);
            if (soundbuffer.Equals(soundDict[audioFile]))
            {
                return c;
            }
        }

        Debug.Log("That Sound is not playing!\nReturned channel is NULL value");
        return new FMOD.Channel();
    }
}
