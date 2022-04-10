using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsUI : MonoBehaviour
{


    public TextMeshProUGUI _Damage;
    public TextMeshProUGUI _Critical;
    public TextMeshProUGUI _Defense;
    public TextMeshProUGUI[] _curHp; // 0 장비창 스텟 , 1 체력바 표시
    public TextMeshProUGUI[] _maxHp; // 0 장비창 스텟 , 1 체력바 표시

    public Slider _PlayerHpBar;

    public void StatsInfo_AllUpdate()
    {
        Player player = GameManager.Instance._player;
        _Damage.text = player._myStats.ATK.ToString();
        _Critical.text = player._myStats.CRI.ToString("F2");
        _Defense.text = player._myStats.DEF.ToString();
        StatsInfo_HpUpdate();
        HpSliderUpdate(player._myStats.curHp, player._myStats.maxHp);
    }

    public void StatsInfo_HpUpdate()
    {
        Player player = GameManager.Instance._player;
        _curHp[0].text = player._myStats.curHp.ToString();
        _maxHp[0].text = "/" + player._myStats.maxHp.ToString();

    }

    public void HpSliderUpdate(float curHp, float maxHp)
    {
        StatsInfo_HpUpdate();

        if (_PlayerHpBar != null)
        {
            _PlayerHpBar.value = curHp / maxHp;
        _curHp[1].text = curHp.ToString();
        _maxHp[1].text = "/ " + maxHp.ToString();
        }
    }

}
