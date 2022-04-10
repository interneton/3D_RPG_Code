using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : SkillPlaying
{
    [SerializeField]
    GameObject _Rangeobj;

    // 라이트닝 스킬 사용시
    protected override IEnumerator _SkillUsing()
    {
        transform.parent = _parent;
        SkillManager.Instance._skilleffect[_skillEffectIndex]._effect.Play();
        _Rangeobj.SetActive(true);
        _myParticle.Play();
        if (_DamageCollider.enabled == false)
            _DamageCollider.enabled = true;
        transform.localPosition = Vector3.zero;
        transform.localScale = new Vector3(1, 1, 1);
        transform.forward = GameManager.Instance._player.transform.forward;

        while (true)
        {
            if (_myParticle.isPlaying)
            {

                _myParticle.GetParticles(_SkillPos);
                if (_DamageCollider.enabled == false)
                    _DamageCollider.enabled = true;
                yield return new WaitForSeconds(0.4f);
                if (_DamageCollider.enabled == true)
                    _DamageCollider.enabled = false;
            }

            if (_myParticle.isStopped)
            {
                if (_DamageCollider.enabled == true)
                    _DamageCollider.enabled = false;
                _Rangeobj.SetActive(false);

                transform.parent = GameManager.Instance._player.transform.Find("SkillPooling");
                break;
            }


            yield return null;
        }
    }

}
