using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } } // 현재 씬을 BaseScene 타입으로 불러온다

    public void LoadScene(Define.Scene type)
    {
        Managers.Clear(); // 씬 이동 시 불필요한 데이터 정리
        SceneManager.LoadScene(GetSceneName(type)); // 입력받은 인자를 통해 Scene 호출
    }

    string GetSceneName(Define.Scene type)
    {
        string name = System.Enum.GetName(typeof(Define.Scene), type); // enum 으로 선언한 씬 이름을 불러온다
        return name;
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}
