using UnityEngine;

//
//          ������ ��ũ��Ʈ
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

    // ���Ͱ� ��� �Ҷ� ���� ������ �޾ƿͼ� ������ �ɷ�ġ�� �������� ����
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
            Debug.Log("�ֿ����ϴ�");
            Inventory._instance.SaveItemData();
            Destroy(this.gameObject);
        }

        if (!IsPickUp)
            Debug.Log("���ֿ����ϴ�");
    }

    Item RandomItemStatsInit(Item newitem) // ������ �ɷ�ġ ���� ����
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
