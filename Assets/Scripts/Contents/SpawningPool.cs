using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawningPool : MonoBehaviour
{
    [SerializeField]
    int _monsterCount = 0; // 몬스터 수
    int _reserveCount = 0; // 생성이 예약된 몬스터 수

    [SerializeField]
    int _keepMonsterCount = 0; // 최대 몬스터 수

    [SerializeField]
    Vector3 _spawnPos = new Vector3(0, 0, 0); // 몬스터 생성 위치
    [SerializeField]
    float _spawnRadius = 15.0f; // 몬스터 생성 범위
    [SerializeField]
    float _spawnTime = 5.0f; // 몬스터 생성 시간

    public void AddMonsterCount(int value) { _monsterCount += value; } // 몬스터 카운트 증가
    public void SetKeepMonsterCount(int count) { _keepMonsterCount = count; } // 최대 몬스터 수 세팅

    void Start()
    {
        Managers.Game.OnSpawnEvent -= AddMonsterCount; // 중복 방지
        Managers.Game.OnSpawnEvent += AddMonsterCount; // 몬스터 수 증가 이벤트 목록에 추가
    }

    void Update() // 프레임마다 실행
    {
        while (_reserveCount + _monsterCount < _keepMonsterCount) // 예약 + 현재 몬스터수가 최대 수를 미만이면
        {
            StartCoroutine("ReserveSpawn"); // spawnTime을 지키기 위해 코루틴으로 몬스터 생성 예약 실행
        }
    }

    IEnumerator ReserveSpawn() // 몬스터 생성 예약(특정 시간 후 실행)
    {
        _reserveCount++; // 예약 수 증가
        yield return new WaitForSeconds(Random.Range(0, _spawnTime)); // 0 부터 _spawnTime 사이의 숫자를 랜덤하게 뽑아서 기다림(고정 시간 스폰 방지)
        GameObject obj = Managers.Game.Spawn(Define.WorldObject.Monster, "Knight"); // 몬스터 생성
        NavMeshAgent nma = obj.GetOrAddComponent<NavMeshAgent>(); // 길찾기 AI

        Vector3 randPos; // 랜덤 위치 벡터
        while (true) // 랜덤 위치를 지정해서 생성할 수 있는 위치에만 생성
        {
            // insideUnitSphere : 특정 범위내에서 랜덤한 위치 지정
            Vector3 randDir = Random.insideUnitSphere * Random.Range(0, _spawnRadius); // 특정 범위 내의 위치 지정
            randDir.y = 0; // y값 초기화(땅 뚫기 방지)
            randPos = _spawnPos + randDir; // 랜덤 스폰 위치 지정

            // 갈 수 있나
            NavMeshPath path = new NavMeshPath(); // 길찾기 경로
            if (nma.CalculatePath(randPos, path)) // randPos 으로 이동 가능한지 확인
                break; // 가능하면 종료
        }

        obj.transform.position = randPos; // 몬스터 위치 지정
        _reserveCount--; // 예약 종료
    }


}
