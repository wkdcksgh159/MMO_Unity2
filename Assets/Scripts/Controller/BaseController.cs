using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    [SerializeField]
    protected Vector3 _destPos; // 도착 지점 위치

    [SerializeField]
    protected Define.State _state = Define.State.Idle; // Player 상태를 받는 변수 선언 및 초기화

    [SerializeField]
    protected GameObject _lockTarget; // 타게팅 락온

    public Define.WorldObject WorldObjectType { get; protected set; } = Define.WorldObject.Unknown;

    public virtual Define.State State
    {
        get { return _state; }
        set
        {
            _state = value;

            Animator anim = GetComponent<Animator>();
            switch (_state)
            {
                case Define.State.Die:
                    break;
                case Define.State.Idle:
                    anim.CrossFade("WAIT", 0.1f); // 애니메이션 종료 0.1초 전 다음 애니메이션 연결
                    break;
                case Define.State.Moving:
                    anim.CrossFade("RUN", 0.1f);
                    break;
                case Define.State.Skill:
                    anim.CrossFade("ATTACK", 0.1f, -1, 0); // 종료 시 처음부터 다시 실행(loop)
                    break;
            }
        }
    }

    private void Start()
    {
        Init();
    }

    void Update()
    {
        switch (State)
        {
            case Define.State.Die: // 죽음 상태
                UpdateDie();
                break;
            case Define.State.Moving: // 이동 상태
                UpdateMoving();
                break;
            case Define.State.Idle: // 대기 상태
                UpdateIdle();
                break;
            case Define.State.Skill:
                UpdateSkill();
                break;
        }
    }

    public abstract void Init();
    protected virtual void UpdateDie() { }
    protected virtual void UpdateMoving() { }
    protected virtual void UpdateIdle() { }
    protected virtual void UpdateSkill() { }
}
