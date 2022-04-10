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

    [Header("몬스터 정보")]
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

    [Header("몬스터 타겟")]
    Vector3 _myPos;
    Player _player;
    public float _Targetdistance = 30;

    [Header("체력 바 관련 스크립트")]
    protected Canvas _uiCanvas;
    protected Slider _HpBar;
    public GameObject _MyHpObject;
    protected TextMeshProUGUI _Hptext;

    [SerializeField]
    Collider _attackCol;


    [Header("드롭 아이템")]
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

    void ReVive()   // 몬스터 재소환
    {
        transform.position = _myPos;
        gameObject.SetActive(true);
    }

    private void LateUpdate()
    {
        _Targetdistance = Vector3.Distance(_player.transform.position, transform.position); // 거리체크
        SetHpbarPos();
    }

    // 몬스터 정보 받아오기
    private void MonsterInfoInit()
    {
        _player = GameManager.Instance._player;   // 타겟

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

            curHp = maxHp; //최대 체력으로 현재 체력 생성
        }
        _uiCanvas = transform.Find("UI Canvas").GetComponent<Canvas>();
        _MyHpObject = Instantiate(Resources.Load("Monster_HpSlider") as GameObject, _uiCanvas.transform);
        _MyHpObject.transform.Find("MonsterName").Find("text").GetComponent<TextMeshProUGUI>().text = Name;
        _Hptext = _MyHpObject.transform.Find("text").GetComponent<TextMeshProUGUI>();

        _HpBar = _MyHpObject.transform.Find("FrontSlider").GetComponent<Slider>();
        _MyHpObject.SetActive(false);
        HpbarRefresh(); // 체력바 초기화
    }
    // 체력 업데이트
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

    // 체력 UI , 플레이어 거리 체크해서 켜고, 끄기
    public void SetHpbarPos()
    {
        if (_Targetdistance >= 15 && _MyHpObject.activeSelf == true || _MonsterCT._beHaviour_State == BehaviourTree.DEATH) // 거리가 멀어지면  꺼주기
            _MyHpObject.SetActive(false);

        if (_Targetdistance < 15 && _MyHpObject.activeSelf == false && _MonsterCT._beHaviour_State != BehaviourTree.DEATH) // 거리가 가까워지면 켜주기
                _MyHpObject.SetActive(true);
    }

    public void ItemDrop() // 리스트에 지정된 아이템 랜덤으로 드롭하기
    {
        int rand = Random.Range(0, _itemDrop.Count);
        GameObject Instance = Instantiate(_itemDrop[rand], transform);
        //Instance.GetComponent<ItemPickUp>().owenr = this;
        Instance.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }

    // 애니메이션 이벤트에서 실행 =>   어택 박스 활성화 == > 1 활성 0 비활성 
    public void AttackColDelay(int show)    
    {
        if (_attackCol != null)
        {
            _attackCol.enabled = System.Convert.ToBoolean(show);
        }
    }



}
