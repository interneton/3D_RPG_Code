using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

public enum State
{ Invaild = -1, Idle, Move, Attack, Hit, Death , Skill, Quest, End}
public enum WeaponType 
{ Invaild = -1, Normal, OneHand, TwoHand, OnehandBig, TwoHandBig, End }

[Serializable]
public class PlayerStats
{
    [Header("플레이어 스텟")]
    public string ID;
    public int maxHp;
    public int curHp;
    public int curExp;
    public int maxExp;
    public int level;
    public int ATK;
    public int DEF;
    public float CRI;

}


public class Player : MonoBehaviour
{
    public State _state;
    public WeaponType _curWeaponType;

    public PlayerStats _myStats;
    public PlayerStateMachine _stateMachine;

    public Transform _LeftHand;
    public Transform _RightHand;

    public Collider _WeaponCol;
    [SerializeField] 
    ParticleSystem[] _SlashParticle;
    PersonalCanvas _personalCanvas;

    private void Start()
    {
        _stateMachine = GetComponent<PlayerStateMachine>();
        _personalCanvas = transform.Find("UI Canvas").GetComponent<PersonalCanvas>();


        // 기본 스텟 증정하기.
        BaseStats();
        _myStats.curHp = _myStats.maxHp;

        GameManager.Instance._EquipStats.StatsInfo_AllUpdate();
    }

    void BaseStats()
    {
        _myStats.ATK = 0;
        _myStats.DEF = 2;
        _myStats.maxHp = 500;
        _myStats.level = 1;
        _myStats.CRI = 0;
    }

    public void OnDamage(int damage, float dmgRate)
    {
        int _hp = _myStats.curHp;
        _hp -= damage;

        OnDmgText(damage, dmgRate);
        if (_hp > 0)
        {
            transform.GetComponent<Animator>().Play("Hit");
        }
        else if (_hp <= 0)
        {
            _stateMachine.Player_StateChange(State.Hit);
            _myStats.curHp = 0;

        }

        _myStats.curHp = _hp;    // 몬스터에 체력 대입하기

        UIManger.Instance._EquipSettings.transform.Find("MoveWindow").Find("Stats").GetComponent<StatsUI>().HpSliderUpdate(_myStats.curHp, _myStats.maxHp);

    }

    int TextCount = 0;
    private void OnDmgText(int dmg, float dmgRate)
    {
        if (TextCount == 2)
            TextCount = 0;

        TextMeshProUGUI textObj = _personalCanvas._list_dmgText[TextCount].GetComponent<TextMeshProUGUI>();
        textObj.text = dmg.ToString();
        textObj.fontSize = 50 * dmgRate;
        textObj.gameObject.SetActive(true);
        TextCount++;
    }
    public void ParticleChange()
    {
        if (_SlashParticle != null)
            _SlashParticle = null;

        if (_curWeaponType == WeaponType.OneHand)
            _SlashParticle = transform.Find("OneHand").GetComponentsInChildren<ParticleSystem>();
        if (_curWeaponType == WeaponType.TwoHand)
            _SlashParticle = transform.Find("TwoHand").GetComponentsInChildren<ParticleSystem>();
    }


    public void StatsUpdate(Item info, bool IsAdd)
    {
        Debug.Log("업데이트 완료");
        if (IsAdd)
        {
            _myStats.ATK += System.Convert.ToInt32(info._Damage);
            _myStats.DEF += System.Convert.ToInt32(info._Defend);
            _myStats.maxHp += System.Convert.ToInt32(info._Hp);
            float.TryParse(info._Critical, out float result);
            _myStats.CRI += result;
        }
        else if (!IsAdd)
        {
            _myStats.ATK -= System.Convert.ToInt32(info._Damage);
            _myStats.DEF -= System.Convert.ToInt32(info._Defend);
            _myStats.maxHp -= System.Convert.ToInt32(info._Hp);
            float.TryParse(info._Critical, out float result);
            _myStats.CRI -= result;
            if (_myStats.CRI <= 0)
                _myStats.CRI = 0;

        }
    }



    public void AttackColDelay(int show)    // 어택 박스 활성화 == > 1 활성 0 비활성
    {
        if (_WeaponCol != null)
        {
            _WeaponCol.enabled = System.Convert.ToBoolean(show);
        }
    }

    public void SlashParticlePlay(int number) // 해당 배열 인덱스 파티클 플레이
    {
        _SlashParticle[number].Play();
    }

}
