using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    public GameObject _QuestBoxPrefab;
    public Transform _QuestBox_Parents;

    public Text _Desc_text;
    public Text _Npc_Name_text;
    public Text _Mission_Count;

    [SerializeField] InvenSlot[] _rewardSlots;



    // 퀘스트 받았을 경우 퀘스트 오브젝트 생성해서 UI에 표시
    public GameObject NewInstance()
    {
        if (_QuestBox_Parents == null || _QuestBoxPrefab == null)
            return null;

        GameObject obj = Instantiate(_QuestBoxPrefab, _QuestBox_Parents);
        return obj;
    }

    // 퀘스트 수행 정보 출력하기
    public void Quest_ShowInfo(QuestInfo info, NPC npc, bool Isupdate = false)
    {
        _Npc_Name_text.text = npc._Name;
        _Desc_text.text = info._OneLineDesc;

        if (info._MonsterType != MonsterName.NULL)

            _Mission_Count.text = string.Format("{0} 사냥  {1} / {2}", GameManager.Instance._Data_Monster.GetData((int)(info._MonsterType)).Name, info._countValue, info._clearValue1);

        if (Isupdate == false)
        {

            for (int i = 0; i < info._RewardItem.Count; i++)
            {
                _rewardSlots[i].MyInfoSet(info._RewardItem[i]);
                _rewardSlots[i].transform.parent.gameObject.SetActive(true);
            }
        }
    }

    // 퀘스트 몬스터와 같은지 체크해서 퀘스트 수행 현황에 더해주기
    public void QuestUpdate(MonsterName monsterType)
    {
        QuestBox[] Allquests = _QuestBox_Parents.GetComponentsInChildren<QuestBox>();

        foreach(QuestBox item in Allquests)
        {
            if(item._myInfo._MonsterType == monsterType)
            {
                item.QuestValueUpdate();
                return;
            }
        }
        Debug.Log("몬스터 사냥 적용");
    }


    // 퀘스트 UI 정보 표시창 리셋하기
    public void QuestUIReset()
    {
        _Npc_Name_text.text = "";
        _Desc_text.text = "";
        _Mission_Count.text = "";

        foreach (InvenSlot item in _rewardSlots)
            if (item.transform.parent.gameObject.activeSelf) item.transform.parent.gameObject.SetActive(false);
    }
}
