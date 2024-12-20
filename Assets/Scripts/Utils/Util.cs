using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    // 특정 Component 를 찾아서 해당 컴포넌트가 존재하는 경우 가져오고 없다면 생성한다
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>(); // Component 를 가져온다
        if (component == null)
            component = go.AddComponent<T>(); // 해당 Component 가 없다면 생성한다
        return component; // Component 리턴
    }

    // GameObject 는 Component 를 상속받지 않기 때문에 GetComponent 를 사용할 수 없다
    // 모든 GameObject는 Transform을 갖고있으므로 Transform 타입으로 실행하여 Transform을 받고
    // 역으로 Transform에서 GameObject를 리턴받는다
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false) // GameObject 전용 FindChild
    {
        Transform transform = FindChild<Transform>(go, name, recursive); // FindChild를 Transform 타입으로 실행하여 Transform 을 받아온다
        if (transform == null)
            return null;
        return transform.gameObject; // Transform의 gameObject 리턴
    }

    // 타입, 이름(선택사항)을 받아서 찾고 recursive 를 허용한다면 객체의 자식 객체까지 탐색
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null) // GameObject 가 null 이면 null 리턴
            return null;

        if (recursive == false) // 자식 객체까지 탐색하지 않는다면
        {
            for (int i = 0; i < go.transform.childCount; i++) // 매개변수로 전달받은 GameObject 전체 탐색
            {
                Transform transform = go.transform.GetChild(i); // 객체를 가져온다
                if (string.IsNullOrEmpty(name) || transform.name == name) // name 이 null 이거나 원하는 객체를 찾은 경우
                {
                    T component = transform.GetComponent<T>(); // 해당 component 를 가져온다
                    if (component != null) //  null이 아니면 component 리턴
                        return component;
                }
            }
        }
        else // 자식 객체까지 탐색한다면
        {
            // GetComponentsInChildren : 특정 GameObject 의 모든 자식 Component를 반복자로 반환한다
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name) // name이 null이거나 원하는 객체를 찾은 경우
                    return component; // component 리턴
            }
        }

        return null;
    }

}
