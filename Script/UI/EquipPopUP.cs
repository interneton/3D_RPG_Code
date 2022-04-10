using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipPopUP : MonoBehaviour
{

    public Image _ItemIcon;
    public TextMeshProUGUI _Name;
    public TextMeshProUGUI _stat_damage;
    public TextMeshProUGUI _stat_defend;
    public TextMeshProUGUI _stat_critical;
    public TextMeshProUGUI _stat_type;



    // 정보를 받아서 띄워주는 팝업창
    public void GetInfoToPopUp(Item info)
    {
        _stat_damage.text = info._Damage;
        _stat_defend.text = info._Defend;
        _stat_critical.text = info._Critical;
        _stat_type.text = info._Type;
        _Name.text = info._Name;

        if (info._Index != "")
            _ItemIcon.sprite = Inventory._instance._ItemSpriteIcon[System.Convert.ToInt32(info._Index)]._Sprite;
    }

}
