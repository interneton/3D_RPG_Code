using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;



public class CharacterInputLogic: MonoBehaviour
{
    Player _player;
    Camera _camera;
    Animator _anim;
    Rigidbody _rigid;
    public Animator Anim { get => _anim; }

    RaycastHit _hit;
    Vector3 _moveDirection = Vector3.zero;

    const float _locoAnimationsmoothTime = .1f;

    [SerializeField] float _speed = 8f;
    [SerializeField] float _speedpercent = 0;
    [SerializeField] float _Rotationsmoothness = 10f;

    public float Speed { get => _speed; }

    bool _Run;
    bool _isBorder;

    [Header("콤보 관련")]
    [SerializeField] int _ComboCount = 0;
    [SerializeField] float _LastTime = 0f;
    [SerializeField] float _MaxDelaytimer = 0.9f;


    void Start()
    {
        _camera = Camera.main;
        _anim = GetComponent<Animator>();
        _player = GetComponent<Player>();
        _rigid = GetComponent<Rigidbody>();
    }


    void Update()
    {
        Move();
        StopToWall();

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        AttackInput();
    }
    #region 공격 입력하면 현재 상태 체크해서 실행
    void AttackInput()
    {
        if (_player._curWeaponType != WeaponType.Normal && _player._state != State.Quest)
        {
            if (Time.time - _LastTime > _MaxDelaytimer)
                _ComboCount = 0;

            if (Input.GetMouseButtonDown(0))
            {
                _speedpercent = 0;
                _LastTime = Time.time;
                _ComboCount++;

                if(_player._state != State.Attack)
                Attack(0);
            }
            _ComboCount = Mathf.Clamp(_ComboCount, 0, 4);
        }
    }
    #endregion

    #region 이동
    void Move()
    {
        if (_player._state  != State.Attack && _player._state != State.Quest && _player._state != State.Skill &&
            _player._state != State.Hit)
        {
            float h = Input.GetAxisRaw("Vertical");
            float v = Input.GetAxisRaw("Horizontal");

            _Run = new Vector3(h, 0, v).magnitude != 0;

            if (_Run)
            {
                if (_speed == 0)
                    _speed = 8;


                _player._stateMachine.Player_StateChange(State.Move);


                Vector3 forward = new Vector3(_camera.transform.forward.x, 0f, _camera.transform.forward.z).normalized;
                Vector3 right = new Vector3(_camera.transform.right.x, 0f, _camera.transform.right.z).normalized;
                _moveDirection = (forward * h) + (right * v);

                if (!_isBorder)
                {
                    transform.position += (_moveDirection.normalized * _speed * Time.deltaTime);

                    if (_speedpercent <= 1)
                    {
                        _speedpercent += Time.deltaTime + _locoAnimationsmoothTime;
                        _speedpercent = Mathf.Clamp(_speedpercent, 0.0f, 1f);
                    }
                }
                else if (_isBorder)
                    _speedpercent = 0;

                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_moveDirection), _Rotationsmoothness * Time.deltaTime);
            }
            else
                _player._stateMachine.Player_StateChange(State.Idle);
        }

        _anim.SetFloat("speedPercent", _speedpercent, _locoAnimationsmoothTime, Time.deltaTime);
    }
    #endregion

    #region 공격
    public void Attack(int turnNumber)
    {
        VelocityZeroSetting();
        _player._stateMachine.Player_StateChange(State.Attack);
        if (_player._curWeaponType != WeaponType.Normal)
        {
            if (turnNumber == 0)
            {
                if (_ComboCount >= 1)
                    _anim.SetBool("Attack1", true);
                else
                    _anim.SetBool("Attack1", false);
            }
            if (turnNumber == 1)
            {
                if (_ComboCount >= 2)
                    _anim.SetBool("Attack2", true);
                else
                    _player._stateMachine.Player_StateChange(State.Idle);

                _anim.SetBool("Attack1", false);
            }
            if (turnNumber == 2)
            {
                if (_ComboCount >= 3)
                    _anim.SetBool("Attack3", true);
                else
                    _player._stateMachine.Player_StateChange(State.Idle);

                _anim.SetBool("Attack2", false);
            }
            if (turnNumber == 3)
            {
                if (_ComboCount >= 4)
                    _anim.SetBool("Attack4", true);
                else
                    _player._stateMachine.Player_StateChange(State.Idle);

                _anim.SetBool("Attack3", false);
            }
            if (turnNumber == 4)
            {
                _anim.SetBool("Attack4", false);
                _player._stateMachine.Player_StateChange(State.Idle);
                _ComboCount = 0;
            }
        }
    }
    #endregion

    public void Skill(string name)
    {
        VelocityZeroSetting();
        _anim.SetTrigger(name);
    }

    #region 회전속도, 이동속도, 카메라 초기화
    public void VelocityZeroSetting()
    {
        _speed = 0;
        _speedpercent = 0;


        if (_rigid.angularVelocity != Vector3.zero)
            _rigid.angularVelocity = Vector3.zero;

        if (_rigid.velocity != Vector3.zero)
            _rigid.velocity = Vector3.zero;
    }
    #endregion

    #region 벽 이동 불가 기능
    void StopToWall()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        _isBorder = Physics.Raycast(pos, transform.forward, out _hit, 1f, LayerMask.GetMask("Wall"));
    }
    #endregion
}
