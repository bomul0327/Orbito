using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 이거 using 안하고 쓸수 있도록
using Newtonsoft.Json.Linq;
using FMOD;
// Marshall library이용을 위해 향후 필요 가능성 있음.
using System.Runtime.InteropServices;
using Debug = UnityEngine.Debug;

// 현 진행상황 : 기초적인 부분은 되어갑니다.
// 한가지 sound로 중복되어 소리를 내고 wav파일 및 모든 음원형식의 파일의 speed를 조절하기 위해
// FMOD.MODE.CREATESAMPLE 및 DSP 공부중입니다.
// memory 관리는 계속 조금씩 늘어가는 중입니다.
// unity editor에서 play exit을 반복할 시 memory가 0.03GB만큼 늘어가는 현상이 있습니다.
// Release되지 않은 메모리가 있는것으로 보입니다.

/// <summary>
/// 전체적인 audioFile들을 관리하기 위한 Singleton Class입니다.
/// </summary>
public class SoundManager : Singleton<SoundManager>
{
    const int maxChannelNum = 32;
    const int BGMChannelIndex = 31;
    // soundPath를 사용하면 관리하기는 편리해지지만 많은 string concat을 실시하게 됩니다.
    // string concat을 지양하려면 json속 모든 path앞에 추가로 달아주면됩니다.
    string soundPath = Application.streamingAssetsPath + "/";
    FMOD.System system;
    FMOD.ChannelGroup MasterChannelGroup;
    FMOD.ChannelGroup SFXChannelGroup;
    FMOD.SoundGroup soundGroup;
    FMOD.Channel[] channelArr;
    string currentBGMName;
    static FMOD.RESULT result;
    JObject soundPathJson;
    Dictionary<string, FMOD.Sound> soundDict;
    void Start()
    {
        soundDict = new Dictionary<string, FMOD.Sound>();
        currentBGMName = "null";

        result = FMOD.Factory.System_Create(out system);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogAssertionFormat("FMOD error! {0} {1}", result, FMOD.Error.String(result));
        }

        // 한번에 다룰 최대 음원의 개수입니다.
        result = system.setSoftwareChannels(64);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogAssertionFormat("FMOD error! {0} : {1}", result, FMOD.Error.String(result));
        }

        
        // 현재 MaxChannel 개수는 임의로 넣었습니다. 아직 도입 과정이라 channel이 몇개 필요할지 모르겠네요.
        // 나머지는 기본값입니다.
        result = system.init(maxChannelNum, FMOD.INITFLAGS.VOL0_BECOMES_VIRTUAL, System.IntPtr.Zero);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogAssertionFormat("FMOD error! {0} : {1}", result, FMOD.Error.String(result));
        }

        // Channel 초기설정 part입니다.
        result = system.getMasterChannelGroup(out MasterChannelGroup);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogAssertionFormat("FMOD error! {0} : {1}", result, FMOD.Error.String(result));
        }

        result = system.createChannelGroup("SFX", out SFXChannelGroup);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogAssertionFormat("FMOD error! {0} : {1}", result, FMOD.Error.String(result));
        }

        result = MasterChannelGroup.addGroup(SFXChannelGroup);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogAssertionFormat("FMOD error! {0} : {1}", result, FMOD.Error.String(result));
        }

        channelArr = new FMOD.Channel[maxChannelNum];
        for (int i =0 ; i < 20; i++)
        {
            // SFX 전용 channel은 channelIndex 0~19로 임의로 정하겠습니다. 변경이 필요합니다.
            system.getChannel(i, out channelArr[i]);
            channelArr[i].setChannelGroup(SFXChannelGroup);
        }
        
        for (int i = 20 ; i < maxChannelNum-1; i++)
        {
            // 그 외의 소리에 사용될 채널들입니다.
            system.getChannel(i, out channelArr[i]);
            channelArr[i].setChannelGroup(MasterChannelGroup);
        }

            // BGM용 채널은 마지막에 넣었습니다.
            system.getChannel(maxChannelNum-1, out channelArr[maxChannelNum-1]);
            channelArr[maxChannelNum-1].setChannelGroup(MasterChannelGroup);

        // Sound 초기설정 part입니다.
        result = system.getMasterSoundGroup(out soundGroup);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogAssertionFormat("FMOD error! {0} : {1}", result, FMOD.Error.String(result));
        }


        // test용이라 compile 에러나서 주석처리했습니다.
        // Load("LevelExampleSFX");
    }
    void OnDestroy()
    {
        foreach (var s in soundDict)
        {
            s.Value.release();
        }

        MasterChannelGroup.release();
        SFXChannelGroup.release();
        soundGroup.release();
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

        if (Input.GetKeyDown(KeyCode.U))
        {
            MuteAll();
        }
        
        if (Input.GetKeyDown(KeyCode.I))
        {
            UnMuteAll();
        }
        
        if (Input.GetKeyDown(KeyCode.O))
        {
            Pause("LaserSample1.wav");
        }
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            UnPause("LaserSample1.wav");
        }
        
        if (Input.GetKeyDown(KeyCode.L))
        {
            Load("LaserSample1.wav", true);
        }

    }

    /// <summary>
    /// 2D SFX사운드를 발생시킵니다.
    /// </summary>
    /// <param name="audioName">대상 audioFile 이름</param>
    /// <param name="playVolume">실행시킬 Volume값(0~1)</param>
    /// <param name="playPitch">실행시킬 Pitch값; Default : 1</param>
    /// <param name="playSpeed">실행시킬 Speed값; Default : 1</param>
    public void PlaySFX(string audioName, float playVolume = 1.0f, float playPitch = 1.0f, float playSpeed = 1.0f)
    {
        int channelIndex = 0;
        foreach (var c in channelArr)
        {
            bool b;
            c.isPlaying(out b);
            if (!b | channelIndex > 19)
            {
                break;
            }
            channelIndex++;
        }
        if (channelIndex == 20)
        {
            Debug.LogError("There is no extra SFX channel now.");
            return;
        }

        // 현재 하나의 FMOD.Sound파일이 서로다른 Channel에서 play될 경우 다시말해 FMOD.playSound함수의 parameter로 받아질경우
        // 기존 Channel과의 연결이 끊어져 하나의 Sound로 중복된 소리가 나지 않습니다.
        // SoundDict로 FMOD.Sound를 관리하면서 생긴 문제로 판명났습니다.
        // FMOD.MODE.CREATESAMPLE, FMOD.MODE.OPENMEMORY 사용 및 FMOD.CREATESOUNDINDEXINFO 설정을 통하여 
        // FMOD API 내부 구조체를 이용해야 합니다. 많이 변경해야합니다 ㅜㅜ
        Debug.Log("Channel Debug: " + channelIndex);

        if (!soundDict.ContainsKey(audioName))
        {
            Debug.LogError("Load the sound file before play.");
            return;
        } else
        {
            result = system.playSound(soundDict[audioName], SFXChannelGroup, true, out channelArr[channelIndex]);
            if (result != FMOD.RESULT.OK)
            {
                Debug.LogAssertionFormat("FMOD error! {0} : {1}", result, FMOD.Error.String(result));
            }

            result = channelArr[channelIndex].setVolume(playVolume);
            if (result != FMOD.RESULT.OK)
            {
                Debug.LogAssertionFormat("FMOD error! {0} : {1}", result, FMOD.Error.String(result));
            }

            result = channelArr[channelIndex].setPitch(playPitch);
            if (result != FMOD.RESULT.OK)
            {
                Debug.LogAssertionFormat("FMOD error! {0} : {1}", result, FMOD.Error.String(result));
            }
            
            // 아 이 코드 한줄 때문에 계속 실행이 안됬었습니다.!!!!!!!!!
            // channelArr[channelIndex].setFrequency(playSpeed/playPitch);

            result = channelArr[channelIndex].setPaused(false);
            if (result != FMOD.RESULT.OK)
            {
                Debug.LogAssertionFormat("FMOD error! {0} : {1}", result, FMOD.Error.String(result));
            }
        }
    }

    /// <summary>
    /// BGM을 변경합니다. BGM은 기본적으로 stream으로 읽어옵니다.
    /// </summary>
    /// <param name="audioName">변경될 BGM audioFile 이름</param>
    public void SetBGM(string audioName, float playVolume = 1.0f, float playPitch = 1.0f, float playSpeed = 1.0f)
    {
        Load(audioName, false);
        // Fade in Fade out 기능이 추가되어야 합니다.
        result = system.playSound(soundDict[audioName], MasterChannelGroup, true, out channelArr[maxChannelNum-1]);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogAssertionFormat("FMOD error! {0} : {1}", result, FMOD.Error.String(result));
        }

        if (!currentBGMName.Equals("null"))
        {
            UnLoad(currentBGMName);
        }

        currentBGMName = audioName;

        channelArr[maxChannelNum-1].setPaused(false);
    }

    /// <summary>
    /// 특정위치에서 3D sound를 발생시키고 싶을때 사용합니다.
    /// </summary>
    /// <param name="audioName">대상 audioFile 이름</param>
    /// <param name="position">SFX를 발생시킬 위치</param>
    public void PlayAt(string audioName, Vector3 position)
    {

    }


    /// <summary>
    /// 해당 소리를 일시정지 시킵니다.
    /// </summary>
    /// <param name="audioName">대상 audioFile 이름</param>
    public void Pause(string audioName)
    {
        result = FindChannelOfSound(audioName).setPaused(true);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogAssertionFormat("FMOD error! {0} : {1}", result, FMOD.Error.String(result));
        }
    }

    /// <summary>
    /// 일시정지된 소리를 다시 재생시킵니다.
    /// </summary>
    /// <param name="audioName">대상 audioFile 이름</param>
    public void UnPause(string audioName)
    {
        result = FindChannelOfSound(audioName).setPaused(false);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogAssertionFormat("FMOD error! {0} : {1}", result, FMOD.Error.String(result));
        }
    }

    /// <summary>
    /// 해당 소리를 음소거 시킵니다. 소리는 계속 재생됩니다.
    /// </summary>
    /// <param name="audioName">대상 audioFile 이름</param>
    public void Mute(string audioName)
    {
        result = FindChannelOfSound(audioName).setMute(true);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogAssertionFormat("FMOD error! {0} : {1}", result, FMOD.Error.String(result));
        }
    }

    /// <summary>
    /// 음소거된 소리의 음소거를 해제합니다.
    /// </summary>
    /// <param name="audioName">대상 audioFile 이름</param>
    public void UnMute(string audioName)
    {
        result = FindChannelOfSound(audioName).setMute(false);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogAssertionFormat("FMOD error! {0} : {1}", result, FMOD.Error.String(result));
        }
    }

    /// <summary>
    /// 모든 소리를 음소거시킵니다.
    /// </summary>
    public void MuteAll()
    {
        MasterChannelGroup.setMute(true);
    }

    /// <summary>
    /// 모든 소리를 음소거 해제시킵니다.
    /// </summary>
    public void UnMuteAll()
    {
        MasterChannelGroup.setMute(false);
    }

    /// <summary>
    /// 해당 소리의 재생여부에 대한 bool값을 return합니다.
    /// </summary>
    /// <param name="audioName">대상 audioFile 이름</param>
    /// <returns></returns>
    public bool isPlaying(string audioName)
    {
        bool isPlaying;
        result = FindChannelOfSound(audioName).isPlaying(out isPlaying);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogAssertionFormat("FMOD error! {0} : {1}", result, FMOD.Error.String(result));
        }
        return isPlaying;
    }

    /// <summary>
    /// 해당 소리의 볼륨을 조절합니다.
    /// </summary>
    /// <param name="audioName">대상 audioFile 이름</param>
    /// <param name="playVolume">원하는 Volume배율</param>
    public void SetVolume(string audioName, float playVolume)
    {
        result = FindChannelOfSound(audioName).setVolume(playVolume);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogAssertionFormat("FMOD error! {0} : {1}", result, FMOD.Error.String(result));
        }
    }

    /// <summary>
    /// 해당 소리의 높낮이를 조절합니다.
    /// </summary>
    /// <param name="audioName">대상 audioFile 이름</param>
    /// <param name="playPitch">원하는 Pitch</param>
    public void SetPitch(string audioName, float playPitch)
    {
        result = FindChannelOfSound(audioName).setPitch(playPitch);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogAssertionFormat("FMOD error! {0} : {1}", result, FMOD.Error.String(result));
        }
    }
    
    /// <summary>
    /// 해당 소리의 속도를 조절합니다.
    /// </summary>
    /// <param name="audioName">대상 audioFile 이름</param>
    /// <param name="playSpeed">상대 배율값입니다. Default : 1, Range[0.01~100]</param>
    public void SetSpeed(string audioName, float playSpeed)
    {
        // 지원하지 않는 음악 Format이라는 에러가 뜹니다. wav파일은 이 함수를 사용할수 없다고하네요.
        // wav파일은 note based가 아닌 sample based라서 랍니다.
        // 알아본 바로는 DSP를 이용해야한다고 합니다. 공부시작!
        // result = soundDict[audioName].setMusicSpeed(playSpeed);
        // if (result != FMOD.RESULT.OK)
        // {
        //     Debug.LogAssertionFormat("FMOD error! {0} : {1}", result, FMOD.Error.String(result));
        // }
    }

    /// <summary>
    /// 게임시작시에 미리 memory에 올려놓을 자주쓰고 크기가 작은 audioFile들을 불러옵니다.
    /// </summary>
    /// <param name="audioName">대상 audioFile 이름</param>
    /// <returns></returns>
    public void Load(string audioName, bool stayOnMemory)
    {
        // 예외처리를 어디까지 해줘야 할까요? 
        soundPathJson = JsonManager.Find(audioName);
        FMOD.Sound soundBuffer;
        FMOD.MODE mode;
        string modeString = soundPathJson["FMOD.MODE"].ToString();
        string[] modesString = modeString.Split(',');
            Debug.Log("Load debug1");
        
        // null | FMOD.MODE 연산이 안되서 먼저 하나 넣고 중첩합니다.
        mode = (FMOD.MODE) System.Enum.Parse(typeof(FMOD.MODE), modesString[0]);
        for (int i =1 ; i < modesString.Length; i++)
        {
            Debug.Log("Load debug2");
            mode = mode | (FMOD.MODE) System.Enum.Parse(typeof(FMOD.MODE), modesString[i]);
        }
        // 아직 폴더구조가 정리가 안된 상태에서의 soundPath입니다.
        if (!stayOnMemory)
        {
            Debug.Log("Load debug3");
            result = system.createStream(soundPath + soundPathJson["Path"].ToString(), mode, out soundBuffer);
            if (result != FMOD.RESULT.OK)
            {
                Debug.Log(audioName + "을(를) 불러오지 못했습니다.");
                Debug.LogAssertionFormat("FMOD error! {0} : {1}", result, FMOD.Error.String(result));
            }
            else
            {
                soundDict.Add(audioName, soundBuffer);
            }
        }
        else
        {
            Debug.Log("Load debug4");

            if ((mode & FMOD.MODE.OPENMEMORY) == FMOD.MODE.OPENMEMORY)
            {
                FMOD.CREATESOUNDEXINFO exinfo = new FMOD.CREATESOUNDEXINFO();
                exinfo.cbsize = Marshal.SizeOf(exinfo);
                exinfo.length = 797*1024;
                exinfo.format = FMOD.SOUND_FORMAT.PCM32;
                result = system.createSound(soundPath + soundPathJson["Path"].ToString(), mode, ref exinfo, out soundBuffer);
                if (result != FMOD.RESULT.OK)
                {
                    Debug.LogWarning(audioName + "을(를) 불러오지 못했습니다.");
                    Debug.LogAssertionFormat("FMOD error! {0} : {1}", result, FMOD.Error.String(result));
                }
                else
                {
                    soundDict.Add(audioName, soundBuffer);
                }
            } else
            {
                result = system.createSound(soundPath + soundPathJson["Path"].ToString(), mode, out soundBuffer);
                if (result != FMOD.RESULT.OK)
                {
                    Debug.LogWarning(audioName + "을(를) 불러오지 못했습니다.");
                    Debug.LogAssertionFormat("FMOD error! {0} : {1}", result, FMOD.Error.String(result));
                }
                else
                {
                    soundDict.Add(audioName, soundBuffer);
                }
            }
        }
    }

    /// <summary>
    /// Runtime시에 동적으로 audioFile을 memory에서 내리기 위한 함수입니다.
    /// </summary>
    /// <param name="audioName">대상 audioFile 이름</param>
    /// <returns></returns>
    public void UnLoad(string audioName)
    {
        if (soundDict.ContainsKey(audioName))
        {
            result = soundDict[audioName].release();
            if (result != FMOD.RESULT.OK)
            {
                Debug.LogAssertionFormat("FMOD error! {0} : {1}", result, FMOD.Error.String(result));
            } else
            {
                soundDict.Remove(audioName);
            }
        }
        else
        {
            Debug.LogAssertionFormat("FMOD error! {0} : {1}", result, FMOD.Error.String(FMOD.RESULT.FILE_NOT_IN));
        }
    }

    /// <summary>
    /// 해당 소리에 Loop option을 추가합니다.
    /// </summary>
    /// <param name="audioName">대상 audioFile 이름</param>
    public void LoopOn(string audioName)
    {
        result = soundDict[audioName].setMode(FMOD.MODE.LOOP_NORMAL);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogAssertionFormat("FMOD error! {0} : {1}", result, FMOD.Error.String(result));
        }
    }

    /// <summary>
    /// 해당 Sound를 실행하고 있는 Channel을 반환합니다.
    /// </summary>
    /// <param name="audioName">대상 audioFile 이름</param>
    /// <returns></returns>
    FMOD.Channel FindChannelOfSound(string audioName)
    {
        FMOD.Sound soundbuffer;
        foreach(var c in channelArr)
        {
            c.getCurrentSound(out soundbuffer);
            if (soundbuffer.Equals(soundDict[audioName]))
            {
                return c;
            }
        }

        Debug.Log("That Sound is not playing!\nReturned channel is NULL value");
        return new FMOD.Channel();
    }
}
