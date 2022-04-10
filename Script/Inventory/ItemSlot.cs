using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected Image icon;

    public Item myItem;

    [Header("ÆË¾÷ Ã¢")]
    [SerializeField]protected Inventory inventory;



    protected virtual void Start()
    {
        inventory = GameManager.Instance.GetComponent<Inventory>();
    }


    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (myItem != null)
        {
            UIManger.Instance._IsPoint = true;
            UIManger.Instance._EquipPopup.GetInfoToPopUp(myItem);

           
            RectTransform rectTrans = UIManger.Instance._EquipPopup.GetComponent<RectTransform>();
            rectTrans.transform.position = transform.position;

            Debug.Log("µé¾î°¬´Ù");
        }
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (UIManger.Instance._IsPoint == true)
        {
            UIManger.Instance._IsPoint = false;
        }
        Debug.Log("³ª°«´Ù");

    }

    public void MyInfoSet(Item item)
    {
        icon = GetComponent<Image>();
        myItem = item;
        if (item._Index != "")
            icon.sprite = Inventory._instance._ItemSpriteIcon[System.Convert.ToInt32(item._Index)]._Sprite;

        icon.enabled = true;
        myItem._IsInven = true;

    }



    protected void InventorySlotRefresh()
    {
        myItem._IsInven = false;
        icon.sprite = null;
        icon.enabled = false;
        myItem._IsInven = true;
        UIManger.Instance._IsPoint = false;
        GameManager.Instance._player.StatsUpdate(myItem, true);
    }


    public virtual void useButton()
    {
      
    }

}


