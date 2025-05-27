using UnityEngine;

// 처음상태 : Start()에서 문이 "닫힌상태"라고 간주하고 현재 회전값 저장
// 문열기 : ToggleDoor()를 호출하면 isOpen을 true로 변경 -> 문이 열림
// 문닫기 : ToggleDoor()를 호출하면 isOpen을 false로 변경 -> 문이 닫힘

public class DoorController : MonoBehaviour
{
    private Animator animator;
    public bool isOpen = false; // 문 열려 있는지의 여부 (true=열림, false=닫힘)
    public float openAngle = 90f; // 문 열림 각도
    public float speen = 2f; // 문 열림 속도

    private Quaternion closedRotation; // 닫힌 상태의 회전 값
    private Quaternion openedRotation; // 열린 상태의 회전 값 

    void Start()
    {
    closedRotation = transform.rotation; // 현재 초기 회전 값을 저장 -> 닫힌 상태로 간주
    openedRotation = Quaternion.Euler(transform.eulerAngles + Vector3.up * openAngle);  // 열린 상태는 Y축으로 openAngle만큼 회전
    animator = GetComponent<Animator>();
    }

    void Update()
    {
        Quaternion targetRotation = isOpen ? openedRotation : closedRotation; // 열림/닫힘 상태에 따라 목표 회전값 결정
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speen * Time.deltaTime); // Slerp로 부드럽게 회전
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen; // 열림/닫힘 상태를 토글
        animator.SetBool("isOpen", isOpen); // 애니메이션 트리거 설정
    }

}