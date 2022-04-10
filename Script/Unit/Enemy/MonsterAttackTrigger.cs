using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackTrigger : MonoBehaviour
{
    [SerializeField] protected Collider _WeaponCollider;
    Monster _monster;

    [Space(3)]
    protected int _dmg;
    public float _dmgRate = 0.0f;

    protected List<int> _playerId = new List<int>();

    protected virtual void Start()
    {
        _WeaponCollider = GetComponent<Collider>();
        _monster = transform.parent.parent.GetComponent<Monster>();
    }

    // 치명타율 계산해서 현재 데미지로 변환하기
    public virtual int CurDamage()
    {
        int criRate = Random.Range(0, 100);

        if (criRate < 50)
            _dmgRate = Random.Range(1.5f, 2.5f);
        else
            _dmgRate = Random.Range(0.8f, 1.2f);


        _dmg = (int)(_monster.damage1 * _dmgRate);

        return _dmg;
    }


    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("PlayerHitBox"))
            return;

        foreach (int item in _playerId)
        {
            if (item == other.GetInstanceID())
                return;
        }
        _playerId.Add(other.GetInstanceID());
        Invoke("ResetMonster", 0.2f);
    }

    protected void ResetMonster()
    {
        _playerId.RemoveAt(0);
    }
}
