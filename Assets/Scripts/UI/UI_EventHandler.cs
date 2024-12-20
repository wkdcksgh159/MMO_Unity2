using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// IBeginDragHandler : 마우스로 처음 클릭해서 드래그를 시작할 때
// IDragHandler : 계속 클릭된 상태로 드래그를 진행할 때
public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IDragHandler // 이벤트 핸들러를 상속
{
    public Action<PointerEventData> OnClickHandler = null;
    public Action<PointerEventData> OnDragHandler = null;
    
    public void OnPointerClick(PointerEventData eventData) // 클릭
    {
        if (OnClickHandler != null)
            OnClickHandler.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData) // 드래그 진행
    {
        if (OnDragHandler != null)
            OnDragHandler.Invoke(eventData);
    }

    
}
