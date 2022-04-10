using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackTrigger : MonoBehaviour
{
    [SerializeField] protected Player _player;
    [SerializeField] protected Collider _WeaponCollider;

    [Space(3)]
    protected int _dmg;
    public float _dmgRate = 0.0f;

    protected List<int> _monsterID = new List<int>();

    protected virtual void Start()
    {
        _WeaponCollider = GetComponent<Collider>();
        _player = GameManager.Instance._player;
    }

    // 치명타율 계산해서 현재 데미지로 환산
    public virtual int CurDamage()
    {
        int criRate = Random.Range(0, 100);

        if (criRate < _player._myStats.CRI * 100)
            _dmgRate = Random.Range(2f, 2.5f);
        else
        {
            _dmgRate = Random.Range(0.8f, 1.2f);
        }

        _dmg = (int)(_player._myStats.ATK * _dmgRate);

        return _dmg;
    }


    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("EnemyHitBox"))
            return;

        foreach (int item in _monsterID)
        {
            if (item == other.GetInstanceID())
                return;
        }
        _monsterID.Add(other.GetInstanceID());
        Invoke("ResetMonster", 0.2f);
    }

    protected void ResetMonster()
    {
        _monsterID.RemoveAt(0);
    }
}
