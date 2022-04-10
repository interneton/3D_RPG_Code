using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//          플레이어 기본 공격시 발생하는 스크립트
//

public class MeleAttack : PlayerAttackTrigger
{
    [SerializeField] 
    HitStop _hitstop;

    protected override void Start()
    {
        base.Start();
        _hitstop = FindObjectOfType<HitStop>();
        _player._WeaponCol = _WeaponCollider;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("EnemyHitBox"))
            return;

        foreach (int item in _monsterID)
        {
            if (item == other.GetInstanceID())
                return;
        }


        _hitstop.StopTime();
        _monsterID.Add(other.GetInstanceID());
        Invoke("ResetMonster", 0.2f);
    }
}
