using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillInfo : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public string _SkillName;

    public bool _IsCoolTime;
    public float _SkillCoolTimer;

    public Image _myImg;

    Transform _parent;
    RectTransform _rectTrans;

    public void Start()
    {

        _parent = transform.parent;

        if (_myImg == null)
            _myImg = GetComponent<Image>();
        if (_rectTrans == null)
            _rectTrans = GetComponent<RectTransform>();

        // 스킬 정보 보내주기 => 드롭 했을때로 변경해야함
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_IsCoolTime == true)
            return;

        transform.parent = UIManger.Instance.transform.Find("LastUI");
        transform.position = eventData.position;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (_IsCoolTime == true)
            return;

        SkillManager.Instance.curDragObj(this);
        _myImg.raycastTarget = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_IsCoolTime == true)
            return;

        _myImg.raycastTarget = true;
        transform.parent = _parent;
        _rectTrans.localPosition = new Vector3(0, 24.8f, 0);
    }

}
