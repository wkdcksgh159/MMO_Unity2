using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value> // 키, 값 형태로 값을 받기 위해 인터페이스 선언
{
    Dictionary<Key, Value> MakeDict(); // Dictionary 를 리턴하는 함수 선언
}

public class DataManager
{
    public Dictionary<int, Data.Stat> StatDict { get; private set; } = new Dictionary<int, Data.Stat>(); // JSON 데이터를 외부에서 사용하기 위한 Dic

    public void Init()
    {
        StatDict = LoadJson<Data.StatData, int, Data.Stat>("StatData").MakeDict(); // JSON 파일 -> StatData -> StatDict 순서로 데이터를 넣는다
    }

    // JSON 파일을 불러와서 리스트에 넣는 함수 선언
    // 키, 값 형식으로 받아서 Dictionary에 넣기 위해 ILoader 인터페이스를 상속받은 클래스만 사용한다
    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value> // ILoader 를 상속받은 클래스만 사용
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}"); // StatData 데이터를 불러온다
        return JsonUtility.FromJson<Loader>(textAsset.text); // JSON 데이터 형식에 맞춰서 선언된 클래스를 호출해서 데이터를 저장
    }
}
