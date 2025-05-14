using UnityEngine;

public class Monster : MonoBehaviour
{    
    // 플레이어의 Transform을 저장 (MonsterManager에서 할당)
    public Transform player;

    // 이동속도 설정
    public float wanderSpeed = 2.0f; // 배회할 때 이동속도
    public float chaseSpeed = 5.0f; // 추격할 때 이동속도

    // 몬스터 동작 상태
    private enum MonsterState
    {
        Inactive,   // 아직 활성화되지 않음
        Wandering,  // 배회 상태
        Chasing     // 플레이어 추격 상태
    }

    private MonsterState currentState = MonsterState.Inactive;


    /// 몬스터를 활성화하여 배회 상태로 전환한다.
    public void ActivateMonster()
    {
        currentState = MonsterState.Wandering;
        // 배회 초기화나 애니메이션 시작 로직 추가 가능
    }


    /// 몬스터를 플레이어 추격 상태로 전환한다.
    public void EnableChase()
    {
        currentState = MonsterState.Chasing;
        // 추격 관련 초기 설정(예: 경로 계산 등)을 넣을 수 있음
    }

    private void Update()
    {
        // 현재 상태에 따라 행동을 실행
        switch (currentState)
        {
            case MonsterState.Wandering:
                Wander();
                break;
            case MonsterState.Chasing:
                Chase();
                break;
            default:
                // Inactive 상태에서는 아무 행동도 하지 않음
                break;
        }
    }


    /// 배회 행동 로직 (여기서는 단순히 전진하는 예시)
    private void Wander()
    {
        //배회 로직 개선 (랜덤 이동, 일정 구역 내 이동 등)
        transform.Translate(Vector3.forward * wanderSpeed * Time.deltaTime);
    }


    /// 플레이어를 향해 추격하는 로직
    private void Chase()
    {
        if (player != null)
        {
            // 플레이어 방향 계산
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * chaseSpeed * Time.deltaTime;
        }
    }
}
