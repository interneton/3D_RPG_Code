using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestBox : MonoBehaviour
{
    NPC _questOwner; // ����Ʈ ����
    public QuestInfo _myInfo;

    // ����Ʈ ���� ������Ʈ �����ֱ�
    public void QuestValueUpdate()
    {
        if (_myInfo != null)
        {
            if (_myInfo._countValue < _myInfo._clearValue1)
            {
                _myInfo._countValue++;
                UIManger.Instance._QuestUI.GetComponent<QuestUI>().Quest_ShowInfo(_myInfo, _questOwner, true); // ����Ʈ UI ������Ʈ
                if (_myInfo._countValue >= _myInfo._clearValue1) // ���� ���൵�� �ִ� ���൵�� ���ų� ũ��
                {
                    _myInfo._IsQuestClear = true;
                    _questOwner.NpcStatusUpdate();
                }
            }
        }
    }


    // ����Ʈ �ڽ� ���� �־��ֱ�
    public void SetInfo(QuestInfo info, NPC owner)
    {
        _myInfo = info;
        _questOwner = owner;
        transform.Find("Text").GetComponent<Text>().text = info._TextName;
    }

    // ��ư ȿ�� => ������Ʈ ���ý� ���� ǥ���ϱ�
    public void ClickToInfo()
    {
        if (_myInfo == null)
            return;

        UIManger.Instance._QuestUI.GetComponent<QuestUI>().Quest_ShowInfo(_myInfo, _questOwner);
    }
}
