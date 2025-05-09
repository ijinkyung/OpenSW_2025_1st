using UnityEngine;
using System.Collections;

public class MonsterManager : MonoBehaviour
{
    // 몬스터 프리팹과 스폰 지점을 에디터에서 할당
    public GameObject monsterPrefab;
    public Transform spawnPoint;

    // 게임 시작 후 몬스터 생성 및 행동 시작 시간 (초 단위)
    private float spawnDelay = 180f; // 3분 후 몬스터 생성
    private float chaseDelay = 300f; // 5분 후 플레이어 추격 시작

    private GameObject monsterInstance;

    private void Start()
    {
        // 코루틴을 통해 시간에 따른 이벤트 실행
        StartCoroutine(InitializeMonster());
    }

    private IEnumerator InitializeMonster()
    {
        // 3분 대기 후 몬스터 생성
        yield return new WaitForSeconds(spawnDelay);
        monsterInstance = Instantiate(monsterPrefab, spawnPoint.position, spawnPoint.rotation);

        // Monster 스크립트를 가져오고, 플레이어 Transform 할당
        Monster monsterScript = monsterInstance.GetComponent<Monster>();
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            monsterScript.player = playerObject.transform;
        }
        // 몬스터를 배회 상태로 활성화
        monsterScript.ActivateMonster();

        // 추가 대기 (spawnDelay와 chaseDelay 사이의 시간)
        yield return new WaitForSeconds(chaseDelay - spawnDelay);
        // 이후 몬스터를 추격 상태로 전환
        monsterScript.EnableChase();
    }
}