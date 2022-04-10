using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MonsterController : MonoBehaviour
{
    public MonsterState _curState;
    public BehaviourTree _beHaviour_State;

    Animator anim;
    Monster _monster;
    public Monster m_monster { get => _monster; }
    PersonalCanvas _personalCanvas;

    public Color _Color1 = new Color(1, 1, 1, 1), _Color2 = new Color(1, 1, 1, 0);
    public float _Offset;

    private Renderer _renderer;
    private Collider _MyCollider;
    private MaterialPropertyBlock _propBlock;

    void Start()
    {
        anim = GetComponent<Animator>();
        _monster = GetComponent<Monster>();
        _MyCollider = GetComponent<Collider>();
        _propBlock = new MaterialPropertyBlock();
        _renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _personalCanvas = transform.Find("UI Canvas").GetComponent<PersonalCanvas>();
    }

    private void OnEnable()
    {
        ChangeBehaviourTree(BehaviourTree.IDLE);
        Change_cur_Combat(MonsterState.NONCOMBAT);

        if (_MyCollider != null)
            _MyCollider.enabled = true;
    }


    // 머터리얼 색깔 바꾸는 함수
    void RenderingChange(int mode, int order)
    {
        switch (order)
        {
            case 1:
                {
                    _renderer.material.SetFloat("_Mode", mode);
                    _renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    _renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcColor);
                    _renderer.material.SetInt("_ZWrite", 0);
                    _renderer.material.DisableKeyword("_ALPHATEST_ON");
                    _renderer.material.DisableKeyword("_ALPHABLEND_ON");
                    _renderer.material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                    _renderer.material.renderQueue = 3000;
                }
                break;
            case 2:
                {
                    _renderer.material.SetFloat("_Mode", mode);
                    _renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    _renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    _renderer.material.SetInt("_ZWrite", 1);
                    _renderer.material.DisableKeyword("_ALPHATEST_ON");
                    _renderer.material.DisableKeyword("_ALPHABLEND_ON");
                    _renderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    _renderer.material.renderQueue = -1;
                }
                break;
        }
    }

    public void Anim_Idle()
    {
        ChangeBehaviourTree(BehaviourTree.IDLE);
        anim.Play("Idle");
    }

    public void Anim_Move()
    {
        ChangeBehaviourTree(BehaviourTree.MOVE);

        if (anim != null)
        {
            if (_curState == MonsterState.NONCOMBAT)
                anim.Play("Walk");

            if (_curState == MonsterState.COMBAT)
                anim.Play("Run");
        }

    }

    public void Anim_Attack(int number)
    {
        ChangeBehaviourTree(BehaviourTree.ATTACK);

        if (number == 0)
            anim.Play("Attack01");
        else if (number == 1)
            anim.Play("Attack02");
        else if (number == 2)
            anim.Play("Attack03");
    }
    
    public void Anim_Hit()
    {
        ChangeBehaviourTree(BehaviourTree.HIT);
        Change_cur_Combat(MonsterState.COMBAT);
        anim.Play("Hit");
    }

    public void Anim_Die()
    {
        _monster.ItemDrop();
        StartCoroutine("Die");
        _MyCollider.enabled = false;
        anim.Play("Die");
    }

    // 죽었을시, 머터리얼 색깔 투명하게 바꾸기
    IEnumerator Die()
    {
        yield return new WaitForSeconds(1.0f);
        float duration = 3f;
        float elapsed = 0f;
        RenderingChange(3, 1);
        while (elapsed <= duration)
        {
            elapsed += Time.deltaTime;
            _renderer.GetPropertyBlock(_propBlock);
            _propBlock.SetColor("_Color", Color.Lerp(_Color1, _Color2, elapsed / duration));
            _renderer.SetPropertyBlock(_propBlock);
            yield return null;
        }

        gameObject.SetActive(false);
        RenderingChange(1, 2);

    }

    // 데미지 처리
    public void OnDamage(int damage, float dmgRate)
    {
        int _hp = _monster.curHp;
        _hp -= damage;

        OnDmgText(damage, dmgRate);
        if (_hp > 0)
        {
            Anim_Hit();
            _monster.BackSliderLerp();
        }
        else if (_hp <= 0)
        {
            ChangeBehaviourTree(BehaviourTree.DEATH);
            _monster.curHp = 0;
            _monster.HpbarRefresh();
            Anim_Die();

            UIManger.Instance._QuestUI.GetComponent<QuestUI>().QuestUpdate(_monster.ThisMonster);
            // 소리 추가

            return;
        }
        _monster.curHp = _hp;    // 몬스터에 체력 대입하기
        _monster.HpbarRefresh();   // 체력바 초기화 시켜주기

    }

    // 데미지 처리 텍스트
    int TextCount = 0;
    private void OnDmgText(int dmg, float dmgRate)
    {
        if (TextCount == 2)
            TextCount = 0;

        TextMeshProUGUI textObj = _personalCanvas._list_dmgText[TextCount].GetComponent<TextMeshProUGUI>();
        textObj.text = dmg.ToString();
        textObj.fontSize = 50 * dmgRate;
        textObj.gameObject.SetActive(true);
        TextCount++;
    }

    // 몬스터 Idle, Move, Attack , Hit , Death  상태 전환
    public void ChangeBehaviourTree(BehaviourTree newTree)
    {
        if (_beHaviour_State != newTree)
            _beHaviour_State = newTree;
    }
    
    // 전투 , 비전투 상태 전환
    public void Change_cur_Combat(MonsterState newCombat)
    {
        if (_curState != newCombat)
            _curState = newCombat;
    }

    // 애니메이션 이벤트에 적용
    public void AnimationEventIdleState()
    {
        ChangeBehaviourTree(BehaviourTree.IDLE);
    }
}
