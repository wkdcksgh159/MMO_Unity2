using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extension
{
    // this 로 선언한 GameObject 에서 BindEvent 를 실행할 수 있게 된다
    public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
        return Util.GetOrAddComponent<T>(go); // GetOrAddComponent 래핑
    }

    public static void BindEvent(this GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_Base.BindEvent(go, action, type); // BindEvent 래핑
    }

    public static bool IsValid(this GameObject go)
    {
        return go != null && go.activeSelf; // 객체가 null 이 아니거나 active 상태이거나
    }
}