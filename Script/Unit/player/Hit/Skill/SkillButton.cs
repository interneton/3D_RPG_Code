using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillButton : MonoBehaviour, IDropHandler
{

    [SerializeField] KeyCode _Skillkey;

    [SerializeField]
    int _ButtonIndex = 0; // 버튼 순서

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

    // 스킬 버튼에 스킬 정보 받아오기
    void SetInfo(SkillInfo skillinfo)
    {
        _MySkill = skillinfo;

        Image iconImage = transform.Find("On").GetComponent<Image>();
        iconImage.sprite = _MySkill._myImg.sprite;
        iconImage.enabled = true;
        ButtonDelegate();
    }

    // 스킬 버튼 창 초기화
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

    // 스킬 버튼 누르면 실행
    public void ButtonDelegate()
    {
        _myBT = GetComponent<Button>();
        _myBT.onClick.AddListener(() =>
        {
            InputSKill();
        });
    }

    // 버튼 효과
    void InputSKill() // 스킬 입력
    {
        if (_MySkill != null && _MySkill._IsCoolTime == false
            && GameManager.Instance._player._curWeaponType != WeaponType.Normal
            && GameManager.Instance._player._state != State.Hit)
        {
            _MySkill._IsCoolTime = true; // 쿨타임으로 변경

            GameManager.Instance._player.transform.GetComponent<CharacterInputLogic>().Skill(_MySkill._SkillName); // 스킬 애니메이션
            GameManager.Instance._player._stateMachine.Player_StateChange(State.Skill); // 상태 변환

            SkillManager.Instance.SkillLists(_MySkill._SkillName); // 스킬 발동
            StartCoroutine("_CoolTimer");
        }
    }

    // 스킬 쿨타임 계산하기
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

    // 스킬 버튼에 드롭시 실행
    public void OnDrop(PointerEventData eventData)
    {
        if (_MySkill != null)
        {
            if (_MySkill._IsCoolTime == true)
                return;
        }

        Debug.Log("등록");

        SkillManager.Instance.skillSlotAdd(_ButtonIndex);
        SetInfo(SkillManager.Instance._curDragObj);
        SkillManager.Instance._curDragObj = null;
    }
}
