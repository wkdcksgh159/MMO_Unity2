using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    // BaseScene에서 Awake 로 Init 을 실행하기 때문에 GameScene에선 작성하지 않는다
    protected override void Init()
    {
        base.Init(); // BaseScene Init 실행
        SceneType = Define.Scene.Game; // 씬 타입 변경(Game)
        Managers.UI.ShowSceneUI<UI_Inven>(); // UI_Inven 생성
        Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;
        gameObject.GetOrAddComponent<CursorController>();

        GameObject player = Managers.Game.Spawn(Define.WorldObject.Player, "UnityChan"); // UnityChan 객체 생성
        Camera.main.gameObject.GetOrAddComponent<CameraController>().SetPlayer(player); // 카메라가 가리키는 플레이어(UnityChan) 세팅
        //Managers.Game.Spawn(Define.WorldObject.Monster, "Knight"); // Knight 객체 생성

        GameObject go = new GameObject { name = "SpawningPool" }; // SpawningPool 오브젝트 생성
        SpawningPool pool = go.GetOrAddComponent<SpawningPool>(); // SpawningPool 클래스 컴포넌트 추가
        pool.SetKeepMonsterCount(5); // 몬스터 개체 수 지정
    }

    public override void Clear()
    {

    }
}
