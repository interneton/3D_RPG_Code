using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPlaying : MonoBehaviour
{
    [SerializeField]
    protected Transform _parent;
    [SerializeField]
    protected int _skillEffectIndex = 0;

    protected ParticleSystem _myParticle;
    protected ParticleSystem.Particle[] _SkillPos;

    protected Collider _DamageCollider;
    [SerializeField] 
    protected float _colliderOffTimer;


    void Start()
    {

        _SkillPos = new ParticleSystem.Particle[1];

        _myParticle = GetComponentInChildren<ParticleSystem>();

        _DamageCollider = transform.Find("DamageTrigger").GetComponentInChildren<Collider>();

    }


    public void Playing()
    {
        StartCoroutine("_SkillUsing");
    }
    protected virtual IEnumerator _SkillUsing()
    {
        SkillManager.Instance._skilleffect[_skillEffectIndex]._effect.Play();

        _myParticle.Play();
        if (_DamageCollider.enabled == false)
            _DamageCollider.enabled = true;
        Invoke("ColiderOnOff", _colliderOffTimer);
        transform.localPosition = new Vector3(0, 0.1f , 0);
        transform.forward = GameManager.Instance._player.transform.forward;
        transform.parent = _parent;

        while (true)
        {
            if (_myParticle.isPlaying)
            {
                _myParticle.GetParticles(_SkillPos);
                _DamageCollider.transform.position = _SkillPos[0].position;
            }

            if (_myParticle.isStopped)
            {
                transform.parent = GameManager.Instance._player.transform.Find("SkillPooling");
                break;
            }


            yield return null;
        }
    }

    protected void ColiderOnOff()
    {
        if (_DamageCollider.enabled == true)
            _DamageCollider.enabled = false;
    }
}
