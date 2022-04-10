using UnityEngine;

//
//          아이템 스크립트
//

public class ItemPickUp : Interactable
{
    [SerializeField] float _upForce = 1f;
    [SerializeField] float _sideForce = 0.1f;

    Monster _Owner;
    Rigidbody _rigid;

    [SerializeField] Item _myItem;


    private void OnEnable()
    {
        _rigid = GetComponent<Rigidbody>();
        float xForce = Random.Range(-_sideForce, _sideForce);
        float yForce = Random.Range(_upForce * 0.5f, _upForce);
        float zForce = Random.Range(-_sideForce, _sideForce);
        Vector3 force = new Vector3(xForce, yForce, zForce);
        _rigid.velocity = force;
    }


    protected override void Start()
    {
        base.Start();
        Init();
    }

    // 몬스터가 드롭 할때 몬스터 정보를 받아와서 아이템 능력치를 랜덤으로 적용
    void Init()
    {
        _Owner = transform.parent.GetComponent<Monster>();

        int rand = Random.Range(0, Inventory._instance.AllItemDataList.Count);

        Item Item = Inventory._instance.AllItemDataList[rand];
        Item newItem = new Item(Item._Name, Item._Index, Item._Level, Item._Damage, Item._Defend, Item._Critical, Item._Hp, Item._Mp, Item._Type);
        if (_Owner == null)
            _myItem = (newItem);
        else
        {
            _myItem = RandomItemStatsInit(newItem);
        }
        transform.parent = null;
    }

    public override void Interact()
    {
        PickUp();
    }


    void PickUp()
    {
        bool IsPickUp = Inventory._instance.ItemInstance(_myItem);

        _player.GetComponent<CharacterInputLogic>().Anim.Play("pickUp");

        if (IsPickUp)
        {
            Debug.Log("주웠습니다");
            Inventory._instance.SaveItemData();
            Destroy(this.gameObject);
        }

        if (!IsPickUp)
            Debug.Log("못주웠습니다");
    }

    Item RandomItemStatsInit(Item newitem) // 아이템 능력치 랜덤 설정
    {
        int itemLevel = Random.Range(System.Convert.ToInt32(newitem._Level), _Owner.level + 1);
        newitem._Level = itemLevel.ToString();

        int itemDamage = System.Convert.ToInt32(newitem._Damage);
        newitem._Damage = Random.Range(itemDamage, itemDamage * itemLevel).ToString();

        int itemDeffend = System.Convert.ToInt32(newitem._Defend);
        newitem._Defend = Random.Range(itemDeffend, itemDeffend * itemLevel).ToString();

        float.TryParse(newitem._Critical, out float result);
        newitem._Critical = Random.Range(result, result * itemLevel).ToString("F2");

        return newitem;
    }
}
