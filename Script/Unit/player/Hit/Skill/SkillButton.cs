using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillButton : MonoBehaviour, IDropHandler
{

    [SerializeField] KeyCode _Skillkey;

    [SerializeField]
    int _ButtonIndex = 0; // ��ư ����

    Button _myBT;
    Slider _fillImage;
    SkillInfo _MySkill;

    private void Start()
    {
        _fillImage = transform.Find("Slider").GetComponent<Slider>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(_Skillkey))
            InputSKill();
    }

    // ��ų ��ư�� ��ų ���� �޾ƿ���
    void SetInfo(SkillInfo skillinfo)
    {
        _MySkill = skillinfo;

        Image iconImage = transform.Find("On").GetComponent<Image>();
        iconImage.sprite = _MySkill._myImg.sprite;
        iconImage.enabled = true;
        ButtonDelegate();
    }

    // ��ų ��ư â �ʱ�ȭ
    public void SetInfoNull()
    {
        if (_MySkill != null)
        {
            _myBT = null;
            _MySkill = null;

            Image iconImage = transform.Find("On").GetComponent<Image>();
            iconImage.enabled = false;
        }
    }

    // ��ų ��ư ������ ����
    public void ButtonDelegate()
    {
        _myBT = GetComponent<Button>();
        _myBT.onClick.AddListener(() =>
        {
            InputSKill();
        });
    }

    // ��ư ȿ��
    void InputSKill() // ��ų �Է�
    {
        if (_MySkill != null && _MySkill._IsCoolTime == false
            && GameManager.Instance._player._curWeaponType != WeaponType.Normal
            && GameManager.Instance._player._state != State.Hit)
        {
            _MySkill._IsCoolTime = true; // ��Ÿ������ ����

            GameManager.Instance._player.transform.GetComponent<CharacterInputLogic>().Skill(_MySkill._SkillName); // ��ų �ִϸ��̼�
            GameManager.Instance._player._stateMachine.Player_StateChange(State.Skill); // ���� ��ȯ

            SkillManager.Instance.SkillLists(_MySkill._SkillName); // ��ų �ߵ�
            StartCoroutine("_CoolTimer");
        }
    }

    // ��ų ��Ÿ�� ����ϱ�
    IEnumerator _CoolTimer()
    {
        float Timer = _MySkill._SkillCoolTimer;

        while (true)
        {
            Timer -= Time.deltaTime;
            _fillImage.value = Timer / _MySkill._SkillCoolTimer;

            if (Timer < 0)
            {
                _MySkill._IsCoolTime = false;
                break;
            }

            yield return null;
        }
    }

    // ��ų ��ư�� ��ӽ� ����
    public void OnDrop(PointerEventData eventData)
    {
        if (_MySkill != null)
        {
            if (_MySkill._IsCoolTime == true)
                return;
        }

        Debug.Log("���");

        SkillManager.Instance.skillSlotAdd(_ButtonIndex);
        SetInfo(SkillManager.Instance._curDragObj);
        SkillManager.Instance._curDragObj = null;
    }
}
