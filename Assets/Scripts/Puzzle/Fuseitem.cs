using UnityEngine;

public class FuseItem : MonoBehaviour
{
    public string fuseID = "Fuse"; // 필요시 특정 퓨즈 분기 가능

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PuzzleManager.Instance.CollectFuse(this);  // 퍼즐 매니저에 전달
            Destroy(gameObject);  // 퓨즈 수집 후 사라짐
        }
    }
}