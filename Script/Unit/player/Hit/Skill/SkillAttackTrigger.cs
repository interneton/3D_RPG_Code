using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAttackTrigger : PlayerAttackTrigger
{
    [SerializeField] protected float _minDamage;
    [SerializeField] protected float _maxDamage;
    [SerializeField] protected float _minCri;
    [SerializeField] protected float _maxCri;


    // 스킬 데미지 처리 ( 플레이어 치명타율 + 플레이어 공격력)
    public override int CurDamage()
    {
        int criRate = Random.Range(0, 100);

        if (criRate < _player._myStats.CRI * 100)
            _dmgRate = Random.Range(_minCri, _maxCri);
        else
        {
            _dmgRate = Random.Range(_minDamage, _maxDamage);
        }

        _dmg = (int)(_player._myStats.ATK * _dmgRate);

        return _dmg;
    }


}
