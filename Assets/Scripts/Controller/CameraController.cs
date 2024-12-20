using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] // private 변수의 값을 유니티에서 수정하기 위해 사용
    Define.CameraMode _mode = Define.CameraMode.QuarterView; // 카메라 모드 지정(QuarterView)

    [SerializeField]
    Vector3 _delta = new Vector3(0.0f, 6.0f, -5.0f); // 카메라 위치 지정

    [SerializeField]
    GameObject _player = null; // Player 객체 지정

    public void SetPlayer(GameObject player) { _player = player; }

    void Start()
    {
        
    }

    void LateUpdate() // 유니티 로직 순서 상 Update 함수가 실행된 뒤에 실행함
    {
        if (_mode == Define.CameraMode.QuarterView) // QuarterView 모드 세팅
        {
            if (_player.IsValid() == false )
            {

                return;
            }
            RaycastHit hit;
            if (Physics.Raycast(_player.transform.position, _delta, out hit, _delta.magnitude, 1 << (int)Define.Layer.Block)) // player 에서 Wall 으로 Raycasting
            {
                float dist = (hit.point - _player.transform.position).magnitude * 0.8f; // 벽과 플레이어 사이의 거리에서 0.8을 곱해 벽을 지나도록
                transform.position = _player.transform.position + _delta.normalized * dist; // 카메라 위치를 벽 앞으로 이동
            }
            else
            {
                transform.position = _player.transform.position + _delta; // 현재 Player 캐릭터의 위치 + 변한 위치
                transform.LookAt(_player.transform); // 현재 Player 캐릭터의 위치를 바라본다
            }
        }
    }

    public void SetQuarterView(Vector3 delta) // QuarterView 세팅
    {
        _mode = Define.CameraMode.QuarterView;
        _delta = delta;
    }
}
