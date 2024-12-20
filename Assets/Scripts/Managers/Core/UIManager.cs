using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    int _order = 10;

    // UI 팝업을 관리하기 위한 Stack
    // 팝업의 경우 마지막에 실행된 팝업부터 종료를 해야하므로 Stack으로 생성
    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    UI_Scene _sceneUI = null; // Scene 컴포넌트를 저장

    public GameObject Root
    {
        get 
        {
            GameObject root = GameObject.Find("@UI_Root"); // 이름으로 GameObject 를 검색
            if (root == null) // 만약 Find로 찾지못하면 null이 반환됨
                root = new GameObject { name = "@UI_Root" }; // 새로운 GameObject 생성
            return root;
        }
    }

    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go); // canvas를 가져온다
        canvas.renderMode = RenderMode.ScreenSpaceOverlay; // 카메라 위치에 관계없이 캔버스가 화면 위에 생성된다
        canvas.overrideSorting = true; // 캔버스가 중첩되었을 때 부모의 sort를 무시한다

        if (sort) // sort 입력 후 sort 증가
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else // sorting 하지 않음
        {
            canvas.sortingOrder = 0;
        }
    }

    public T MakeWorldSpaceUI<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name)) // 이름이 없으면 타입으로 찾는다
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/WorldSpace/{name}"); // UI/SubItem 위치에서 파일을 찾는다

        if (parent != null) // parent가 입력되면 해당 parent 로 SetParent 실행
            go.transform.SetParent(parent);

        Canvas canvas = go.GetComponent<Canvas>(); // Canvas 호출
        canvas.renderMode = RenderMode.WorldSpace; // RenderMode 변경(WorldSpace)
        canvas.worldCamera = Camera.main; // 카메라를 메인 카메라로 지정

        return Util.GetOrAddComponent<T>(go); // 컴포넌트 가져오기or생성
    }

    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name)) // 이름이 없으면 타입으로 찾는다
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}"); // UI/SubItem 위치에서 파일을 찾는다

        if (parent != null) // parent가 입력되면 해당 parent 로 SetParent 실행
            go.transform.SetParent(parent);
        return Util.GetOrAddComponent<T>(go); // 컴포넌트 가져오기or생성
    }

    // 씬 UI를 실행한다
    // UI_Scene 을 상속받은 객체의 이름 혹은 타입을 받고 해당 UI를 실행한다
    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name)) // 이름이 없으면 타입으로 찾는다
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}"); // 찾은 UI의 prefab을 실행한다
        T sceneUI = Util.GetOrAddComponent<T>(go); // 컴포넌트를 가져온다
        _sceneUI = sceneUI;

        go.transform.SetParent(Root.transform); // go 오브젝트의 부모를 root로 설정한다

        return sceneUI; // 컴포넌트 리턴
    }

    // 팝업을 실행한다
    // UI_Popup 을 상속받은 객체의 이름 혹은 타입을 받고 해당 UI를 실행한다
    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name)) // 이름이 없으면 타입으로 찾는다
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}"); // 찾은 UI의 prefab을 실행한다
        T popup = Util.GetOrAddComponent<T>(go); // 컴포넌트를 가져온다
        _popupStack.Push(popup); // 가져온 컴포넌트를 스택에 추가

        go.transform.SetParent(Root.transform); // go 오브젝트의 부모를 root로 설정한다

        return popup; // 컴포넌트 리턴
    }

    public void ClosePopupUI(UI_Popup popup) // 삭제할 팝업이 맞는지 크로스 체크
    {
        if (_popupStack.Count == 0)
            return;

        if (_popupStack.Peek() != popup) // 삭제할 팝업 확인
        {
            Debug.Log("Close Popup Failed!");
            return;
        }

        ClosePopupUI();
    }

    public void ClosePopupUI() // 마지막으로 실행된 팝업을 닫는다
    {
        if (_popupStack.Count == 0) // 스택 카운트 확인
            return;

        UI_Popup popup = _popupStack.Pop(); // 마지막으로 실행된 팝업을 가져온다
        Managers.Resource.Destroy(popup.gameObject); // 객체 제거
        popup = null; // 제거된 객체에 접근하면 안되므로 null로 변경
        _order--; // 객체 개수 -1
    }

    public void CloseAllPopupUI() // 모든 팝업을 종료한다
    {
        while (_popupStack.Count > 0)
            ClosePopupUI();
    }

    public void Clear()
    {
        CloseAllPopupUI(); // 모든 팝업 종료
        _sceneUI = null; // 컴포넌트 초기화
    }
}
