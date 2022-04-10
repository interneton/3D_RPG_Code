using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum MonsterName
{
    NULL = -1,
    Slime = 0,
    Spector,
    GoblinWarrior,
    Werewolf,
    Worm,
    GoblinMagician,
    Beholder,
    GoblinCheif,
    HunmanGolem,
    FlyingDemon,
    Indian,
    Skeleton,
    SkeletonWarrior,
    DemonBox,
    Knight,
    DragonWarrior,
    Crab,
    NinjaMouse,
    Ghost,
    StoneGolem = 19
}

public class Monster : MonoBehaviour
{
    public MonsterName ThisMonster;

    [Header("���� ����")]
    public string Name;
    public int level;
    public int curHp;
    public int maxHp;
    public int curMp;
    public int maxMp;
    public int defend;
    public int index;
    public int damage1;
    public int damage2;
    public int attackSpeed;
    public int attackRange;

    MonsterInfo myMonsterInfo;
    MonsterController _MonsterCT;

    [Header("���� Ÿ��")]
    Vector3 _myPos;
    Player _player;
    public float _Targetdistance = 30;

    [Header("ü�� �� ���� ��ũ��Ʈ")]
    protected Canvas _uiCanvas;
    protected Slider _HpBar;
    public GameObject _MyHpObject;
    protected TextMeshProUGUI _Hptext;

    [SerializeField]
    Collider _attackCol;


    [Header("��� ������")]
    [SerializeField] List<GameObject> _itemDrop;

    void Start()
    {
        _MonsterCT = GetComponent<MonsterController>();

        MonsterInfoInit();
        _myPos = transform.position;
    }
    private void OnEnable()
    {
        if (_MyHpObject != null)
        {
            curHp = maxHp;
            HpbarRefresh();
            BackSliderReSet();
        }
    }
    private void OnDisable()
    {
        Invoke("ReVive", 5f);
    }

    void ReVive()   // ���� ���ȯ
    {
        transform.position = _myPos;
        gameObject.SetActive(true);
    }

    private void LateUpdate()
    {
        _Targetdistance = Vector3.Distance(_player.transform.position, transform.position); // �Ÿ�üũ
        SetHpbarPos();
    }

    // ���� ���� �޾ƿ���
    private void MonsterInfoInit()
    {
        _player = GameManager.Instance._player;   // Ÿ��

        myMonsterInfo = GameManager.Instance._Data_Monster.GetData((int)ThisMonster);

        if (myMonsterInfo != null)
        {
            this.Name = myMonsterInfo.Name;
            this.index = myMonsterInfo.index;
            this.level = myMonsterInfo.level;
            this.maxHp = myMonsterInfo.maxHp;
            this.maxMp = myMonsterInfo.maxMp;
            this.damage1 = myMonsterInfo.damage1;
            this.damage2 = myMonsterInfo.damage2;
            this.defend = myMonsterInfo.defend;
            this.attackSpeed = myMonsterInfo.attackSpeed;
            this.attackRange = myMonsterInfo.attackRange;

            curHp = maxHp; //�ִ� ü������ ���� ü�� ����
        }
        _uiCanvas = transform.Find("UI Canvas").GetComponent<Canvas>();
        _MyHpObject = Instantiate(Resources.Load("Monster_HpSlider") as GameObject, _uiCanvas.transform);
        _MyHpObject.transform.Find("MonsterName").Find("text").GetComponent<TextMeshProUGUI>().text = Name;
        _Hptext = _MyHpObject.transform.Find("text").GetComponent<TextMeshProUGUI>();

        _HpBar = _MyHpObject.transform.Find("FrontSlider").GetComponent<Slider>();
        _MyHpObject.SetActive(false);
        HpbarRefresh(); // ü�¹� �ʱ�ȭ
    }
    // ü�� ������Ʈ
    public void HpbarRefresh()
    {
        _HpBar.value = ((float)curHp / (float)maxHp);
        _Hptext.text = string.Format("{0}/{1}", curHp, maxHp);
    }
    public void BackSliderLerp()
    {
        if (curHp > 0)
        {
            SliderLerp lerp = _MyHpObject.GetComponent<SliderLerp>();
            if (lerp != null)
                lerp.LerpSlider();
        }
    }
    public void BackSliderReSet()
    {
        if (_MyHpObject != null)
        {
            SliderLerp getCT = _MyHpObject.GetComponent<SliderLerp>();
            getCT._Back_Slider.value = getCT._Front_Slider.value;
        }
    }

    // ü�� UI , �÷��̾� �Ÿ� üũ�ؼ� �Ѱ�, ����
    public void SetHpbarPos()
    {
        if (_Targetdistance >= 15 && _MyHpObject.activeSelf == true || _MonsterCT._beHaviour_State == BehaviourTree.DEATH) // �Ÿ��� �־�����  ���ֱ�
            _MyHpObject.SetActive(false);

        if (_Targetdistance < 15 && _MyHpObject.activeSelf == false && _MonsterCT._beHaviour_State != BehaviourTree.DEATH) // �Ÿ��� ��������� ���ֱ�
                _MyHpObject.SetActive(true);
    }

    public void ItemDrop() // ����Ʈ�� ������ ������ �������� ����ϱ�
    {
        int rand = Random.Range(0, _itemDrop.Count);
        GameObject Instance = Instantiate(_itemDrop[rand], transform);
        //Instance.GetComponent<ItemPickUp>().owenr = this;
        Instance.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }

    // �ִϸ��̼� �̺�Ʈ���� ���� =>   ���� �ڽ� Ȱ��ȭ == > 1 Ȱ�� 0 ��Ȱ�� 
    public void AttackColDelay(int show)    
    {
        if (_attackCol != null)
        {
            _attackCol.enabled = System.Convert.ToBoolean(show);
        }
    }



}
