using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat // Stat 을 상속받는다
{
    [SerializeField]
    protected int _exp;
    [SerializeField]
    protected int _gold;

    public int Exp // 경험치와 레벨 property
    { 
        get { return _exp; } 
        set 
        { 
            _exp = value;
            // 레벨업 체크

            int level = Level; // 상위 클래스 Stat 의 Level 을 가져온다
            while (true)
            {
                Data.Stat stat; // StatData.json 파일의 내용을 불러온다 
                if (Managers.Data.StatDict.TryGetValue(level + 1, out stat) == false) // 상위 level 의 스탯 데이터가 존재하는지 확인
                    break;
                if (_exp < stat.totalExp) // 플레이어의 현재 경험치가 레벨업의 필요 경험치량보다 낮은 경우 넘어감
                    break;
                level++; // 레벨 업
            }

            if (level != Level) // 레벨 값이 다르면(레벨업을 했다면)
            {
                Debug.Log("Level Up!");
                Level = level; // Stat의 Level 값을 변경된 level 값으로 전달받고
                SetStat(Level); // 변경된 Level의 Stat 데이터값으로 다시 입력
            }
        } 
    }
    public int Gold { get { return _gold; } set { _gold = value; } }

    private void Start()
    {
        _level = 1;
        _exp = 0;
        _defense = 5;
        _moveSpeed = 5.0f;
        _gold = 0;

        SetStat(_level);
    }

    public void SetStat(int level)
    {
        Dictionary<int, Data.Stat> dict = Managers.Data.StatDict; // StatData.json 를 가져온다
        Data.Stat stat = dict[level]; // 레벨을 입력받고 해당 레벨에 해당하는 스탯 정보를 받는다
        _hp = stat.maxHp;
        _maxHp = stat.maxHp;
        _attack = stat.attack;
    }

    protected override void OnDead(Stat attacker)
    {
        Debug.Log("Player Dead");
    }
}
