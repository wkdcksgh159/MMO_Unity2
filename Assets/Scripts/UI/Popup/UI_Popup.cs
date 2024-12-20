using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup : UI_Base
{
    public override void Init()
    {
        Managers.UI.SetCanvas(gameObject, true); // UIMAnager의 SetCanvas 함수 실행
    }

    public virtual void ClosePopupUI()
    {
        Managers.UI.ClosePopupUI(this); // UIMAnager의 ClosePopupUI 함수 실행
    }
}
