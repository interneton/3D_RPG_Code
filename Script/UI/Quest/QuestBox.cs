using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestBox : MonoBehaviour
{
    NPC _questOwner; // 퀘스트 주인
    public QuestInfo _myInfo;

    public void QuestValueUpdate()
    {
        if (_myInfo != null)
        {
            if (_myInfo._countValue < _myInfo._clearValue1)
            {
                _myInfo._countValue++;
                UIManger.Instance._QuestUI.GetComponent<QuestUI>().Quest_ShowInfo(_myInfo, _questOwner, true); // 퀘스트 UI 업데이트
                if (_myInfo._countValue >= _myInfo._clearValue1) // 현재 진행도가 최대 진행도와 같거나 크면
                {
                    _myInfo._IsQuestClear = true;
                    _questOwner.NpcStatusUpdate();
                }
            }
        }
    }

    public void SetInfo(QuestInfo info, NPC owner)
    {
        _myInfo = info;
        _questOwner = owner;
        transform.Find("Text").GetComponent<Text>().text = info._TextName;
    }

    public void ClickToInfo()
    {
        if (_myInfo == null)
            return;

        UIManger.Instance._QuestUI.GetComponent<QuestUI>().Quest_ShowInfo(_myInfo, _questOwner);
    }
}
