using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이거 using 안하고 쓸수 있도록? 가능한건감
using Newtonsoft.Json.Linq;

using FMOD;
using Debug = UnityEngine.Debug;

// 현 진행상황 : 기초적인 부분은 되어갑니다.
// wav 및 모든 음원형식의 파일의 speed를 조절하기 위해
// MODE.CREATESAMPLE 및 DSP 공부중입니다.
// memory 관리는 계속 조금씩 늘어가는 중입니다. 최적화는 FMOD.CREATESOUNDEXINFO 를 공부해야합니다.
// unity editor에서 play exit을 반복할 시 memory가 0.03GB만큼 늘어가는 현상이 있습니다.
// Release되지 않은 메모리가 있는것으로 보입니다.

/// <summary>
/// 전체적인 audioFile들을 관리하기 위한 Singleton Class입니다.
/// </summary>
public class SoundManager : Singleton<SoundManager>
{
    const int maxChannelNum = 21, BattleSoundChannelIndex = 0, EventSoundChannelIndex = 10,
             EnvironmentSoundChannelIndex = 15, BGMChannelIndex = 20;

    string soundPath = Application.streamingAssetsPath + "/";
    FMOD.System system;
    static RESULT result; DSP_TYPE typeTemp; Sound soundTemp; string strTemp;
    ChannelGroup MasterChannelGroup, SFXChannelGroup, BattleSoundChannelGroup, EventSoundChannelGroup, 
            EnvironmentSoundChannelGroup, BGMChannelGroup;
    Channel[] channelArr;
    DSP DSPTemp, PitchshiftDSP;
    List<DSPConnection> DSPConnectionsList;
    Dictionary<string, Sound> soundDict;
    string currentBGMName = "null";
    JObject soundPathJson;
    void Start()
    {
        soundDict = new Dictionary<string, Sound>();
        channelArr = new Channel[maxChannelNum];
        DSPConnectionsList = new List<DSPConnection>();

        //@ System 초기설정 part입니다.

        result = Factory.System_Create(out system);
        Error(result); 
        
        // 현재 MaxChannel 개수는 임의로 넣었습니다. 아직 도입 과정이라 channel이 몇개 필요할지 모르겠네요.
        // 나머지는 기본값입니다.
        result = system.init(maxChannelNum, INITFLAGS.VOL0_BECOMES_VIRTUAL | INITFLAGS.PROFILE_METER_ALL, System.IntPtr.Zero);
        Error(result); 

        ADVANCEDSETTINGS advancedSetting = new ADVANCEDSETTINGS();
        advancedSetting.profilePort = 9264;

        result = system.setAdvancedSettings(ref advancedSetting);

        //@ Channel 초기설정 part입니다.
        result = system.getMasterChannelGroup(out MasterChannelGroup);
        Error(result); 

        result = system.createChannelGroup("SFX", out SFXChannelGroup);
        Error(result); 

        result = system.createChannelGroup("BattleSound", out BattleSoundChannelGroup);
        Error(result); 

        result = system.createChannelGroup("EventSound", out EventSoundChannelGroup);
        Error(result); 

        result = system.createChannelGroup("EnvironmentSound", out EnvironmentSoundChannelGroup);
        Error(result); 

        result = system.createChannelGroup("BGM", out BGMChannelGroup);
        Error(result); 

        result = MasterChannelGroup.addGroup(SFXChannelGroup);
        Error(result); 

        result = MasterChannelGroup.addGroup(BGMChannelGroup);
        Error(result); 

        result = SFXChannelGroup.addGroup(BattleSoundChannelGroup);
        Error(result); 

        result = SFXChannelGroup.addGroup(EventSoundChannelGroup);
        Error(result); 

        result = SFXChannelGroup.addGroup(EnvironmentSoundChannelGroup);
        Error(result); 

        for (int i = 0 ; i < maxChannelNum; i++)
        {
            result = system.getChannel(i, out channelArr[i]);
            Error(result); 
        }

        //@ DSP 초기설정 part입니다.
        result = system.createDSPByType(DSP_TYPE.PITCHSHIFT, out PitchshiftDSP);
        Error(result); 

        result = MasterChannelGroup.addDSP(CHANNELCONTROL_DSP_INDEX.HEAD, PitchshiftDSP);
        Error(result); 

        //@ Sound 초기설정 part입니다.

    }
    void OnDestroy()
    {
        foreach (var s in soundDict)
        {
            result = s.Value.release();
            Error(result); 
        }

        result = MasterChannelGroup.release();
        Error(result); 
        result = SFXChannelGroup.release();
        Error(result); 
        result = BattleSoundChannelGroup.release();
        Error(result); 
        result = EventSoundChannelGroup.release();
        Error(result); 
        result = EnvironmentSoundChannelGroup.release();
        Error(result); 
        result = system.close();
        Error(result); 
        result = system.release();
        Error(result); 
    }

    /// <summary>
    /// Test용 Update함수입니다. L을 누르고 SpaceBar를 누르면 example 2D SFX 사운드를 play합니다.
    /// 완성본에서는 지워질 함수입니다.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlaySFX("LaserSample1.wav", SFXEnum.BattleSound);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlaySFX("LaserSample1.wav", SFXEnum.EventSound);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            PlaySFX("LaserSample1.wav", SFXEnum.EnvironmentSound);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            SetBGM("BGMSample.wav");
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            LoopOn("LaserSample1.wav");
        }
        
        if (Input.GetKeyDown(KeyCode.I))
        {
            LoopOff("LaserSample1.wav");
        }
        
        if (Input.GetKeyDown(KeyCode.L))
        {
            Load("LaserSample1.wav", true);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            SetPitch("LaserSample1.wav", 2f);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            SetSpeed("LaserSample1.wav", 30000);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            SetSpeed("LaserSample1.wav", 40000);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            SetSpeed("LaserSample1.wav", 88200);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            foreach (var c in FindChannelOfSound("LaserSample1.wav"))
            {
                c.getFrequency(out float frequency);
                Debug.Log(frequency);
            }
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            PrintInfo();
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            system.playSound(soundDict["LaserSample1.wav"], MasterChannelGroup, true, out channelArr[0]);
            channelArr[0].setPaused(false);
        }
    }

    /// <summary>
    /// 2D SFX사운드를 발생시킵니다.
    /// </summary>
    /// <param name="audioName">대상 audioFile 이름</param>
    /// <param name="playVolume">실행시킬 Volume값(0~1)</param>
    /// <param name="playPitch">실행시킬 Pitch값; Default : 1</param>
    /// <param name="playSpeed">실행시킬 Speed값; Default : 1</param>
    public void PlaySFX(string audioName, SFXEnum SFXEnum = SFXEnum.SFX, float playVolume = 1.0f, float playPitch = 1.0f, float playSpeed = 1.0f)
    {
        int channelIndex;
        
        if (!soundDict.ContainsKey(audioName))
        {
            Debug.LogError("Load the sound file before play.");
            return;
        } else 
        {
            for (channelIndex =0 ; channelIndex < maxChannelNum ; channelIndex++)
            {
                channelArr[channelIndex].isPlaying(out bool b);
                if (!b) break;
            }
        }

        if (channelIndex == maxChannelNum)
        {
            Debug.LogError("There is no extra SFX channel now.");
            return;
        } else
        {
            switch (SFXEnum)
            {
                case SFXEnum.BattleSound :
                    Debug.Log("ChannelIndex = "+channelIndex);
                    result = system.playSound(soundDict[audioName], BattleSoundChannelGroup, true, out channelArr[channelIndex]);
                    Error(result); 
                    break;
                case SFXEnum.EventSound :
                    Debug.Log("ChannelIndex = "+channelIndex);
                    result = system.playSound(soundDict[audioName], EventSoundChannelGroup, true, out channelArr[channelIndex]);
                    Error(result); 
                    break;
                case SFXEnum.EnvironmentSound :
                    Debug.Log("ChannelIndex = "+channelIndex);
                    result = system.playSound(soundDict[audioName], EnvironmentSoundChannelGroup, true, out channelArr[channelIndex]);
                    Error(result); 
                    break;
                case SFXEnum.SFX :
                    Debug.Log("ChannelIndex = "+channelIndex);
                    result = system.playSound(soundDict[audioName], SFXChannelGroup, true, out channelArr[channelIndex]);
                    Error(result); 
                    break;
                    
            }
        
            SetVolume(audioName, playVolume);
            // SetPitch(audioName, playPitch);
            // SetSpeed(audioName, playSpeed);
            
            result = channelArr[channelIndex].setPaused(false);
            Error(result); 
        }
    }

    /// <summary>
    /// BGM을 변경합니다. BGM은 기본적으로 stream으로 읽어옵니다.
    /// </summary>
    /// <param name="audioName">변경될 BGM audioFile 이름</param>
    public void SetBGM(string audioName, float playVolume = 1.0f, float playPitch = 1.0f, float playSpeed = 1.0f)
    {
        Load(audioName, false);
        // Todo : Fade in Fade out 기능이 추가되어야 합니다.
        result = system.playSound(soundDict[audioName], BGMChannelGroup, true, out channelArr[BGMChannelIndex]);
        Error(result); 

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
        // Todo? 필요할지 모르겠네요.
    }


    /// <summary>
    /// 해당 소리를 일시정지 시킵니다.
    /// </summary>
    /// <param name="audioName">대상 audioFile 이름</param>
    public void Pause(string audioName)
    {
        foreach (var c in FindChannelOfSound(audioName))
        {
            result = c.setPaused(true);
            Error(result); 
        }
    }

    /// <summary>
    /// 일시정지된 소리를 다시 재생시킵니다.
    /// </summary>
    /// <param name="audioName">대상 audioFile 이름</param>
    public void UnPause(string audioName)
    {
        foreach (var c in FindChannelOfSound(audioName))
        {
            result = c.setPaused(false);
            Error(result); 
        }
    }

    /// <summary>
    /// 해당 소리를 음소거 시킵니다. 소리는 계속 재생됩니다.
    /// </summary>
    /// <param name="audioName">대상 audioFile 이름</param>
    public void Mute(string audioName)
    {
        foreach (var c in FindChannelOfSound(audioName))
        {
            result = c.setMute(true);
            Error(result); 
        }
    }

    /// <summary>
    /// 음소거된 소리의 음소거를 해제합니다.
    /// </summary>
    /// <param name="audioName">대상 audioFile 이름</param>
    public void UnMute(string audioName)
    {
        foreach (var c in FindChannelOfSound(audioName))
        {
            result = c.setMute(false);
            Error(result); 
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
        foreach (var c in FindChannelOfSound(audioName))
        {
            result = c.isPlaying(out isPlaying);
            Error(result); 
            if (isPlaying) return isPlaying;
        }
        return false;
    }

    /// <summary>
    /// 해당 소리의 볼륨을 조절합니다.
    /// </summary>
    /// <param name="audioName">대상 audioFile 이름</param>
    /// <param name="playVolume">원하는 Volume배율</param>
    public void SetVolume(string audioName, float playVolume)
    {
        foreach (var c in FindChannelOfSound(audioName))
        {
            result = c.setVolume(playVolume);
            Error(result); 
        }
    }

    /// <summary>
    /// 해당 소리의 높낮이를 조절합니다.
    /// </summary>
    /// <param name="audioName">대상 audioFile 이름</param>
    /// <param name="playPitch">원하는 Pitch in Range[0.5~2]</param>
    public void SetPitch(string audioName, float playPitch)
    {
        Debug.Log("SetPitch debug M");
        // foreach (var c in FindChannelOfSound(audioName))
        // {
            // result = c.addDSP(0, PitchshiftDSP);
            // Error(result); 
            // result = c.getDSPIndex(PitchshiftDSP, out int DSPIndex);
            // Error(result); 
            // result = c.getDSP(DSPIndex, out DSPTemp);
            // Error(result); 
            // result = DSPTemp.getNumParameters(out int numdsp);
            // Error(result); 
            // result = DSPTemp.getInfo(out string name, out uint version, out int chan, out int cofi, out int cofig);
            // Error(result); 


            // Debug.Log("Before set the parameter value");
            // Debug.Log("The index of pitchshift is "+ DSPIndex);
            // Debug.Log("number of param = " + numdsp);
            // Debug.Log(name + "/" + version + "/" + chan + "/" + cofi + "/" + cofig);
            // for (int j = 0 ; j < numdsp ; j++)
            // {
            //     result = DSPTemp.getParameterFloat(j, out float value);
            //     Error(result); 
            //     Debug.Log(j+"th parameter is "+value);
            // }

            // result = DSPTemp.setParameterFloat(0, playPitch);
            // Error(result); 

            // Debug.Log("After set the parameter value");
            // for (int j = 0 ; j < numdsp ; j++)
            // {
            //     DSPTemp.getParameterFloat(j, out float value);
            //     Debug.Log(j+"th parameter is "+value);
            // }

        
        // }
        result = MasterChannelGroup.getDSPIndex(PitchshiftDSP, out int dspIndex);
        Error(result); 
        result = MasterChannelGroup.getDSP(dspIndex, out DSPTemp);
        Error(result); 
        result = DSPTemp.setParameterFloat(0, playPitch);
        Error(result); 

    }
    
    /// <summary>
    /// 해당 소리의 속도를 조절합니다.
    /// </summary>
    /// <param name="audioName">대상 audioFile 이름</param>
    /// <param name="playSpeed">상대 배율값입니다. Default : 1, Range[0.01~100]</param>
    public void SetSpeed(string audioName, float playSpeed)
    {
        foreach (var c in FindChannelOfSound(audioName))
        {
            // SetPitch(audioName, 44100/playSpeed);
            c.setFrequency(playSpeed);
        }
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
        MODE mode;
        string modeString = soundPathJson["MODE"].ToString();
        string[] modesString = modeString.Split(',');
        
        // null | MODE 연산이 안되서 먼저 하나 넣고 중첩합니다.
        mode = (MODE) System.Enum.Parse(typeof(MODE), modesString[0]);
        for (int i =1 ; i < modesString.Length; i++)
        {
            mode = mode | (MODE) System.Enum.Parse(typeof(MODE), modesString[i]);
        }
        // 아직 폴더구조가 정리가 안된 상태에서의 soundPath입니다.
        if (!stayOnMemory)
        {
            result = system.createStream(soundPath + soundPathJson["Path"].ToString(), mode, out soundTemp);
            if (Error(result))
            {
                Debug.Log(audioName + "을(를) 불러오지 못했습니다.");
            }
            else
            {
                soundDict.Add(audioName, soundTemp);
            }
        }
        else
        {
            if ((mode & MODE.OPENMEMORY) == MODE.OPENMEMORY)
            {
                // 하나도 모르겠습니다 여긴 ㄹㅇ
                // using (System.IO.StreamReader sr = System.IO.File.OpenText((string)soundPathJson["Path"]))
                // {
                //     System.IO.BinaryReader br = new System.IO.BinaryReader(sr.Read()));
                //     byte[] soundData = System.IO
                // }
                // CREATESOUNDEXINFO exinfo = new CREATESOUNDEXINFO();
                // exinfo.cbsize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(CREATESOUNDEXINFO));
                // exinfo.length = 797*1024;
                // exinfo.suggestedsoundtype = SOUND_TYPE.WAV;
                // result = system.createSound(soundPath + soundPathJson["Path"].ToString(), mode, ref exinfo, out soundTemp);
                // Error(result);
                // {
                //     Debug.LogWarning(audioName + "을(를) 불러오지 못했습니다.");
                // }
                // else
                // {
                //     soundDict.Add(audioName, soundTemp);
                // }
            } else
            {
                result = system.createSound(soundPath + soundPathJson["Path"].ToString(), mode, out soundTemp);
                if (Error(result))
                {
                    Debug.LogWarning(audioName + "을(를) 불러오지 못했습니다.");
                }
                else
                {
                    soundDict.Add(audioName, soundTemp);
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
            if(Error(result))
            {
                Debug.LogWarning(audioName + "을(를) 정상적으로 UnLoad하지 못했습니다.");
            } else
            {
                soundDict.Remove(audioName);
            }
        }
        else
        { Error(RESULT.FILE_NOT_IN); }
    }

    /// <summary>
    /// 해당 소리에 Loop option을 추가합니다.
    /// </summary>
    /// <param name="audioName">대상 audioFile 이름</param>
    public void LoopOn(string audioName)
    {
        result = soundDict[audioName].setMode(MODE.LOOP_NORMAL);
        Error(result);
    }

    /// <summary>
    /// 해당 소리에 Loop option을 제거합니다.
    /// </summary>
    /// <param name="audioName">대상 audioFile 이름</param>
    public void LoopOff(string audioName)
    {
        result = soundDict[audioName].setMode(MODE.LOOP_OFF);
        Error(result); 
    }

    /// <summary>
    /// 해당 Sound를 실행하고 있는 Channel들을 담은 List를 반환합니다.
    /// </summary>
    /// <param name="audioName">대상 audioFile 이름</param>
    /// <returns></returns>
    List<Channel> FindChannelOfSound(string audioName)
    {
        List<Channel> channelList = new List<Channel>();
        foreach(var c in channelArr)
        {
            c.getCurrentSound(out soundTemp);
            if (soundTemp.Equals(soundDict[audioName]))
            {
                channelList.Add(c);
            }
        }

        if (channelList.Count == 0)
            Debug.LogWarning("That Sound is not playing!\nReturned channel is NULL value");
        return channelList;
    }

    public void PrintInfo()
    {
        ChannelGroup CGTemp;
        // Debug.Log("The number of channel is " + maxChannelNum);
        // Debug.Log("The BattleSound channels are from " + BattleSoundChannelIndex + " to " + (EventSoundChannelIndex-1) );
        // Debug.Log("The EventSound channels are from " + EventSoundChannelIndex + " to " + (EnvironmentSoundChannelIndex-1) );
        // Debug.Log("The EnvironmentSound channels are from " + EnvironmentSoundChannelIndex + " to " + (BGMChannelIndex-1) );
        // Debug.Log("The BGM channels is " + BGMChannelIndex);
        Debug.Log("------------------------------------");
        Debug.Log("Currently these sounds are in memory");
        foreach (var s in soundDict)
        {
            Debug.Log(s.Key);
            s.Value.getMode(out MODE mode);
            Debug.Log(s.Key+"'s modes are "+mode);
        }
        Debug.Log("------------------------------------");
        Debug.Log("Currently playing channels are");
        int i = 0;
        foreach (var c in channelArr)
        {
            bool b;
            c.getIndex(out i);
            c.isPlaying(out b);
            if (b)
            {
                c.getCurrentSound(out soundTemp);
                c.getNumDSPs(out int numdsp);
                soundTemp.getName(out strTemp, 30);
                Debug.Log("On "+i+" channel the "+strTemp+" is playing. And number of DSP is "+numdsp);
                c.getChannelGroup(out CGTemp);
                CGTemp.getName(out strTemp, 30);
                Debug.Log("ChannelGroup which this channel belongs to is "+strTemp);
                Debug.Log("--------List of DSPs in this channel");
                for (int j = 0 ; j < numdsp ; j++)
                {
                    c.getDSP(j, out DSPTemp);
                    DSPTemp.getType(out typeTemp);
                    Debug.Log(j+"th DSP's type is "+typeTemp);
                }
            }
        }
        // Debug.Log("------------------------------------");
        // Debug.Log("Currently structed channelGroups are");
        // MasterChannelGroup.getName(out strTemp, 30);
        // Debug.Log(strTemp);        
        // SFXChannelGroup.getName(out strTemp, 30);
        // Debug.Log(strTemp);        
        // BGMChannelGroup.getName(out strTemp, 30);
        // Debug.Log(strTemp);        
        // BattleSoundChannelGroup.getName(out strTemp, 30);
        // Debug.Log(strTemp);        
        // EventSoundChannelGroup.getName(out strTemp, 30);
        // Debug.Log(strTemp);        
        // EnvironmentSoundChannelGroup.getName(out strTemp, 30);
        // Debug.Log(strTemp);
        // BattleSoundChannelGroup.getParentGroup(out CGTemp);
        // CGTemp.getName(out strTemp, 30);        
        // Debug.Log("BattleChannelGroup's parrent ChannelGroup is "+strTemp);
        // EventSoundChannelGroup.getParentGroup(out CGTemp);
        // CGTemp.getName(out strTemp, 30);        
        // Debug.Log("EventChannelGroup's parrent ChannelGroup is "+strTemp);
        // EnvironmentSoundChannelGroup.getParentGroup(out CGTemp);
        // CGTemp.getName(out strTemp, 30);        
        // Debug.Log("EnvironmentChannelGroup's parrent ChannelGroup is "+strTemp);
        // SFXChannelGroup.getParentGroup(out CGTemp);
        // CGTemp.getName(out strTemp, 30);        
        // Debug.Log("SFXChannelGroup's parrent ChannelGroup is "+strTemp);
        // BGMChannelGroup.getParentGroup(out CGTemp);
        // CGTemp.getName(out strTemp, 30);        
        // Debug.Log("BGMChannelGroup's parrent ChannelGroup is "+strTemp);
    }

    public enum SFXEnum
    {
        BattleSound, EventSound, EnvironmentSound, SFX
    }

    /// <summary>
    /// 에러가 있으면 true를 반환, 에러가 없으면 false를 반환
    /// </summary>
    private bool Error(RESULT result)
    {
        if (result != RESULT.OK) { Debug.LogAssertionFormat("FMOD error! {0} : {1}", result, FMOD.Error.String(result)); return true;}
        else { return false; }
    }
}
