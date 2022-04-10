using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class EquipInfo
{
    public EquipInfo DeepCopy()
    {
        EquipInfo deepCopyClass = new EquipInfo();
        deepCopyClass.Name = this.Name;
        deepCopyClass.index = this.index;
        deepCopyClass.level = this.level;
        deepCopyClass.damage = this.damage;
        deepCopyClass.defend = this.defend;
        deepCopyClass.critical = this.critical;
        deepCopyClass.hp = this.hp;
        deepCopyClass.mp = this.mp;
        deepCopyClass.type = this.type;

        return deepCopyClass;
    }
    public string Name;
    public int index;
    public int level;
    public int damage;
    public int defend;
    public string critical;
    public int hp;
    public int mp;
    public string type;

}

[CreateAssetMenu(fileName = "GameData_Equip", menuName = "Gamedata/GameData_Equip", order = 1)]
public class GameData_Equip : GameData
{

    public List<EquipInfo> DataList;

    public EquipInfo GetData(string Name)
    {
        EquipInfo info = null;

        foreach (EquipInfo b in DataList)
        {
            if (b.Name == Name)
            {
                info = b;

                break;
            }
        }
        return info.DeepCopy();
    }


#if UNITY_EDITOR

    public override void parse(System.Object[] objList)
    {
        DataList = new List<EquipInfo>();

        foreach (System.Object csvObj in objList)
        {
            // C# Object¸¦ SurvivorItemInfo °´Ã¼·Î º¯È¯
            EquipInfo info = new EquipInfo();

            ParseObject(info, csvObj);

            DataList.Add(info);

        }

    }
#endif





}

