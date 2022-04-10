using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDamageTrigger : MonoBehaviour
{
    ParticleSystem[] _bloodParticle;
    MonsterController _monster_controller;
    void Start()
    {
        _bloodParticle = GetComponentsInChildren<ParticleSystem>();
        _monster_controller = transform.parent.GetComponent<MonsterController>();
    }

    List<int> WeaponId = new List<int>();
    int i = 0;

    // 몬스터 데미지 처리
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("PlayerAttack") || _monster_controller.m_monster.curHp <= 0)
            return;

        if (i <= 2)
            i = 0;

        foreach (int id in WeaponId)
        {
            if (id == other.GetInstanceID())
                return;
        }

        _monster_controller.ChangeBehaviourTree(BehaviourTree.HIT);
        Vector3 direction = (_monster_controller.transform.position - GameManager.Instance._player.transform.position).normalized;

        _monster_controller.transform.GetComponent<Rigidbody>().AddForce(direction.normalized * 100, ForceMode.Force);

        PlayerAttackTrigger attack = other.transform.GetComponent<PlayerAttackTrigger>();
        _monster_controller.OnDamage(attack.CurDamage(), attack._dmgRate);
        BloodParticleOrder(other);

        WeaponId.Add(other.GetInstanceID());
        Invoke("ResetHitObject", 0.2f);
    }

    // 파티클 피 효과
    void BloodParticleOrder(Collider other)
    {
        _bloodParticle[i].transform.forward = other.transform.forward;
        _bloodParticle[i].Play();
        i++;
    }

    void ResetHitObject()
    {
        WeaponId.RemoveAt(0);
    }


}
