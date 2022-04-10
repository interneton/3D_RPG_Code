using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//
//      퀘스트 관리 스크립트
//

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


    // 퀘스트 인덱스 가져오기
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


