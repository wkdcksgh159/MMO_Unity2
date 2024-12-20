using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : BaseController
{
    int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster); // 레이어 시프트 연산

    PlayerStat _stat; // 플레이어 스탯
    bool _stopSkill = false;

    public override void Init()
    {
        WorldObjectType = Define.WorldObject.Player;
        _stat = gameObject.GetComponent<PlayerStat>(); // 플레이어 스탯 컴포넌트를 가져온다
        Managers.Input.MouseAction -= OnMouseEvent; // 중복 호출 방지
        Managers.Input.MouseAction += OnMouseEvent; // 마우스 이벤트 등록
        if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
    }

    protected override void UpdateMoving() // 이동 상태
    {
        // 몬스터가 내 사정거리보다 가까우면 공격
        if (_lockTarget != null)
        {
            _destPos = _lockTarget.transform.position;
            float distance = (_destPos - transform.position).magnitude;
            if (distance <= 1)
            {
                State = Define.State.Skill;
                return;
            }
        }

        // 이동
        Vector3 dir = _destPos - transform.position; // 도착 위치에서 현재 위치를 뺀 방향 벡터 생성
        dir.y = 0; // y값 초기화(오브젝트 위에 올라타기 방지)

        // 도착 확인(vector 끼리의 연산의 경우 0으로 딱 맞아 떨어지는 경우는 거의 없기 때문에 아주 작은 값으로 확인)
        if (dir.magnitude < 0.1f)
        {
            State = Define.State.Idle; // 도착하면 대기 상태
        }
        else
        {
            /*// TODO
            NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>(); // 길찾기 API NavMeshAgent
            // 이동 거리 계산
            // Clamp 함수의 경우 (값, min, max) 를 인자로 받아서 값이 min 보다 작아지면 min 으로 max 보다 커지면 max 로 지정한다
            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            // nma.CalculatePath
            // 길찾기 API를 활용해 이동
            nma.Move(dir.normalized * moveDist); // transform.position 처럼 정확한 위치에 이동하는것이 아니기 때문에 도착 확인을 0.1f 로 수정*/

            // 이동 불가한 경우 멈춤
            Debug.DrawRay(transform.position, dir.normalized, Color.green);// 레이캐스팅 디버깅
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block"))) // 정면으로 Raycast 를 쏴서 Block 물체와 부딪힌 경우
            {
                if (Input.GetMouseButton(0) == false)
                    State = Define.State.Idle; // Player 상태를 대기상태로 전환
                return;
            }

            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            transform.position = transform.position + dir.normalized * moveDist; // 도착 지점으로 물체 이동
            //transform.LookAt(_destPos); // 도착 지점으로 방향 전환
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }
    }

    protected override void UpdateSkill()
    {
        if (_lockTarget != null)
        {
            Vector3 dir = _lockTarget.transform.position - transform.position; // 타겟의 방향 벡터
            Quaternion quat = Quaternion.LookRotation(dir); // 타겟을 바라본다
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime); // 모션을 부드럽게 연결
        }
    }

    void OnRunEvent(int a)
    {
        Debug.Log($"뚜벅 뚜벅~~{a}");
    }

    void OnRunEvent(string a)
    {
        Debug.Log($"뚜벅 뚜벅~~{a}");
    }

    void OnHitEvent()
    {
        Debug.Log("OnHitEvent");

        if (_lockTarget != null)
        {
            // TODO
            Stat targetStat = _lockTarget.GetComponent<Stat>(); // 타겟 스탯 호출
            targetStat.OnAttacked(_stat);

            /*PlayerStat myStat = gameObject.GetComponent<PlayerStat>(); // 플레이어 스탯 호출
            int damage = Mathf.Max(0, myStat.Attack - targetStat.Defense); // 플레이어의 공격력 - 타겟의 방어력을 데미지로 변환(값이 0 이상)
            targetStat.Hp -= damage; // 타겟의 HP를 감소한다*/
        }

        // TODO
        if (_stopSkill)
        {
            State = Define.State.Idle;
        }
        else
        {
            State = Define.State.Skill;
        }
    }

    /*void OnKeyboard() // 키보드 입력 발생 시 실행할 플레이어 이동 코드
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.2f);
            transform.position += Vector3.forward * Time.deltaTime * _speed;
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.2f);
            transform.position += Vector3.back * Time.deltaTime * _speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.2f);
            transform.position += Vector3.left * Time.deltaTime * _speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.2f);
            transform.position += Vector3.right * Time.deltaTime * _speed;
        }

        _moveToDest = false;
    }*/

    void OnMouseEvent(Define.MouseEvent evt) // MouseEvent 종류를 매개변수로 전달
    {
        switch (State)
        {
            case Define.State.Idle:
                OnMouseEvent_IdleRun(evt);
                break;
            case Define.State.Moving:
                OnMouseEvent_IdleRun(evt);
                break;
            case Define.State.Skill:
                {
                    if (evt == Define.MouseEvent.PointerUp) // 드래그 해제시 공격 중지
                        _stopSkill = true;
                }
                break;
        }
    }

    void OnMouseEvent_IdleRun(Define.MouseEvent evt)
    {
        if (State == Define.State.Die) // 죽음 상태인 경우 종료
            return;

        RaycastHit hit; // Raycasting 으로 확인된 물체의 정보를 저장
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // 현재 마우스 위치값을 Ray 데이터 타입으로 변환한다
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, _mask); // 레이캐스팅 체크
        //Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f); // 메인 카메라 에서 100.0f 길이만큼 빨간색 레이저를 1초간 쏜다

        switch (evt)
        {
            case Define.MouseEvent.PointerDown: // 첫 클릭시
                {
                    if (raycastHit) // 레이캐스팅 체크시
                    {
                        _destPos = hit.point; // 클릭 지점
                        State = Define.State.Moving; // 이동 상태로 변경
                        _stopSkill = false;

                        if (hit.collider.gameObject.layer == (int)Define.Layer.Monster) // Monster 레이어라면
                            _lockTarget = hit.collider.gameObject; // 락온할 타게팅 지정
                        else
                            _lockTarget = null; // 타게팅 해제
                    }
                }
                break;
            case Define.MouseEvent.Press: // 드래그
                {
                    if (_lockTarget == null && raycastHit) // 락온된 타겟이 없고 레이캐스팅이 확인되면
                        _destPos = hit.point; // 해당 위치로 이동
                }
                break;
            case Define.MouseEvent.PointerUp: // 드래그 해제시 공격 중지
                _stopSkill = true;
                break;
        }
    }
}
