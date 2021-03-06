using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerStateMachine : MonoBehaviour
{

    [Header("공개")]
    public Interactable _select;
    public MonsterController _enemySelect;


    [Header("비공개")]
    [SerializeField] float _minDistance = 1f;
    [SerializeField] float _AttackToMonsterLerpSpeed = 0.02f;

    private Camera _cam;
    private Player _player;
    private bool _IsDistance = false;
    private bool _IsLookRotate = false;
    private float _MonsterToPlayerDistance = 5.0f;
    public CharacterInputLogic _characterLogic
    { get; set; }
    

    void Start()
    {
        _cam = Camera.main;
        _player = GetComponent<Player>();
        _characterLogic = GetComponent<CharacterInputLogic>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && _select == null) // 오른쪽
        {
            RaycastHit hit;
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {   //플레이어가 이동 // 레이캐스트가 맞는 콜라이더
                Interactable interactable = hit.collider.GetComponent<Interactable>();

                if (interactable != null)
                {
                    setSelect(interactable);

                    if (interactable is ItemPickUp)
                        return;
                    _IsLookRotate = true;
                }
            }
        }
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        #region 방향체크해서 바라보기
        if (_IsLookRotate == true)
        {
            if (_select != null)
                Facetarget(_select.transform);

            if (_enemySelect != null)
                Facetarget(_enemySelect.transform);
        }
        #endregion

        if ((_characterLogic.Speed != 0) && _player._state != State.Attack) //캐릭터가 움직일 경우 실행
        {
            RemoveSelect();
            RemoveEnemySelect();
        }

        MonsterFind();

        if (_enemySelect != null)
        {
            Facetarget(_enemySelect.transform, true);

            if (_IsDistance == true)
                MoveLerpToMonster();
        }
    }

    #region  몬스터 관련
    // 몬스터 인지 체크 후, 몬스터한테 근접하기
    void MonsterFind()
    {
        if (Input.GetMouseButtonDown(0) && _player._curWeaponType != WeaponType.Normal) // 왼쪽마우스
        {
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {   //플레이어가 이동 // 레이캐스트가 맞는 콜라이더

                if (hit.collider.CompareTag("EnemyHitBox"))
                {
                    MonsterController monsterobj = hit.collider.transform.parent.GetComponent<MonsterController>();

                    if (monsterobj != null && monsterobj._beHaviour_State != BehaviourTree.DEATH)
                    {
                        float _dis = Vector3.Distance(monsterobj.transform.position, transform.position);
                        if (_dis <= _MonsterToPlayerDistance)
                        {
                                _IsDistance = true;
                                _IsLookRotate = true;
                                _enemySelect = monsterobj;
                        }
                    }
                    else if(_enemySelect == null && monsterobj == null)
                    {
                        RemoveEnemySelect();
                    }
                }
            }
        }
    }

    // 몬스터한테 달라 붙기
    void MoveLerpToMonster()
    {
        float _dis = Vector3.Distance(_enemySelect.transform.position, transform.position);

        if (_dis <= _MonsterToPlayerDistance && _dis >= _minDistance)
            transform.position = Vector3.Lerp(transform.position, _enemySelect.transform.position, _AttackToMonsterLerpSpeed);
        else
            _IsDistance = false;
    }

    // 적 선택 해제
    void RemoveEnemySelect()
    {
        _enemySelect = null;
        _IsDistance = false;
        _IsLookRotate = false;
    }

    // 몬스터 바라보기
    void Facetarget(Transform dirTrans, bool playing = false)
    {
        Vector3 direction = (dirTrans.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        if (transform.rotation == lookRotation)
        {
            _IsLookRotate = false;
            return;
        }
    }
    #endregion

    // 무기 상태 애니메이션으로 전환
    public WeaponType WeaponStateTrue(WeaponType newState)
    {
        if (_player._curWeaponType != newState)
        {
            _player._curWeaponType = newState;

            if (_player._curWeaponType == WeaponType.OneHand)
                _characterLogic.Anim.SetBool("OneHand", true);

            if (_player._curWeaponType == WeaponType.TwoHand)
                _characterLogic.Anim.SetBool("TwoHand", true);
            return _player._curWeaponType;
        }

        return _player._curWeaponType;
    }

    // 무기 상태 애니메이션 끄고, 일반 상태로 전환
    public void WeaponStateFalse(WeaponType nowState)
    {
        if (nowState != WeaponType.Normal)
        {
            if (nowState == WeaponType.OneHand)
            {
                _characterLogic.Anim.SetBool("OneHand", false);

            }
            else if (nowState == WeaponType.TwoHand)
            {
                _characterLogic.Anim.SetBool("TwoHand", false);
            }

        }
        _player._curWeaponType = WeaponType.Normal;
    }

    // 현재 선택한 인터랙터블 객체 받아오기
    void setSelect(Interactable newSelect)
    {
        if (newSelect != _select)
        {
            if (_select != null)
                _select.NullSelectTrans();

            _select = newSelect;

        }
        newSelect.SelectTrans(transform);
    }

    // 선택한 인터랙터블 객체 삭제
    public void RemoveSelect()
    {
        if (_select != null)
            _select.NullSelectTrans();

        _select = null;
    }


    // 플레이어 Idle, Move, Attack, Hit , SKill , Quest 상태 변경
    public void Player_StateChange(State newState)
    {
        if (_player._state != newState)
        {
            _player._state = newState;

            if (_player._state == State.Idle || _player._state == State.Hit)
                _characterLogic.VelocityZeroSetting();
        }
    }

    // 플레이어 무기 타입 변경
    public void Player_WeaponTypeChange(WeaponType newWeapontype)
    {
        if (_player._curWeaponType != newWeapontype)
            _player._curWeaponType = newWeapontype;
    }

    // 애니메이션 이벤트에서 실행
    public void StateIdleSet()
    {
        Player_StateChange(State.Idle);
    }

}
