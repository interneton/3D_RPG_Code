using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageTrigger : MonoBehaviour
{
    Player _player;
    ParticleSystem[] _hitParticle;
    CharacterInputLogic _charInputlogic;
    PlayerStateMachine _playerStatemachine;

    void Start()
    {
        _player = transform.parent.GetComponent<Player>();
        _hitParticle = GetComponentsInChildren<ParticleSystem>();
        _charInputlogic = transform.parent.GetComponent<CharacterInputLogic>();
        _playerStatemachine = transform.parent.GetComponent<PlayerStateMachine>();
    }

    List<int> WeaponId = new List<int>();
    int i = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("EnemyAttack"))
            return;

        if (i <= 2)
            i = 0;

        foreach (int id in WeaponId)
        {
            if (id == other.GetInstanceID())
                return;
        }

        _playerStatemachine.Player_StateChange(State.Hit);

        transform.parent.GetComponent<Rigidbody>().AddForce(other.transform.forward * 100, ForceMode.Force);
        transform.parent.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        MonsterAttackTrigger attack = other.transform.GetComponent<MonsterAttackTrigger>();
        _player.OnDamage(attack.CurDamage(), attack._dmgRate);
        BloodParticleOrder(other);

        WeaponId.Add(other.GetInstanceID());
        Invoke("ResetHitObject", 0.2f);
    }

    void BloodParticleOrder(Collider other)
    {
        _hitParticle[i].transform.forward = other.transform.forward;
        _hitParticle[i].Play();
        i++;
    }

    void ResetHitObject()
    {
        WeaponId.RemoveAt(0);

    }
}
