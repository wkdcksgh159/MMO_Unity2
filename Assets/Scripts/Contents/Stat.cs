using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [SerializeField]
    protected int _level;
    [SerializeField]
    protected int _hp;
    [SerializeField]
    protected int _maxHp;
    [SerializeField]
    protected int _attack;
    [SerializeField]
    protected int _defense;
    [SerializeField]
    protected float _moveSpeed;

    public int Level { get { return _level; } set { _level = value; } }
    public int Hp { get { return _hp; } set { _hp = value; } }
    public int MaxHp { get { return _maxHp; } set { _maxHp = value; } }
    public int Attack { get { return _attack; } set { _attack = value; } }
    public int Defense { get { return _defense; } set { _defense = value; } }
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }

    private void Start()
    {
        _level = 1;
        _hp = 100;
        _maxHp = 100;
        _attack = 10;
        _defense = 5;
        _moveSpeed = 5.0f;
    }

    public virtual void OnAttacked(Stat attacker)
    {
        int damage = Mathf.Max(0, attacker.Attack - Defense); // 플레이어의 공격력 - 타겟의 방어력을 데미지로 변환(값이 0 이상)
        Hp -= damage; // 타겟의 HP를 감소한다
        if (Hp < 0) // 체력이 0미만이면
        {
            Hp = 0; // 체력을 0으로 고정
            OnDead(attacker); // 캐릭터 사망 처리
        }
    }

    protected virtual void OnDead(Stat attacker) // 사망
    {
        Define.WorldObject type = Managers.Game.GetWorldObjectType(attacker.gameObject); // 공격자의 객체를 가져온다
        if (type == Define.WorldObject.Player) // 공격자가 플레이어라면
        {
            PlayerStat playerStat = attacker as PlayerStat; // 공격자의 스탯을 가져와서
            if (playerStat != null)
            {
                playerStat.Exp += 15; // 플레이어 경험치 추가
            }
        }
        Managers.Game.Despawn(gameObject); // 몬스터 사망
    }
}

