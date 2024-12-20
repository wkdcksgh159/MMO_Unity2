using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager // 키보드 입력을 받는 클래스
{
    public Action KeyAction = null; // 이벤트 delegate(대리자)
    public Action<Define.MouseEvent> MouseAction = null; // Press 와 Click을 구분하기 위해 MouseEvent 를 인자로 받는다

    bool _pressed = false; // 마우스를 클릭했는지 확인
    float _pressedTime = 0; // 클릭 시간 확인

    public void OnUpdate()
    {
        if (EventSystem.current.IsPointerOverGameObject()) // 마우스 커서가 GameObject 위에 위치한 경우 이동하지 않음
            return;

        if (Input.anyKey && KeyAction != null) // Keyboard 입력 확인
            KeyAction.Invoke(); // 등록된 이벤트 실행

        if (MouseAction != null) // 등록된 마우스 이벤트가 있으면
        {
            if (Input.GetMouseButton(0)) // 마우스 왼쪽 클릭시
            {
                if (!_pressed) // 클릭중이 아니면
                {
                    MouseAction.Invoke(Define.MouseEvent.PointerDown); // 첫 클릭 이벤트 실행
                    _pressedTime = Time.time; // 클릭 시간 저장
                }

                MouseAction.Invoke(Define.MouseEvent.Press); // Press 로 등록된 마우스 이벤트 실행
                _pressed = true; // 마우스 클릭 확인
            }
            else
            {
                if (_pressed) // 마우스 클릭 확인시
                {
                    if (Time.time < _pressedTime * 0.2f) // 클릭 시간이 짧으면 클릭으로 인정
                        MouseAction.Invoke(Define.MouseEvent.Click); // Click 으로 등록된 마우스 이벤트 실행
                    MouseAction.Invoke(Define.MouseEvent.PointerUp); // 클릭 종료 이벤트 실행
                }
                _pressed = false; // 마우스 클릭 해제
                _pressedTime = 0; // 클릭 시간 초기화
            }
        }
    }

    public void Clear()
    {
        KeyAction = null;
        MouseAction = null;
    }
}