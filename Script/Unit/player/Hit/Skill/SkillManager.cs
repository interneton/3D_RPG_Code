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

    public List<SkillEffect> _skilleffect; // �÷��̾� �ֺ��� ����Ʈ

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

    public void skillSlotAdd(int index) // ��ų ���Կ� ��� �Ǿ� �ִ��� üũ
    {
        if (_curDragObj == null)
            return;

        if (_InSlot.ContainsKey(_curDragObj._SkillName)) // ��� �Ǿ� ������ �������ְ�
        {
            int slotIndex = _InSlot[_curDragObj._SkillName]; // �������������

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
                    Debug.Log(" ���̵� ���");
                }
                break;
            case "Fire":
                {
                    GetSkill(skillName);
                    Debug.Log(" ���̾� ���");
                }
                break;
            case "Lightning":
                {
                    GetSkill(skillName);
                    Debug.Log(" ����Ʈ�� ���");
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
