using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//          장비창 슬롯에 스크립트
//

public class EquipSlot : ItemSlot
{
    public void ImageIconReset(Sprite sprite)
    {
        icon.sprite = sprite;
    }

    public override void useButton()
    {
        if (myItem == null)
            return;
        
        
    }
}
