using UnityEngine;

public class Monster : MonoBehaviour
{
    // 에셋스토어에서 임포트한 텍스처를 할당하기 위한 변수
    public Texture monsterTexture;

    private void Start()
    {
        // 텍스처 적용: 만약 다른 초기화 작업이 있다면 같이 진행해도 됩니다.
        ApplyTexture();

        // 만약 상태 초기화나 다른 로직이 필요하다면 여기서 진행
    }

    /// <summary>
    /// MeshRenderer를 사용해 monsterTexture를 머티리얼에 적용하는 메서드
    /// </summary>
    private void ApplyTexture()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        if (meshRenderer != null && monsterTexture != null)
        {
            // Standard 셰이더를 사용하는 머티리얼 생성
            Material monsterMaterial = new Material(Shader.Find("Standard"));
            monsterMaterial.mainTexture = monsterTexture;
            meshRenderer.material = monsterMaterial;
        }
        else
        {
            Debug.LogWarning("MeshRenderer 또는 monsterTexture가 할당되지 않았습니다.");
        }
    }
    
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

    /// <summary>
    /// 몬스터를 활성화하여 배회 상태로 전환한다.
    /// </summary>
    public void ActivateMonster()
    {
        currentState = MonsterState.Wandering;
        // 배회 초기화나 애니메이션 시작 로직 추가 가능
    }

    /// <summary>
    /// 몬스터를 플레이어 추격 상태로 전환한다.
    /// </summary>
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

    /// <summary>
    /// 배회 행동 로직 (여기서는 단순히 전진하는 예시)
    /// </summary>
    private void Wander()
    {
        // TODO: 배회 로직 개선 (랜덤 이동, 일정 구역 내 이동 등)
        transform.Translate(Vector3.forward * wanderSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 플레이어를 향해 추격하는 로직
    /// </summary>
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