using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;

    // Start 대신 사용할 수 있는 함수
    // Awake 사용 시 오브젝트가 비활성화 상태여도 실행하고,
    // 부모에서 Awake 사용 시 하위에 Start 함수등을 작성하지 않더라도 부모가 대신 실행한다
    void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem)); // EventSystem 오브젝트를 찾는다
        if (obj == null) // 없다면
            Managers.Resource.Instantiate("UI/EventSystem").name = "@EventSystem"; // 프리팹을 통해 @EventSystem 오브젝트 생성
    }

    public abstract void Clear();
}
