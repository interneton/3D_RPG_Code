using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
