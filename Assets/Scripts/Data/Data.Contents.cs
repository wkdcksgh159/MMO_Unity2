using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    // 관리하기 편하도록 region으로 묶는다
    #region Stat
    [Serializable] // 파일에서 가져온 데이터를 저장할 때 사용
    public class Stat
    {
        public int level; // StatData.json 이름과 형식에 맞춰서 선언, public이거나 SerializeField를 붙이거나 해야한다
        public int maxHp;
        public int attack;
        public int totalExp;
    }

    [Serializable] // json 등의 파일에서 가져온 데이터를 저장할 때 사용
    public class StatData : ILoader<int, Stat> // 키, 값 형태로 값을 받기 위해 ILoader 인터페이스 상속
    {
        public List<Stat> stats = new List<Stat>(); // StatData.json 형식에 맞춰서 선언(List)

        public Dictionary<int, Stat> MakeDict() // 인터페이스 함수 구현
        {
            Dictionary<int, Stat> dict = new Dictionary<int, Stat>();
            foreach (Stat stat in stats) // JSON을 통해 전달된 데이터 stats을 Dictionary에 넣는다
                dict.Add(stat.level, stat);
            return dict;
        }
    }
    #endregion
}