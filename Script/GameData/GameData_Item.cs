using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ItemInfo
{
    public string Name;
    public int index;
    public int hpHealing;
    public int mpHealing;
    public int damage;
    public int movespeed;
    public int attackspeed;
    public int defend;

}

[CreateAssetMenu(fileName = "GameData_Item", menuName = "Gamedata/GameData_Item", order = 1)]
public class GameData_Item: GameData
{

    public List<ItemInfo> DataList;

    public ItemInfo GetData(int index)
    {
        ItemInfo info = null;

        foreach (ItemInfo b in DataList)
        {
            if (b.index == index)
            {
                info = b;

                break;
            }
        }
        return info;
    }


#if UNITY_EDITOR

    public override void parse(System.Object[] objList)
    {
        DataList = new List<ItemInfo>();

        foreach (System.Object csvObj in objList)
        {
            // C# Object¸¦ SurvivorItemInfo °´Ã¼·Î º¯È¯
            ItemInfo info = new ItemInfo();

            ParseObject(info, csvObj);

            DataList.Add(info);

        }

    }
#endif





}

