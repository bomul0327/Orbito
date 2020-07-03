using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 발사대 건조 기능이 구현된 클래스.
/// </summary>
public class Launchpad
{   
    /// <summary>
    /// 제조에 필요한 아이템.
    /// </summary>
    public List<Item> RequiredItemList;

    /// <summary>
    /// 제조에 필요한 자원.
    /// </summary>
    public int RequiredResource;

    /// <summary>
    /// 현재 사용중인 Spec 이름.
    /// </summary>
    public string Name
    {
        get;
        private set;
    }

    /// <summary>
    /// 발사대가 이미 건조 되어 활성화 되어 있는가?
    /// </summary>
    private bool isBuildComplete = false;

    public Launchpad(string levelName)
    {
        //Spec을 JSON에서 가져옵니다.
        LaunchpadSpec spec= JsonManager.GetLaunchpadSpec(levelName);
        
        Name = spec.Name;
        RequiredItemList = spec.Items;
        RequiredResource = spec.Resource;

    }


    /// <summary>
    /// LaunchPad 건설 요구사항을 만족하는 지 확인합니다.
    /// </summary>
    /// <param name="baseInventory">base의 인벤토리.</param>
    public bool CanBuild(Inventory baseInventory)
    {
        return !isBuildComplete && baseInventory.HasSufficientItems(RequiredItemList);
    }

    /// <summary>
    /// LaunchPad 건설 요구사항을 만족하는 지 확인하고, 만족한다면 건설 이벤트를 발행합니다.
    /// </summary>
    /// <param name="baseInventory"></param>
    /// <returns>True, if satified.</returns>
    public bool TryBuild(Inventory baseInventory)
    {
        //이미 발사대가 제조 완료 되었거나, 재료가 부족한지 확인합니다.

        if (isBuildComplete)
        {
            Debug.LogWarning($"LaunchpadBuildError::Launchpad '{Name}' is already built.");
            return false;
        }

        if (!baseInventory.TryUseItems(RequiredItemList))
        {
            Debug.LogWarning($"LaunchpadBuildError::Not enough items to build launchpad '{Name}'.");
            return false;
        }

        //제조 조건을 만족했다면, 발사대 건조를 수행.
        Build();

        return true;
    }

    /// <summary>
    /// 발사대를 건조하고 활성화합니다.
    /// </summary>
    private void Build()
    {
        //발사대 건조 상태를 업데이트합니다.
        isBuildComplete = true;

        //Build 성공 이벤트를 전송합니다.
        OrbitoEvent.EventDispatcher.Notify(
        new LaunchpadBuildEvent()
        {
            launchpad = this
        });

        Debug.Log($"LaunchpadBuildSuccess::Build launchpad '{Name}'.");
    }

    

}
