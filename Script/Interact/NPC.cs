using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPC : Interactable
{
    public bool _IsQuest;

    [SerializeField] public int _Questindex;
    [SerializeField] QuestInfo _MyQuest;

    public override void Interact()
    {
        Debug.Log("NPC " + _Name);
        if (_MyQuest != null)
        {
            GameManager.Instance._player._state = State.Quest;
            DialogBoxOn();
        }

    }
    protected override void Start()
    {
        base.Start();
        QuestInfoGetIntoMyQuest();
        NpcStatusUpdate();

    }

    private void DialogBoxOn()
    {
        UIManger.Instance._DialogUI.GetComponent<Dialogbox>().DialogBoxSend(_MyQuest, this);
    }

    public void QuestClear_ResetMyInfo()
    {
        _IsQuest = false;
        _MyQuest = null;
    }


    public void QuestInfoGetIntoMyQuest()
    {
        foreach (QuestInfo info in GameManager.Instance._questLists._questLists)
        {
            if (info._Index == _Questindex)
                _MyQuest = info;
        }
    }


    protected override void InteractDistance()
    {
        float distance = Vector3.Distance(GameManager.Instance._player.transform.position, transform.position);
        if (_isSelect && !_AbleToInteracted)
        {

            if (distance <= _Interact_radius)
            {
                Animator anim = GetComponent<Animator>();
                if (anim != null)
                {
                    anim.SetTrigger("Talk");
                    _camPlayer.maxDistance = 2f;
                    _camPlayer.RotX = 10;
                }
                Interact();
                _AbleToInteracted = true;
                transform.LookAt(_player);
            }
        }

        if (distance <= _UI_CanvasSight && _NamePopUp.gameObject.activeSelf == false)
            _NamePopUp.gameObject.SetActive(true);
        else if (distance > _UI_CanvasSight && _NamePopUp.gameObject.activeSelf == true)
            _NamePopUp.gameObject.SetActive(false);

    }

    public void NpcStatusUpdate()
    {
        if (_Questindex == -1)
            return;
        _MyImage.enabled = true;


        if (_MyQuest != null && _IsQuest == false) // 퀘스트가 있으며, 아직 퀘스트를 받지 않은 상태
        {
            _MyImage.sprite = UIManger.Instance._NpcStatusSprite[1]._MyImage; // 느낌표
            _MyImage.rectTransform.sizeDelta = new Vector2(30, 60);
            _MyImage.color = Color.green;
        }
        else if (_MyQuest != null && _IsQuest == true && _MyQuest._IsQuestClear == false) // 퀘스트가 있으며, 퀘스트를 받은 상태
        {
            _MyImage.sprite = UIManger.Instance._NpcStatusSprite[0]._MyImage; // 기본
            _MyImage.rectTransform.sizeDelta = new Vector2(60, 60);
            _MyImage.color = Color.white;
        }
        else if (_MyQuest != null && _MyQuest._IsQuestClear == true)   // 퀘스트를 완료한 상태
        {
            _MyImage.sprite = UIManger.Instance._NpcStatusSprite[2]._MyImage; // 물음표
            _MyImage.rectTransform.sizeDelta = new Vector2(40, 60);
            _MyImage.color = Color.yellow;
        }
        else // 아무것도 없는 상태는 꺼주기
            _MyImage.enabled = false;
    }
}
