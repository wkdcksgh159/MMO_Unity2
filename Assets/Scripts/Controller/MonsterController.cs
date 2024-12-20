using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : BaseController
{
    Stat _stat;

    [SerializeField]
    float _scanRange = 10; // 플레이어 스캔 거리

    [SerializeField]
    float _attackRange = 2; // 공격 범위

    public override void Init()
    {
        WorldObjectType = Define.WorldObject.Monster;
        _stat = gameObject.GetComponent<Stat>(); // 스탯 컴포넌트를 가져온다

        if (gameObject.GetComponentInChildren<UI_HPBar>() == null) // UI_HPBar 가 없으면 생성
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
    }

    protected override void UpdateIdle() // 대기
    {
        // TODO : 매니저가 생기면 옮기자
        //GameObject player = GameObject.FindGameObjectWithTag("Player"); // Player 태그를 가진 오브젝트 탐색
        GameObject player = Managers.Game.GetPlayer(); // GameManagerEx 의 플레이어 객체를 불러온다
        if (player == null) // 없으면 종료
            return;

        float distance = (player.transform.position - transform.position).magnitude; // 플레이어와 몬스터의 거리
        if (distance <= _scanRange) // 스캔 범위내면
        {
            _lockTarget = player; // 타게팅 락온
            State = Define.State.Moving; // 타겟으로 이동
            return;
        }
    }

    protected override void UpdateMoving() // 이동
    {
        // 플레이어가 내 사정거리보다 가까우면 공격
        if (_lockTarget != null)
        {
            _destPos = _lockTarget.transform.position; // 도착지점(타겟)
            float distance = (_destPos - transform.position).magnitude; // 도착지점까지의 거리
            if (distance <= _attackRange) // 공격범위에 들어오면
            {
                NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>(); // 길찾기 API NavMeshAgent
                nma.SetDestination(transform.position); // 길찾기 도착지점을 자기자신으로 설정(길찾기 정지)
                State = Define.State.Skill; // 공격 실행
                return;
            }
        }

        // 이동
        Vector3 dir = _destPos - transform.position; // 도착 위치에서 현재 위치를 뺀 방향 벡터 생성
        // 도착 확인(vector 끼리의 연산의 경우 0으로 딱 맞아 떨어지는 경우는 거의 없기 때문에 아주 작은 값으로 확인)
        if (dir.magnitude < 0.1f)
        {
            State = Define.State.Idle; // 도착하면 대기 상태
        }
        else
        {
            NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>(); // 길찾기 API NavMeshAgent
            nma.SetDestination(_destPos); // 목적지까지 이동하는 함수
            nma.speed = _stat.MoveSpeed; // 이동속도 지정

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime); // 부드러운 회전
        }
    }

    protected override void UpdateSkill()
    {
        Debug.Log("Monster UpdateSkill");
    }

    void OnHitEvent() // 공격 적중 이벤트
    {
        if (_lockTarget != null)
        {
            // 체력
            PlayerStat targetStat = _lockTarget.GetComponent<PlayerStat>(); // 타겟 스탯 호출
            targetStat.OnAttacked(_stat);
            /*int damage = Mathf.Max(0, _stat.Attack - targetStat.Defense); // 플레이어의 공격력 - 타겟의 방어력을 데미지로 변환(값이 0 이상)
            targetStat.Hp -= damage; // 타겟의 HP를 감소한다*/

            if (targetStat.Hp <= 0)
            {
                //GameObject.Destroy(targetStat.gameObject);
                //Managers.Game.Despawn(targetStat.gameObject);
            }

            if (targetStat.Hp > 0) // 체력이 있으면
            {
                float distance = (_lockTarget.transform.position - transform.position).magnitude; // 타겟과의 거리를 구한다
                if (distance <= _attackRange) // 공격범위에 들어오면
                    State = Define.State.Skill; // 공격
                else // 공격범위에 들어오지 않으면
                    State = Define.State.Moving; // 이동
            }
            else // 체력이 없으면
            {
                State = Define.State.Idle; // 대기
            }
        }
        else // 타겟이 없으면
        {
            State = Define.State.Idle; // 대기
        }
    }
}
