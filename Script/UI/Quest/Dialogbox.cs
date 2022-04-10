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

    #region 다이얼로그 UI 창에 정보 전달 받기 ( NPC정보와 퀘스트정보 가져오기 )
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
            _DescText.text = "진행 가능한 퀘스트가 없습니다";
            transform.Find("CloseBT").gameObject.SetActive(true);
            return;
        }

        if (_showQuest._IsQuestClear == true)
        {
            _DescText.text = "퀘스트를 완료 하였습니다. 보상을 받으세요!!";
            transform.Find("ClearBT").gameObject.SetActive(true); // 클리어 버튼 온
            return;
        }

        if (Trans._IsQuest == false)
            NextTo();
        else if (Trans._IsQuest == true)
            IsQuest();
    }
    #endregion

    #region 퀘스트 완료시 활성화 버튼
    public void ClearQuest() // 퀘스트 클리어
    {
        int space = Inventory._instance.InvenSlots.Length - Inventory._instance.InventorySpaceCheck();

        if (space >= _showQuest._RewardItem.Count)// 현재 인벤토리에 남은 칸이 보상 받을 아이템 갯수 보다 많은지 체크
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
            _DescText.text = "장비칸을 비우고 다시 시도해주세요";
            transform.Find("ClearBT").gameObject.SetActive(false);
            transform.Find("CloseBT").gameObject.SetActive(true); // 나가기 버튼
        }
    }
    #endregion

    #region 이미 퀘스트가 진행중이라면 띄워주기
    void IsQuest()
    {
        _DescText.text = "이미 퀘스트가 진행 중 입니다";
        transform.Find("CloseBT").gameObject.SetActive(true);
    }
    #endregion

    #region 퀘스트를 받지 않았으면 , 대화 인덱스 만큼 실행
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

    #region 확인 버튼 , 취소 버튼 활성화
    private void Button_CloseOrOk(bool ok, bool next)
    {
        transform.Find("Button_ok").gameObject.SetActive(ok);
        transform.Find("Button_next").gameObject.SetActive(next);
    }
    #endregion

    #region 대화 창 글자 한글자씩 띄워주기
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

    #region 퀘스트 수락시 , 퀘스트창에 퀘스트 박스 생성하고, 퀘스트중이라고 bool 값으로 전달
    public void Quest_Accept() // 퀘스트 수락시 초기화
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

    #region 퀘스트 거절시, 다시 퀘스트 받을 수 있도록 초기화
    public void Quest_Close() // 퀘스트 거절시 인덱스 초기화
    {
        _textCount = 0;
        QuestDialogReset(false);
        Button_CloseOrOk(false, true);
    }
    #endregion

    #region 초기화 함수 ( 플레이어 상태, 다이얼로그 UI 창 끄기, 현재 퀘스트 null 로 동기화)
    public void QuestDialogReset(bool show)
    {
        GameManager.Instance._player._state = State.Idle;
        GameManager.Instance._player._stateMachine.RemoveSelect();
        _showQuest = null;
        gameObject.SetActive(show);
    }
    #endregion
}
