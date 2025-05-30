using UnityEngine;

public class DoorController : MonoBehaviour
{
    public float openAngle = 90f;  // 열린 상태의 회전 각도
    public float openSpeed = 2f;   // 열리는 속도
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private bool isOpen = false;
    private bool isMoving = false;

    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));
    }

    void Update()
    {
        if (isMoving)
        {
            Quaternion targetRotation = isOpen ? openRotation : closedRotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * openSpeed);

            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.rotation = targetRotation;
                isMoving = false;
            }
        }
    }

    public void ToggleDoor()
    {
        if (!isMoving)
        {
            isOpen = !isOpen;
            isMoving = true;
        }
    }

    // 충돌 감지로 동작 (선택사항)
    private void OnMouseDown()
    {
        ToggleDoor();
    }
}
