using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogbox : MonoBehaviour
{

    int _textCount;
    float _TextSpeed = 0.05f;

    NPC _npc;
    QuestInfo _showQuest;

    public Text _Name;
    public Text _DescText;

    #region ���̾�α� UI â�� ���� ���� �ޱ� ( NPC������ ����Ʈ���� �������� )
    public void DialogBoxSend(QuestInfo quest, NPC Trans)
    {
        _npc = Trans;
        _showQuest = quest;
        _Name.text = _npc._Name;
        gameObject.SetActive(true);
        transform.Find("CloseBT").gameObject.SetActive(false);
        transform.Find("ClearBT").gameObject.SetActive(false);


        if (Trans._Questindex == -1)
        {
            Button_CloseOrOk(false, false);
            _DescText.text = "���� ������ ����Ʈ�� �����ϴ�";
            transform.Find("CloseBT").gameObject.SetActive(true);
            return;
        }

        if (_showQuest._IsQuestClear == true)
        {
            _DescText.text = "����Ʈ�� �Ϸ� �Ͽ����ϴ�. ������ ��������!!";
            transform.Find("ClearBT").gameObject.SetActive(true); // Ŭ���� ��ư ��
            return;
        }

        if (Trans._IsQuest == false)
            NextTo();
        else if (Trans._IsQuest == true)
            IsQuest();
    }
    #endregion

    #region ����Ʈ �Ϸ�� Ȱ��ȭ ��ư
    public void ClearQuest() // ����Ʈ Ŭ����
    {
        int space = Inventory._instance.InvenSlots.Length - Inventory._instance.InventorySpaceCheck();

        if (space >= _showQuest._RewardItem.Count)// ���� �κ��丮�� ���� ĭ�� ���� ���� ������ ���� ���� ������ üũ
        {
            for (int i = 0; i < _showQuest._RewardItem.Count; i++)
                Inventory._instance.ItemInstance(_showQuest._RewardItem[i]);

            _npc.QuestClear_ResetMyInfo();
            _npc.NpcStatusUpdate();
            _npc._Questindex = -1;

            foreach (QuestBox quest in GameManager.Instance._questLists._curQuestlists)
            {
                if (quest._myInfo._Index == _showQuest._Index)
                {
                    GameManager.Instance._questLists._curQuestlists.Remove(quest);

                    UIManger.Instance._QuestUI.GetComponent<QuestUI>().QuestUIReset();
                    Destroy(quest.gameObject);
                    QuestDialogReset(false);
                    return;
                }
            }
        }
        else
        {
            _DescText.text = "���ĭ�� ���� �ٽ� �õ����ּ���";
            transform.Find("ClearBT").gameObject.SetActive(false);
            transform.Find("CloseBT").gameObject.SetActive(true); // ������ ��ư
        }
    }
    #endregion

    #region �̹� ����Ʈ�� �������̶�� ����ֱ�
    void IsQuest()
    {
        _DescText.text = "�̹� ����Ʈ�� ���� �� �Դϴ�";
        transform.Find("CloseBT").gameObject.SetActive(true);
    }
    #endregion

    #region ����Ʈ�� ���� �ʾ����� , ��ȭ �ε��� ��ŭ ����
    public void NextTo()
    {
        if (_showQuest == null)
            return;

        if (_textCount < _showQuest._desc.Count)
        {
            Button_CloseOrOk(false, true);
            StopCoroutine("Text_Typing");
            StartCoroutine("Text_Typing");
            _textCount++;

            if (_textCount == _showQuest._desc.Count)
                Button_CloseOrOk(true, false);
        }
    }
    #endregion

    #region Ȯ�� ��ư , ��� ��ư Ȱ��ȭ
    private void Button_CloseOrOk(bool ok, bool next)
    {
        transform.Find("Button_ok").gameObject.SetActive(ok);
        transform.Find("Button_next").gameObject.SetActive(next);
    }
    #endregion

    #region ��ȭ â ���� �ѱ��ھ� ����ֱ�
    IEnumerator Text_Typing()
    {
        int i = 0;
        _DescText.text = "";
        char[] write = _showQuest._desc[_textCount].ToCharArray();

        while (i < write.Length)
        {
            _DescText.text += write[i];
            i++;
            yield return new WaitForSeconds(_TextSpeed);
        }
    }
    #endregion

    #region ����Ʈ ������ , ����Ʈâ�� ����Ʈ �ڽ� �����ϰ�, ����Ʈ���̶�� bool ������ ����
    public void Quest_Accept() // ����Ʈ ������ �ʱ�ȭ
    {
        GameObject newObj = UIManger.Instance._QuestUI.GetComponent<QuestUI>().NewInstance();
        QuestBox newQuest = newObj.GetComponentInChildren<QuestBox>();
        newQuest.SetInfo(_showQuest, _npc);
        GameManager.Instance._questLists._curQuestlists.Add(newQuest);

        _npc._IsQuest = true;
        _npc.NpcStatusUpdate();
        QuestDialogReset(false);
        Button_CloseOrOk(false, false);
    }
    #endregion

    #region ����Ʈ ������, �ٽ� ����Ʈ ���� �� �ֵ��� �ʱ�ȭ
    public void Quest_Close() // ����Ʈ ������ �ε��� �ʱ�ȭ
    {
        _textCount = 0;
        QuestDialogReset(false);
        Button_CloseOrOk(false, true);
    }
    #endregion

    #region �ʱ�ȭ �Լ� ( �÷��̾� ����, ���̾�α� UI â ����, ���� ����Ʈ null �� ����ȭ)
    public void QuestDialogReset(bool show)
    {
        GameManager.Instance._player._state = State.Idle;
        GameManager.Instance._player._stateMachine.RemoveSelect();
        _showQuest = null;
        gameObject.SetActive(show);
    }
    #endregion
}
