using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    private Coroutine wanderCoroutine;
    private Animator animator;
    private MeshRenderer meshRenderer;
    private NavMeshAgent agent;
    private Camera mainCamera;

    private MonsterState currentState = MonsterState.Inactive; // 현재상태를 비활성상태로 지정

    // 플레이어의 위치
    public Transform player;

    // 이동속도 설정
    float wanderSpeed = 1.0f;       // 배회할 때 이동속도
    float chaseSpeed = 2.0f;        // 추격할 때 이동속도
    float rotationSpeed = 5.0f;     // 회전 속도
    float chaseDistance = 5f;      // 추격 시작 거리
    float attackRange = 2f;       // 공격 거리
    float attackCooldown = 2f;      // 공격 간격
    bool canAttack = true;

    // 몬스터 동작 상태
    enum MonsterState
    {
        Inactive,   // 아직 활성화되지 않음
        Wandering,  // 배회 상태
        Chasing,     // 플레이어 추격 상태
        Attacking   // 플레이어 공격
    }

    void Start()
    {
        CleanupMemory();
        // 만약 상태 초기화나 다른 로직이 필요하다면 여기서 진행
        agent.SetDestination(transform.position);
        agent.isStopped = false; // 초반에는 이동을 멈춘 상태로 설정
        agent.speed = wanderSpeed;
        ActivateMonster();
    }

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        agent = GetComponent<NavMeshAgent>(); // NavMeshAgent 가져오기
        animator = GetComponent<Animator>();
        mainCamera = Camera.main; // 메인 카메라 가져오기
    }

    /// 몬스터를 활성화하여 배회 상태로 전환한다.
    public void ActivateMonster()
    {
        currentState = MonsterState.Wandering;
        agent.isStopped = false;
        agent.speed = wanderSpeed;
        // 코루틴이 이미 실행중이면 Stop하고 다시 실행
        if (wanderCoroutine != null)
        {
            StopCoroutine(wanderCoroutine);
        }
        wanderCoroutine = StartCoroutine(WanderRoutine());
    }

    /// 몬스터를 플레이어 추격 상태로 전환한다.
    public void EnableChase()
    {
        currentState = MonsterState.Chasing;
        agent.speed = chaseSpeed;

        if (wanderCoroutine != null)
        {
            StopCoroutine(wanderCoroutine);
            wanderCoroutine = null;
        }
    }

    void Update()
    {
        if (player == null || mainCamera == null) return;
        // 몬스터가 카메라 시야 안에 있는지 확인
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(transform.position);
        bool isVisible = viewportPos.x > 0 && viewportPos.x < 1 && viewportPos.y > 0 && viewportPos.y < 1 && viewportPos.z > 0;
        // 플레이어와의 거리 확인
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        // 시야 안에 있고, 추격 거리 내에 있을 경우
        if (currentState != MonsterState.Chasing && isVisible && distanceToPlayer < chaseDistance)
        {
            Debug.Log("추격시작");
            EnableChase();
        }
        if (currentState == MonsterState.Chasing)
        {
            Chase();
        }
        if (currentState == MonsterState.Chasing && distanceToPlayer < attackRange && canAttack)
        {
            currentState = MonsterState.Attacking;
            Attack();
        }

        if (Time.time % 300 == 0) // 5분마다 실행
        {
            CleanupMemory();
        }
    }

    /// 배회 행동 로직 (코루틴 사용) / 추가해야될 내용) 장애물 회피 , 일정 범위 내에서 활동
    private IEnumerator WanderRoutine()
    {
        // 회전 속도와 회전 각도 제한 (필요에 따라 값 조절)
        float maxAngularDeviation = 45f;  // 현재 진행 방향 기준 ±45도 내에서 회전

        while (currentState == MonsterState.Wandering)
        {
            // 현재 회전 값에서 ±45도 범위 내의 작은 각도 변경을 계산합니다.
            float randomOffset = Random.Range(-maxAngularDeviation, maxAngularDeviation);
            Quaternion targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + randomOffset, 0);

            // 부드럽게 회전하기 위해 일정 시간 동안 Slerp로 보간합니다.
            float rotationTime = Random.Range(0.5f, 1f); // 회전하는 데 걸리는 시간
            float elapsed = 0f;
            while (elapsed < rotationTime)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, elapsed / rotationTime);
                elapsed += Time.deltaTime;
                yield return null;
            }
            // 보간 후 정확히 목표 회전값으로 맞춥니다.
            transform.rotation = targetRotation;

            // 현재 진행 방향을 기준으로 무작위 이동 거리 계산
            Vector3 randomDirection = transform.forward * Random.Range(2f, 5f);
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position + randomDirection, out hit, 5f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }

            // 이동 시간과 도착 후 대기 시간을 설정합니다.
            // yield return new WaitForSeconds(Random.Range(2f, 4f)); // 이동 중 대기
            yield return new WaitForSeconds(Random.Range(0f, 0.5f)); // 이동 후 대기
        }
    }

    /// 플레이어를 향해 추격하는 로직
    void Chase()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance > attackRange) //거리가 정지거리보다 멀면 추격 실행
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
                Debug.Log("몬스터가 플레이어 추격");

                Vector3 direction = (player.position - transform.position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            else
            {
                Debug.Log("플레이어에게 도착! 공격 시작!");
                agent.isStopped = true;
            }
        }
    }

    void Attack()
    {
        Debug.Log("공격");
        if (canAttack && player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance <= attackRange)
            {
                Debug.Log("플레이어를 공격");
                // 애니메이션 실행
                animator.SetTrigger("AttackTrigger");

                // 플레이어에게 데미지 적용
                // player.GetComponent<PlayerHealth>().TakeDamge(10);

                canAttack = false;
                StartCoroutine(AttackCooldown()); // 공격후 쿨타임임
            }
            else
            {
                currentState = MonsterState.Chasing; // 공격 범위 밖이면 다시 추격
            }
        }

        // 공격 애니메이션 및 데미지 적용
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    void CleanupMemory()
    {
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
        Debug.Log("불필요한 메모리 정리 완료");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            // 충돌 면의 첫번째 접촉 정보를 사용해 반사 방향 계산
            Vector3 reflectDir = Vector3.Reflect(transform.forward, collision.contacts[0].normal);
            // 새로운 목적지: 반사 방향으로 일정 거리(0~1 단위) 이동한 위치
            Vector3 newDestination = transform.position + reflectDir.normalized * Random.Range(3f, 5f);
            NavMeshHit hit;
            if (NavMesh.SamplePosition(newDestination, out hit, 5f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
            // 즉시 회전으로 새 방향 적용 (부드럽게 하고 싶다면 Slerp 사용 가능)
            transform.rotation = Quaternion.LookRotation(reflectDir);
            Debug.Log("벽 충돌: 방향 전환");
        }
    }

}