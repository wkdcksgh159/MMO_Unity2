using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance; // 유일성이 보장된다
    static Managers Instance { get { Init(); return s_instance; } } // 유일한 매니저를 갖고온다

    #region Contents
    GameManagerEx _game = new GameManagerEx();

    public static GameManagerEx Game { get { return Instance._game; } }
    #endregion

    #region Core
    DataManager _data = new DataManager();
    InputManager _input = new InputManager();
    PoolManager _pool = new PoolManager();
    ResourceManager _resource = new ResourceManager();
    SceneManagerEx _scene = new SceneManagerEx();
    SoundManager _sound = new SoundManager();
    UIManager _ui = new UIManager();
   
    public static DataManager Data { get { return Instance._data; } }
    public static InputManager Input { get { return Instance._input; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static UIManager UI { get { return Instance._ui; } }
    #endregion

    void Start()
    {
        Init();
    }

    void Update()
    {
        _input.OnUpdate();
    }

    static void Init()
    {
        if (s_instance == null)
        {
            // 초기화
            GameObject go = GameObject.Find("@Managers"); // 이름으로 GameObject 를 검색
            if (go == null) // 만약 Find로 찾지못하면 null이 반환됨
            {
                go = new GameObject { name = "@Managers" }; // 새로운 GameObject 생성
                go.AddComponent<Managers>(); // Managers 컴포넌트 추가
            }

            DontDestroyOnLoad(go); // go 가 삭제되지 않음(Scene 이 이동하더라도)
            s_instance = go.GetComponent<Managers>(); // 검색한 GameObject에서 Managers Component 를 호출할 수 있음

            s_instance._data.Init(); // DataManager 초기화
            s_instance._pool.Init(); // PoolManager 초기화
            s_instance._sound.Init(); // SoundManager 초기화
        }
    }

    public static void Clear()
    {
        Input.Clear();
        Sound.Clear();
        Scene.Clear();
        UI.Clear();

        Pool.Clear(); // 다른곳에서 Pool을 사용하고 있을 수 있기 때문에 마지막에 Clear
    }
}
