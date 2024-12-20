using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UI_Base : MonoBehaviour
{
    // <UI객체의 타입, UI객체>
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();
    public abstract void Init(); // UI_Base에서 구현하지 않기에 추상 메소드로 선언

    private void Start() 
    {
        Init(); // 최상위 클래스에서 Init 호출 시 상속받은 클래스에서도 자동으로 호출된다
    }

    protected void Bind<T>(Type type) where T : UnityEngine.Object // Reflection 을 사용해서 enum 타입을 받는다
    {
        string[] names = Enum.GetNames(type); // Enum 타입에 저장된 이름을 가져온다
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length]; // UI 객체를 저장할 Object 배열
        _objects.Add(typeof(T), objects); // Dictionary 에 UI 객체 Object 배열 입력

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject)) // GameObject는 Component를 상속받지 않기 때문에 GetComponent 를 사용할 수 없다
                objects[i] = Util.FindChild(gameObject, names[i], true); // GameObject 전용 FindChild 함수
            else
                objects[i] = Util.FindChild<T>(gameObject, names[i], true); // FindChild 함수로 UI 객체를 찾고 Object 배열에 추가

            if (objects[i] == null)
                Debug.Log($"Failed to Bind({names[i]})");
        }
    }

    protected T Get<T>(int idx) where T : UnityEngine.Object // idx(enum) 를 받아서 Dictionary에 저장된 해당 객체를 리턴한다
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false) // T 타입의 객체가 존재하면 objects에 전달
            return null;

        return objects[idx] as T; // 입력받은 idx(enum)의 object 리턴
    }

    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }
    protected Text GetText(int idx) { return Get<Text>(idx); } // Text 객체를 리턴
    protected Button GetButton(int idx) { return Get<Button>(idx); } // Button 객체를 리턴
    protected Image GetImage(int idx) { return Get<Image>(idx); } // Image 객체를 리턴

    // 게임 오브젝트, 실행할 함수의 대리자, 실행할 UI Event의 타입을 매개변수로 받는다
    public static void BindEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go); // component 를 가져오고 없으면 생성한다

        switch (type)
        {
            case Define.UIEvent.Click: // 클릭
                evt.OnClickHandler -= action; // 중복 방지
                evt.OnClickHandler += action; // 대리자로 전달받은 함수를 핸들러에 추가
                break;
            case Define.UIEvent.Drag: // 드래그
                evt.OnDragHandler -= action; // 중복 방지
                evt.OnDragHandler += action; // 대리자로 전달받은 함수를 핸들러에 추가
                break;
        }
    }
}