using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    #region Pool
    class Pool // 특정 오브젝트를 여러개 생성하여 관리
    {
        public GameObject Original { get; private set; } // 풀에 저장할 게임 오브젝트(원본)
        public Transform Root { get; private set; } // 특정 오브젝트를 모아두기 위한 부모

        Stack<Poolable> _poolStack = new Stack<Poolable>(); // 풀에 저장한 게임 오브젝트(대기), Poolable Script 유무를 확인하여 관리

        public void Init(GameObject original, int count = 5) // 초기화, 원본 오브젝트, Pool 개수(Count)
        {
            Original = original; // 입력받은 원본 오브젝트를 클래스에 입력
            Root = new GameObject().transform; // 원본 오브젝트를 부모 타입으로 지정
            Root.name = $"{original.name}_Root"; // 원본이름_Root 형식으로 이름 입력

            for (int i = 0; i < count; i++)
                Push(Create()); // 
        }

        Poolable Create() // Pool 생성
        {
            GameObject go = Object.Instantiate<GameObject>(Original); // 원본 오브젝트를 복사한 오브젝트 생성
            go.name = Original.name; // 원본과 동일한 이름
            return go.GetOrAddComponent<Poolable>(); // Pool 여부 확인을 위한 Poolable 컴포넌트 추가
        }

        public void Push(Poolable poolable) // Poolable 오브젝트 세부사항 입력 및 Stack에 추가
        {
            if (poolable == null)
                return;

            poolable.transform.parent = Root; // 부모를 Root 로 연결
            poolable.gameObject.SetActive(false); // Poolable 오브젝트를 비활성화
            poolable.IsUsing = false; // 사용하지 않음

            _poolStack.Push(poolable); // Poolable Stack에 오브젝트 추가
        }

        public Poolable Pop(Transform parent) // Stack에서 Poolable 오브젝트 꺼내기
        {
            Poolable poolable;

            if (_poolStack.Count > 0) // Stack에 Pool이 존재하면
                poolable = _poolStack.Pop(); // 꺼내기
            else // 존재하지 않으면
                poolable = Create(); // Pool 생성

            poolable.gameObject.SetActive(true); // Pool 오브젝트 활성화

            // DontDestroyOnLoad 해제 용도
            if (parent == null) // Root 연결이 끊어지면
                poolable.transform.parent = Managers.Scene.CurrentScene.transform; // Root를 Scene으로 변경해서 DontDestroyOnLoad에서 빠져나온다

            poolable.transform.parent = parent; // 인자로 받은 parent 내부로 이동
            poolable.IsUsing = true; // Poolable 사용중

            return poolable; // 리턴
        }
    }
    #endregion

    Dictionary<string, Pool> _pool = new Dictionary<string, Pool>(); // 이름, 오브젝트 형식으로 저장된 전체 Pool
    Transform _root; // @Pool_Root
    public void Init() // PoolManager 초기화
    {
        if (_root == null)
        {
            _root = new GameObject { name = "@Pool_Root" }.transform; // PoolManager 의 Root 생성
            Object.DontDestroyOnLoad(_root); // Pool 은 삭제되지 않는다
        }
    }

    public void CreatePool(GameObject original, int count = 5) // Pool 오브젝트 생성(원본, Pool 개수(Count))
    {
        Pool pool = new Pool(); // Pool 객체 생성
        pool.Init(original, count); // Pool 초기화
        pool.Root.parent = _root; // @Pool_Root 를 부모로 지정

        _pool.Add(original.name, pool); // Dic에 추가(원본이름, Pool)
    }

    public void Push(Poolable poolable)
    {
        string name = poolable.gameObject.name; // Poolable 오브젝트 이름
        if (_pool.ContainsKey(name) == false) // Dic에 Poolable 오브젝트가 존재하지 않으면
        {
            GameObject.Destroy(poolable.gameObject); // 입력받은 Poolable 오브젝트 제거
            return;
        }

        _pool[name].Push(poolable); // Dic의 name 위치에 Poolable 오브젝트 추가
    }

    public Poolable Pop(GameObject original, Transform parent = null)
    {
        if (_pool.ContainsKey(original.name) == false) // 원본 오브젝트 Root가 없으면
            CreatePool(original); // 생성

        return _pool[original.name].Pop(parent); // 원본 오브젝트 Root 를 꺼내기
    }

    public GameObject GetOriginal(string name) // 원본 오브젝트를 가져오기
    {
        if (_pool.ContainsKey(name) == false)
            return null;
        return _pool[name].Original;
    }

    public void Clear() // Pool 전체 제거
    {
        foreach (Transform child in _root)
            GameObject.Destroy(child.gameObject);

        _pool.Clear();
    }
}
