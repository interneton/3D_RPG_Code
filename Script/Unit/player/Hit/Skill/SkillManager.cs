using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[Serializable]
public class SkillEffect
{
    public string _effectName;
    public ParticleSystem _effect;
}

public class SkillManager : MonoBehaviour
{
    public SkillInfo _curDragObj;
    public static SkillManager Instance;
    public Transform _SkillPoolingObjParent;
    public Dictionary<string, int> _InSlot = new Dictionary<string, int>();

    public List<SkillEffect> _skilleffect; // 플레이어 주변에 이펙트

    void Awake()
    {
        Instance = this;
    }


    public void curDragObj(SkillInfo info)
    {
        if (_curDragObj != null)
            _curDragObj = null;

        _curDragObj = info;
    }

    public void skillSlotAdd(int index) // 스킬 슬롯에 등록 되어 있는지 체크
    {
        if (_curDragObj == null)
            return;

        if (_InSlot.ContainsKey(_curDragObj._SkillName)) // 등록 되어 있으면 제거해주고
        {
            int slotIndex = _InSlot[_curDragObj._SkillName]; // 밸류값가져오기

            UIManger.Instance._SkillUI.transform.GetChild(slotIndex).GetComponent<SkillButton>().SetInfoNull();
            InSlotRemove(slotIndex);
        }

            UIManger.Instance._SkillUI.transform.GetChild(index).GetComponent<SkillButton>().SetInfoNull();
            InSlotRemove(index);

        _InSlot.Add(_curDragObj._SkillName, index);
    }

    void InSlotRemove(int index)
    {
        var key = _InSlot.FirstOrDefault(x => x.Value == index).Key;

        if (key != null)
            _InSlot.Remove(key);
    }



    public void SkillLists(string skillName)
    {
        switch (skillName)
        {
            case "Blade":
                {
                    GetSkill(skillName);
                    Debug.Log(" 블레이드 사용");
                }
                break;
            case "Fire":
                {
                    GetSkill(skillName);
                    Debug.Log(" 파이어 사용");
                }
                break;
            case "Lightning":
                {
                    GetSkill(skillName);
                    Debug.Log(" 라이트닝 사용");
                }
                break;
        }
    }

    void GetSkill(string skill)
    {
        Transform trans = _SkillPoolingObjParent.Find(skill);

        if (trans != null)
            trans.GetComponent<SkillPlaying>().Playing();
    }
}
