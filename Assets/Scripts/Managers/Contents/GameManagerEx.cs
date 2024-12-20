using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEx
{
    GameObject _player; // 플레이어 객체
    HashSet<GameObject> _monsters = new HashSet<GameObject>(); // 몬스터 객체

    public Action<int> OnSpawnEvent; // 몬스터 생성 이벤트를 받는다

    public GameObject GetPlayer() { return _player; }

    public GameObject Spawn(Define.WorldObject type, string path, Transform parent = null) // 플레이어 or 몬스터 생성
    {
        GameObject go = Managers.Resource.Instantiate(path, parent); // 생물체 오브젝트 생성

        switch (type)
        {
            case Define.WorldObject.Monster: // 몬스터 타입인 경우
                _monsters.Add(go); // 몬스터 객체 추가
                if (OnSpawnEvent != null) // 몬스터 생성 시 
                    OnSpawnEvent.Invoke(1); // 등록된 스폰 이벤트 실행(AddMonsterCount + 1)
                break;
            case Define.WorldObject.Player: // 플레이어 객체인 경우
                _player = go; // 플레이어 객체 입력
                break;
        }

        return go;
    }

    public Define.WorldObject GetWorldObjectType(GameObject go) // 생물체 타입 확인
    {
        BaseController bc = go.GetComponent<BaseController>(); // BaseController 의 WorldObject 타입 확인
        if (bc == null)
            return Define.WorldObject.Unknown;

        return bc.WorldObjectType;
    }

    public void Despawn(GameObject go) // 생물체 제거 
    {
        Define.WorldObject type = GetWorldObjectType(go); // 타입을 가져온다

        switch (type)
        {
            case Define.WorldObject.Monster: // 몬스터 타입인 경우
                {
                    if (_monsters.Contains(go)) // 해쉬셋에서 해당 객체를 찾고 제거
                    {
                        _monsters.Remove(go);
                        if (OnSpawnEvent != null) // 몬스터 제거 시 
                            OnSpawnEvent.Invoke(-1); // 등록된 이벤트를 실행한다(AddMonsterCount - 1)
                    }
                }
                break;
            case Define.WorldObject.Player: // 플레이어 타입인 경우
                {
                    if (_player == go) // 플레이어 객체의 참조를 null 로 변경
                    {
                        _player = null;
                    }
                }
                break;
        }

        Managers.Resource.Destroy(go); // 객체 제거 실행
    }
}
