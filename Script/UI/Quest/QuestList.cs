using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//
//      ����Ʈ ���� ��ũ��Ʈ
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


    // ����Ʈ �ε��� ��������
    public QuestInfo QuestIndex(int index)
    {
        foreach (QuestInfo item in _questLists)
        {
            if(item._Index == index)
            {
                return item;
            }
        }
        Debug.Log("�ش� �ϴ� ����Ʈ�� �����ϴ�");
        return null;
    }

}


