using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MonsterInfo
{
    public string Name;
    public int index;
    public int level;
    public int maxHp;
    public int maxMp;
    public int damage1;
    public int damage2;
    public int defend;
    public int attackSpeed;
    public int attackRange;

}

[CreateAssetMenu(fileName = "GameData_Monster", menuName = "Gamedata/GameData_Monster", order = 1)]
public class GameData_Monster : GameData
{

    public List<MonsterInfo> DataList;

    public MonsterInfo GetData(int index)
    {
        MonsterInfo info = null;

        foreach (MonsterInfo b in DataList)
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
        DataList = new List<MonsterInfo>();

        foreach (System.Object csvObj in objList)
        {
            // C# Object¸¦ SurvivorItemInfo °´Ã¼·Î º¯È¯
            MonsterInfo info = new MonsterInfo();

            ParseObject(info, csvObj);

            DataList.Add(info);

        }

    }
#endif





}

