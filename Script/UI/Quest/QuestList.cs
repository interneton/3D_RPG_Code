using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class QuestInfo
{
    public int _Index;
    public string _TextName;
    public List<string> _desc;
    public string _OneLineDesc;
    public int _countValue;
    public int _clearValue1;
    public List<Item> _RewardItem;
    public MonsterName _MonsterType;
    public bool _IsQuestClear;

}


public class QuestList : MonoBehaviour
{
    public List<QuestInfo> _questLists;
    public List<QuestBox> _curQuestlists = new List<QuestBox>();


    public QuestInfo QuestIndex(int index)
    {
        foreach (QuestInfo item in _questLists)
        {
            if(item._Index == index)
            {
                return item;
            }
        }
        Debug.Log("해당 하는 퀘스트가 없습니다");
        return null;
    }

}


