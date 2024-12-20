using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginScene : BaseScene
{
    protected override void Init()
    {
        base.Init(); // BaseScene 초기화

        SceneType = Define.Scene.Login; // 씬 타입 변경(Login)
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //SceneManager.LoadScene("Game"); // 유니티 자체 씬 매니저를 통해 Game 씬으로 이동
            Managers.Scene.LoadScene(Define.Scene.Game); // SceneManagerEx 를 통해 Game 씬으로 이동
        }
    }

    public override void Clear()
    {
        Debug.Log("LoginScene Clear!"); // Clear 실행 확인
    }
}
