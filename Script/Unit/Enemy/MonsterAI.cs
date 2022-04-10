using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum MonsterState
{
    NULL = -1,
    COMBAT, // ����
    NONCOMBAT, // ������
}
public enum BehaviourTree
{
    NULL = -1,
    IDLE,
    MOVE,
    ATTACK,
    HIT,
    DEATH,
}

public class MonsterAI : MonoBehaviour
{
    public float speed_Combat;
    public float speed_NonCombat;

    [SerializeField] int AttackCombo;


    [Header("�ι� ����")]
    [SerializeField] int moveRange;
    [SerializeField] int trackingRange;
    [SerializeField] int IsSightRange = 7;

    NavMeshAgent nav;
    MonsterController monsteranimCT;

    [Header("����")]
    [SerializeField] bool IsBack;
    [SerializeField] bool IsCoroutine;
    [SerializeField] bool inAttackRange;


    float IdleTimer = 0;
    float originDis;
    float targetDis;
    Vector3 originPos;
    Vector3 targetPos;

    private void OnEnable()
    {
        originPos = transform.position;

        if (nav == null)
            nav = GetComponent<NavMeshAgent>();

        if (monsteranimCT == null)
            monsteranimCT = GetComponent<MonsterController>();

        StartCoroutine("EnemyRoamingAI");
    }

    private void OnDisable()
    {
        IsBack = false;
        IsCoroutine = false;
        inAttackRange = false;

    }

    // ������ ����
    void Move_Combat()
    {
        targetPos = GameManager.Instance._player.transform.position;

        if (!inAttackRange && monsteranimCT._beHaviour_State != BehaviourTree.HIT)
        {
            NavSpeedSetting(speed_Combat);
            nav.SetDestination(targetPos);
            if (targetDis <= 2)
            {
                inAttackRange = true;
                monsteranimCT.Anim_Idle();
            }
        }


        if (originDis >= trackingRange && monsteranimCT._beHaviour_State != BehaviourTree.DEATH) // ���ڸ��� ���ư���
        {
            monsteranimCT.Change_cur_Combat(MonsterState.NONCOMBAT);

            IsBack = true;
            monsteranimCT.Anim_Move();
            NavSpeedSetting(8f);
            targetPos = originPos;
            nav.SetDestination(targetPos);
        }
    }

    // �������� ����
    void Move_NonCombat()
    {
        if (targetDis <= 3) // ���ڸ��� ���ƿ�����
        {
            monsteranimCT.Anim_Idle();
            monsteranimCT.Change_cur_Combat(MonsterState.NONCOMBAT);

            monsteranimCT.m_monster.curHp = monsteranimCT.m_monster.maxHp;
            monsteranimCT.m_monster.HpbarRefresh();
            monsteranimCT.m_monster.BackSliderReSet();

            IsBack = false;
        }

        if (monsteranimCT._beHaviour_State != BehaviourTree.MOVE)
        {
            if (IdleTimer <= 0.01f)
                monsteranimCT.Anim_Idle();

            IdleTimer += Time.deltaTime;

            if (IdleTimer >= 2.5f)
            {
                IdleTimer = 0;

                monsteranimCT.Anim_Move();
                NavSpeedSetting(speed_NonCombat);

                targetPos = new Vector3(transform.position.x + Random.Range(-1 * moveRange, moveRange),
                    nav.velocity.y, transform.position.z + Random.Range(-1 * moveRange, moveRange));

                nav.SetDestination(targetPos);
            }
        }

        if (originDis >= moveRange)
        {
            monsteranimCT.Anim_Move();

            targetPos = originPos;
            nav.SetDestination(targetPos);
        }

    }

    // �ִϸ��̼� �̺�Ʈ�� ���� ����
    public void Enemy_Atk()
    {
        if (monsteranimCT._beHaviour_State == BehaviourTree.HIT)
            return;

        if (monsteranimCT._curState == MonsterState.COMBAT)
        {
            if (targetDis <= 3 && !IsCoroutine)
            {
                IsCoroutine = true;
                StartCoroutine("AttackDelay");
            }
            if (targetDis > 3)
            {
                IsCoroutine = false;
                inAttackRange = false;
                monsteranimCT.Anim_Move();
                StopCoroutine("AttackDelay");
            }
        }
    }

    // ���� ������
    IEnumerator AttackDelay()
    {
        while (true)
        {
            int number = Random.Range(0, AttackCombo);
            monsteranimCT.Anim_Attack(number);
            yield return new WaitForSeconds(1f);
            monsteranimCT.Anim_Idle();
            yield return new WaitForSeconds(monsteranimCT.m_monster.attackSpeed - 1f);
        }
    }

    // ������� ��� �ڷ�ƾ���� ���� ����
    IEnumerator EnemyRoamingAI()
    {
        yield return new WaitUntil(() => monsteranimCT != null);

        while (gameObject.activeSelf == true)
        {
            originDis = (originPos - transform.position).magnitude;
            targetDis = (targetPos - transform.position).magnitude;

            if (monsteranimCT._curState == MonsterState.NONCOMBAT && monsteranimCT._beHaviour_State != BehaviourTree.DEATH)
                Move_NonCombat();

            if (monsteranimCT._curState == MonsterState.COMBAT && monsteranimCT._beHaviour_State != BehaviourTree.DEATH)
            {
                Move_Combat();
                LookRotation();
            }


            if (monsteranimCT.m_monster._Targetdistance < (float)IsSightRange
                && monsteranimCT._curState == MonsterState.NONCOMBAT && !IsBack) // �÷��̾ ����������
            {
                monsteranimCT.Change_cur_Combat(MonsterState.COMBAT);
            }


            if (monsteranimCT._beHaviour_State == BehaviourTree.DEATH)
            {
                NavSpeedSetting(0);
                StopAllCoroutines();
                break;
            }

            yield return null;
        }
    }

    // �÷��̾� ���� �ٶ󺸱�
    void LookRotation()
    {
        if (monsteranimCT._beHaviour_State != BehaviourTree.ATTACK)
        {
            Vector3 direction = (targetPos - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));

            if (transform.rotation == lookRotation)
                return;

            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    // �ִϸ��̼� �̺�Ʈ���� ���ǵ� �ʱ�ȭ
    public void NavSpeedSetting(float speed)
    {
        nav.speed = speed;
    }


}
