using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

    public int fuseCount = 0;
    public int requiredFuseCount = 3;

    public GameObject generator;     // 발전기 GameObject (애니메이션 or 파티클)
    public EscapeDoor escapeDoor;    // 탈출 문 제어 스크립트

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void CollectFuse(FuseItem fuse)
    {
        fuseCount++;
        Debug.Log($"퓨즈 획득: {fuseCount}/{requiredFuseCount}");

        if (fuseCount >= requiredFuseCount)
        {
            ActivateGenerator();
        }
    }

    public void ActivateGenerator()
    {
        Debug.Log("모든 퓨즈가 설치되었습니다! 발전기 작동.");
        if (generator != null)
            generator.SetActive(true);  // 이펙트 또는 사운드 연출

        if (escapeDoor != null)
            escapeDoor.OpenDoor();
    }
}