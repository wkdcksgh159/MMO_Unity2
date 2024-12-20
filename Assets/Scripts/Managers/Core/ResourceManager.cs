using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    // 리소스를 불러오는 함수
    public T Load<T>(string path) where T : Object // 제네릭 타입을 사용하고 Object 클래스를 상속받은 클래스만 사용
    {
        if (typeof(T) == typeof(GameObject))
        {
            string name = path;
            int index = name.LastIndexOf('/'); // 경로의 '/' 문자의 위치를 찾는다
            if (index >= 0) // '/' 가 있으면
                name = name.Substring(index + 1); // '/' 이후의 문자열을 저장한다 ex) Prefab/Test -> Test

            GameObject go = Managers.Pool.GetOriginal(name); // 원본 오브젝트 존재 여부 확인
            if (go != null) // 있으면
                return go as T; // 리턴
        }

        // 없으면 새로 생성
        return Resources.Load<T>(path); // Resources 의 함수 Load 를 래핑한다
    }

    // 리소스를 동적으로 생성하는 함수
    public GameObject Instantiate(string path, Transform parent = null)
    {
        // 1. Original 이미 들고 있으면 바로 사용
        GameObject original = Load<GameObject>($"Prefabs/{path}"); // Load 함수를 통해 프리팹 파일을 불러온다
        if (original == null) // 프리팹 파일이 없으면
        {
            Debug.Log($"Failed to load prefab : {path}"); // 에러 로그 출력
            return null; // 종료
        }

        // 2. 혹시 풀링된 애가 있을까?
        if (original.GetComponent<Poolable>() != null) // Poolable 컴포넌트가 있으면
            return Managers.Pool.Pop(original, parent).gameObject; // Pool 에서 오브젝트를 가져온다

        GameObject go = Object.Instantiate(original, parent); // 컴포넌트 생성, Instantiate 함수 래핑(재귀함수 방지를 위한 Object 명시적 표시)
        go.name = original.name; // 오브젝트 이름이 겹칠 때 (Clone) 문자열이 붙는것을 방지

        return go; // 생성된 GameObject 리턴
    }

    // 리소스를 동적으로 제거하는 함수
    public void Destroy(GameObject go) 
    {
        if (go == null) // 이미 제거된 리소스면 종료
            return;

        // 만약 풀링이 필요한 아이라면 -> 풀링 매니저한테 위탁
        Poolable poolable = go.GetComponent<Poolable>(); // Poolable 컴포넌트 체크
        if (poolable != null) // 있으면
        {
            Managers.Pool.Push(poolable); // Pool에 다시 넣고 종료
            return;
        }

        Object.Destroy(go); // Destroy 함수 래핑
    }
}
