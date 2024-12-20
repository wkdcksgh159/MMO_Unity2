using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster); // 레이어 시프트 연산
    Texture2D _attackIcon;
    Texture2D _handIcon;

    enum CursorType
    {
        None,
        Attack,
        Hand,
    }

    CursorType _cursorType = CursorType.None;

    void Start()
    {
        _attackIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Attack"); // Attack 텍스쳐 호출
        _handIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Hand"); // Hand 텍스쳐 호출
    }

    void Update()
    {
        if (Input.GetMouseButton(0)) // 좌클릭중이면 커서 변경하지 않음
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // 현재 마우스 위치값을 Ray 데이터 타입으로 변환한다

        RaycastHit hit; // Raycasting 으로 확인된 물체의 정보를 저장
        if (Physics.Raycast(ray, out hit, 100.0f, _mask)) // 마우스로 클릭한 방향(dir)에 레이저를 쏜다, mask 로 확인된 레이어만 적용
        {
            if (hit.collider.gameObject.layer == (int)Define.Layer.Monster) // Monster 레이어라면
            {
                if (_cursorType != CursorType.Attack) // 불필요한 커서 변경을 피하기 위해 Attack 이 아닐 경우만 실행
                {
                    Cursor.SetCursor(_attackIcon, new Vector2(_attackIcon.width / 5, 0), CursorMode.Auto); // 커서 텍스쳐 변경(텍스쳐, 마우스 기준 커서 텍스쳐를 표시할 위치, 하드웨어 별 최적화)
                    _cursorType = CursorType.Attack; // enum 변경
                }
            }
            else
            {
                if (_cursorType != CursorType.Hand) // 불필요한 커서 변경을 피하기 위해 Hand 가 아닐 경우만 실행
                {
                    Cursor.SetCursor(_handIcon, new Vector2(_handIcon.width / 3, 0), CursorMode.Auto);
                    _cursorType = CursorType.Hand;
                }
            }
        }
    }
}
