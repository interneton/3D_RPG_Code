using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System;
using System.IO;

[Serializable]
public class SpriteIcon
{
    public string _Name;
    public Sprite _Sprite;
}

[Serializable]
public class Item
{
    public Item()
    {
        _Name = ""; _Index = ""; _Level = "";
        _Damage = ""; _Defend = ""; _Critical = ""; _Hp = ""; _Mp = ""; _Type = "";
        _IsInven = false;
    }

    public Item(string Name, string Index, string Level, string Damage, string Defend, string Critical, string Hp, string Mp, string Type)
    {
        _Name = Name; _Index = Index; _Level = Level;
        _Damage = Damage; _Defend = Defend; _Critical = Critical; _Hp = Hp; _Mp = Mp; _Type = Type;

    }

    public string _Name, _Index, _Level, _Damage, _Defend, _Critical, _Hp, _Mp, _Type;
    public bool _IsInven;


}

public class Inventory : MonoBehaviour
{
    public TextAsset _ItemDataBase;
    public static Inventory _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found!");
            return;
        }
        _instance = this;

        string[] ItemLine = _ItemDataBase.text.Substring(0, _ItemDataBase.text.Length - 1).Split('\n');

        for (int i = 0; i < ItemLine.Length; i++)
        {
            string[] row = ItemLine[i].Split(',');

            AllItemDataList.Add(new Item(row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8]));
        }


    }
    public int _InvenSpace = 0;
    public ItemSlot[] InvenSlots;
    public EquipSlot[] MyEquipment;
    public List<Item> AllItemDataList;

    public Transform ItemParents;
    public List<SpriteIcon> _ItemSpriteIcon;
    public List<SpriteIcon> _equipSpriteIcon;

    List<Item> newList;

    private void Start()
    {

        InvenSlots = ItemParents.GetComponentsInChildren<ItemSlot>(true);

        Transform equipTrans = UIManger.Instance._EquipSettings.transform.Find("MoveWindow");
        MyEquipment = equipTrans.GetComponentsInChildren<EquipSlot>(true);

        LoadItemData();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U) && GameManager.Instance._player._state != State.Attack)  // 장비 전체 해제
        {
            for (int i = 0; i < 6; ++i)
            {
                if (MyEquipment[i].myItem._IsInven == true && MyEquipment[i].myItem != null)
                    Unequip(i);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
            SaveItemData();

    }

    public void Unequip(int slot)   // 해당 슬롯 아이템 빼기
    {
        if (MyEquipment[slot].myItem._IsInven == true && MyEquipment[slot].myItem != null)
        {

            if (MyEquipment[slot].myItem._Type.Trim() == WeaponType.OneHand.ToString())
            {
                GameManager.Instance._player._stateMachine.ExitStateMachine(WeaponType.OneHand);
                Destroy(GameManager.Instance._player._RightHand.GetChild(0).gameObject);
            }
            if (MyEquipment[slot].myItem._Type.Trim() == WeaponType.TwoHand.ToString())
            {
                GameManager.Instance._player._stateMachine.ExitStateMachine(WeaponType.TwoHand);

                Destroy(GameManager.Instance._player._LeftHand.GetChild(0).gameObject);
                Destroy(GameManager.Instance._player._RightHand.GetChild(0).gameObject);
            }

            MyEquipment[slot].myItem._IsInven = false;
            ItemInstance(MyEquipment[slot].myItem);
            GameManager.Instance._player.StatsUpdate(MyEquipment[slot].myItem, false);  // 빼주기

            MyEquipment[slot].ImageIconReset(_equipSpriteIcon[slot]._Sprite);
            MyEquipment[slot].myItem = new Item();

            GameManager.Instance._EquipStats.StatsInfo_AllUpdate();
        }
    }

    public void OldItemAndNewItemChange(int index, Item newItem)
    {
        Unequip(index);


        newItem._IsInven = true;
        MyEquipment[index].MyInfoSet(newItem);

        if (newItem._Type.Trim() == WeaponType.OneHand.ToString())
        {
            GameManager.Instance._player._stateMachine.WeaponAnimChange(WeaponType.OneHand);
            GameObject obj = Resources.Load("OneHand") as GameObject;
            Instantiate(obj, GameManager.Instance._player._RightHand);
        }
        if (newItem._Type.Trim() == WeaponType.TwoHand.ToString())
        {
            GameManager.Instance._player._stateMachine.WeaponAnimChange(WeaponType.TwoHand);
            GameObject obj = Resources.Load("OneHand") as GameObject;
            Instantiate(obj, GameManager.Instance._player._RightHand);
            Instantiate(obj, GameManager.Instance._player._LeftHand);
        }

        GameManager.Instance._player.ParticleChange();
        GameManager.Instance._EquipStats.StatsInfo_AllUpdate();
    }

    public bool ItemInstance(Item instanItem)
    {
        ItemSlot trans = SetInventoryItem();

        if (trans != null) // 아이템 획득시
        {
            trans.MyInfoSet(instanItem);
            trans.myItem._IsInven = true;

            return true;
        }

        return false;
    }


    ItemSlot SetInventoryItem()
    {
        ItemSlot trans = null;

        foreach (ItemSlot obj in InvenSlots)
        {
            if (obj.myItem._IsInven == false) // 아이템이 없을시
            {
                trans = obj;
                return trans;
            }
        }
        return null;
    }

    public void LoadItemData()
    {
        newList = new List<Item>();

        string jdata = File.ReadAllText(Application.dataPath + "/Resources/MyItemData.txt");
        newList = JsonConvert.DeserializeObject<List<Item>>(jdata);

        for (int i = 0; i < InvenSlots.Length; i++)
        {
            if (newList == null)
                return;
            if (newList[i]._Name != "")
                InvenSlots[i].MyInfoSet(newList[i]);
        }
    }

    public void SaveItemData()
    {
        newList = new List<Item>();
        for (int i = 0; i < InvenSlots.Length; i++)
            newList.Add(InvenSlots[i].myItem);

        string jdata = JsonConvert.SerializeObject(newList);
        File.WriteAllText(Application.dataPath + "/Resources/MyItemData.txt", jdata);
    }

    public int InventorySpaceCheck()
    {
        _InvenSpace = 0;

        foreach (InvenSlot item in InvenSlots)
        {
            if (item.myItem._Name != "")
                _InvenSpace++;
        }
        return _InvenSpace;
    }
}

