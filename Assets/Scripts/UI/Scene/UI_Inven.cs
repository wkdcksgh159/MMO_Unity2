using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inven : UI_Scene
{
    enum GameObjects // 가져올 UI_Inven 컴포넌트 이름을 입력
    {
        GridPanel,
    }

    public override void Init()
    {
        base.Init(); // UI_Scene 초기화 실행

        Bind<GameObject>(typeof(GameObjects)); // GameObjects 의 컴포넌트들을 불러온다(GridPanel)

        GameObject gridPanel = Get<GameObject>((int)GameObjects.GridPanel); // GridPanel 컴포넌트를 가져온다
        foreach (Transform child in gridPanel.transform) // GridPanel 내부의 모든 컴포넌트들을 제거한다
            Managers.Resource.Destroy(child.gameObject); 

        // 실제 인벤토리 정보를 참고해서(나중에 인벤토리 정보를 만들면)
        for (int i = 0; i < 8; i++)
        {
            //GameObject item = Managers.Resource.Instantiate("UI/Scene/UI_Inven_Item"); // UI_Inven_Item 프리팹으로 컴포넌트 생성
            //item.transform.SetParent(gridPanel.transform); // GridPanel 의 내부에 생성
            GameObject item = Managers.UI.MakeSubItem<UI_Inven_Item>(parent:gridPanel.transform).gameObject; // GridPanel 내부에 UI_Inven_Item 컴포넌트 생성

            UI_Inven_Item invenItem = item.GetOrAddComponent<UI_Inven_Item>(); // UI_Inven_Item 컴포넌트 정보를 가져온다
            invenItem.SetInfo($"집행검{i}번"); // SetInfo 실행(아이템 텍스트 입력)
        }
    }
}
