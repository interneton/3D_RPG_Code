using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InvenSlot : ItemSlot
{
    public override void useButton()
    {
        InventorySlotRefresh();

        switch (myItem._Type.Trim()) // 머리, 목, 갑옷, 하의, 신발, 무기
        {
            case "OneHand":
            case "TwoHand":
            case "OnehandBig":
            case "TwoHandBig":    // 무기 케이스
                {
                    Inventory._instance.OldItemAndNewItemChange(5, myItem);
                }
                break;
            case "Helmet":
                {
                    Inventory._instance.OldItemAndNewItemChange(0, myItem);
                }
                break;
            case "Necklace":
                {
                    Inventory._instance.OldItemAndNewItemChange(1, myItem);
                }
                break;
            case "Armor":
                {
                    Inventory._instance.OldItemAndNewItemChange(2, myItem);
                }
                break;
            case "Pants":
                {
                    Inventory._instance.OldItemAndNewItemChange(3, myItem);
                }
                break;
            case "Boots":
                {
                    Inventory._instance.OldItemAndNewItemChange(4, myItem);
                }
                break;
        }
        myItem = new Item();
    }
}