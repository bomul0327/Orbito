using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OrbitoEvent;

/// <summary>
/// Base 기능을 테스트하기 위한 코드.
/// </summary>
public class TestBase : MonoBehaviour, IEventListener
{
    //재료가 충분한 상황에서 Launchpad를 건설하는 Base.
     public Base SufficientBase
    {
        get;
        private set;
    }
    //재료가 부족한 상황에서 Launchpad를 건설하는 Base.
    public Base NotSufficientBase
    {
        get;
        private set;
    }


    public void Awake()
    {
        SufficientBase = new Base();
        
        NotSufficientBase = new Base();
        NotSufficientBase.Launchpad = new Launchpad("Level1");
    }

    private void OnEnable()
    {
        EventDispatcher.AddListener(this);
    }

    private void OnDisable()
    {
        EventDispatcher.RemoveListener(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            //첫 번째 제조 시도 시, 건설 성공 로그 출력 및 LaunchpadBuildEvent 수신.
            //두 번째 제조 시도 시, 건설 실패 로그 출력(이미 건설됨).
            SufficientBase.BuildLaunchpad();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            //재료 부족으로 항상 실패.
            NotSufficientBase.BuildLaunchpad();
        }
    }

    /// <summary>
    /// Launchpad 건설 성공 이벤트를 수신합니다.
    /// </summary>
    /// <param name="e"></param>
    public void OnEvent(IEvent e)
    {
        if (e is LaunchpadBuildEvent)
        {
            var evt = (LaunchpadBuildEvent)e;
            Debug.Log($"Recieved LaunchpadBuildEvent with {evt.launchpad.Name}" );
        }
    }
}
