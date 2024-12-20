using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inven_Item : UI_Base // UI_Scene 이 아닌 독립적인 존재
{
    enum GameObjects
    {
        ItemIcon, // 아이템 아이콘
        ItemNameText, // 아이템 텍스트
    }

    string _name; // ItemNameText 입력에 사용할 이름

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects)); // GameObjects 에 입력된 컴포넌트들을 불러온다(ItemIcon, ItemNameText)
        Get<GameObject>((int)GameObjects.ItemNameText).GetComponent<Text>().text = _name; // ItemNameText의 Text 컴포넌트를 가져와서 내용을 수정

        Get<GameObject>((int)GameObjects.ItemIcon).BindEvent((PointerEventData) => { Debug.Log($"아이템 클릭! {_name}"); }); // 아이템 클릭시 콘솔 출력
    }

    public void SetInfo(string name) // UI_Inven_Item 정보 입력
    {
        _name = name;
    }
}
