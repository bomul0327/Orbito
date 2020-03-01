using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 기지
/// </summary>
public class Base
{
    //강화시 필요 자원 레시피
    public static Dictionary<string, Item[]> ReinforceRecipe = new Dictionary<string, Item[]>();

    //청사진 연구 레시피(Blueprint class를 만든다면 Dictionary<Blueprint, Item[]>이 더 적절할듯)
    public static Dictionary<Item, Item[]> BlueprintRecipe = new Dictionary<Item, Item[]>();

    //거점의 창고
    //public Inventory BaseInventory;

    //정박중인 정찰선
    public Character player = null;
    
    /// <summary>
    /// 정찰선을 기지에 정박시킵니다
    /// </summary>
    /// <param name="player">플레이어</param>
    public void Anchor(Character player)
    {
        this.player = player;
    }

    /// <summary>
    /// 수집품 확보 작업(정박 중인 정찰선이 채집한 아이템을 거점의 창고로 이전)
    /// </summary>
    public void SecureItems()
    {
        if(player == null)
        {
            return;
        }

        //for(int i = 0; i < player.inventory.count; ++i)
        //{
        //     플레이어 인벤토리에서 거점의 창고로 아이템 이전
        //}
    }

    /// <summary>
    /// 정찰선 수리
    /// </summary>
    public void RepairPlayer()
    {
        if(player == null)
        {
            return;
        }

        int repairNeeded = player.MaxHP - player.CurrentHP;

        // 필요한 자원량 계산
        //int itemNeeded = Mathf.Round(repairNeeded / repairAmount + 0.5f);

        //Item repairItem = BaseInventory.getItem("repairItem");
        //if(repairItem == null)
        //{
        //    return;
        //}

        // 자원이 충분하면 수리 후 자원 감소
        //if(repairItem.amount >= itemNeeded)
        //{
        //    player.CurrentHP = player.MaxHP;
        //    repairItem.amount -= itemNeeded;
        //}

        // 자원량이 불충분할 경우 부분수리할지 아니면 수리가 아예 안될지는 추후 추가

    }

    /// <summary>
    /// 정찰선 강화
    /// </summary>
    /// <param name="target">강화 대상</param>
    public void Reinforce(string target)
    {
        if(player == null)
        {
            return;
        }

        Item[] neededItems;
        if(ReinforceRecipe.TryGetValue(target, out neededItems))
        {
            //현재 강화레벨에 따른 필요 자원량 조정 수치(필요없다면 삭제)
            //float adjustValue = 계산...

            Item resource;
            for(int i = 0; i < neededItems.Length; ++i)
            {
                //기지 창고에서 해당 자원 찾기
                //resource = BaseInventory.getItem(neededItems[i]);

                //기지 창고에서 해당 자원량 충분한지 확인
                //if(resource == null || resource.amount < (int)(neededItems[i].amount * adjustValue))
                //{
                //    //아이템이 충분치 않다는 메세지
                //    return;
                //}
            }
            for(int i = 0; i < neededItems.Length; ++i)
            {
                //기지 창고에서 해당 자원 찾기
                //resource = BaseInventory.getItem(neededItems[i]);

                //자원 소모
                //resource.amount -= (int)(neededItems[i].amount * adjustValue);

                //강화 작업
            }
        }
        else
        {
            Debug.Log("Can't reinforce " + target);
        }
    }

    /// <summary>
    /// 정찰선에 장비 장착
    /// </summary>
    /// <param name="equipment">장비 아이템</param>
    public void Equip(Item equipment)
    {
        if(player == null)
        {
            return;
        }

        //장비인지 확인 후 정찰선에 장착
    }

    /// <summary>
    /// 청사진을 사용한 연구
    /// </summary>
    /// <param name="blueprint">사용할 청사진</param>
    public void Research(Item blueprint)
    {
        //청사진인지 확인(Blueprint class를 파라미터로 받는다면 삭제할 부분)

        //이미 연구된 청사진인지 확인

        Item[] neededItems;
        if (BlueprintRecipe.TryGetValue(blueprint, out neededItems))
        {
            Item resource;
            for (int i = 0; i < neededItems.Length; ++i)
            {
                //기지 창고에서 해당 자원 찾기
                //resource = BaseInventory.getItem(neededItems[i]);

                //기지 창고에서 해당 자원량 충분한지 확인
                //if(resource == null || resource.amount < neededItems[i].amount)
                //{
                //    //아이템이 충분치 않다는 메세지
                //    return;
                //}
            }
            for (int i = 0; i < neededItems.Length; ++i)
            {
                //기지 창고에서 해당 자원 찾기
                //resource = BaseInventory.getItem(neededItems[i]);

                //자원 소모
                //resource.amount -= neededItems[i].amount * adjustValue;

                //연구 완료
            }
        }
        else
        {
            Debug.Log("Can't research " + blueprint);
        }
    }

    /// <summary>
    /// 거점 이동
    /// </summary>
    /// <param name="targetPlanet">목표 천체</param>
    public void MoveBase(Planet targetPlanet)
    {

    }

    /// <summary>
    /// 발사대 건조
    /// </summary>
    public void BuildLauncher()
    {

    }
}
