using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//
//      UI 드래그 스크립트
//

public class DragUI : MonoBehaviour, IDragHandler , IEndDragHandler
{
    [SerializeField]Transform pivot;
    [SerializeField] Transform moveWindow;

    void Start()
    {
        pivot = transform.Find("pivot");
        moveWindow = transform.Find("MoveWindow");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 screenpos = Input.mousePosition;

        pivot.position = screenpos;
        moveWindow.parent = pivot;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        moveWindow.parent = moveWindow.parent.parent;
    }
}
