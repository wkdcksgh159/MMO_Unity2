using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Button : UI_Popup
{
    enum Buttons
    {
        PointButton
    }

    enum Texts
    {
        PointText,
        ScoreText
    }

    enum GameObjects
    {
        TestObject
    }

    enum Images
    {
        ItemIcon,
    }

    public override void Init()
    {
        base.Init(); // UI_Popup Init 함수 실행

        Bind<Button>(typeof(Buttons)); // Button 타입의 객체들을 가져온다
        Bind<Text>(typeof(Texts)); // Text 타입의 객체들을 가져온다
        Bind<GameObject>(typeof(GameObjects)); // GameObject 타입의 객체들을 가져온다
        Bind<Image>(typeof(Images)); // GameObject 타입의 객체들을 가져온다

        //GetImage((int)Images.ItemIcon).gameObject.BindEvent();
        // Button 객체를 가져와서 BindEvent 를 실행한다
        // 가져온 Button 객체가 첫 인자로 전달되고 이후로 대리자를 통해 OnButtonClicked 와 기본값인 Click 타입이 전달된다
        GetButton((int)Buttons.PointButton).gameObject.BindEvent(OnButtonClicked);

        GameObject go = GetImage((int)Images.ItemIcon).gameObject; // Image 객체를 가져온다
        BindEvent(go, (PointerEventData data) => { go.transform.position = data.position; }, Define.UIEvent.Drag);
    }

    int _score = 0; // 점수

    public void OnButtonClicked(PointerEventData data) // public 이 아니면 UI에서 확인되지 않는다
    {
        _score++; // 버튼 클릭 시 점수 1 증가
        GetText((int)Texts.ScoreText).text = $"점수 : {_score}"; // ScoreText 객체를 찾아서 text 값을 수정
    }
}
