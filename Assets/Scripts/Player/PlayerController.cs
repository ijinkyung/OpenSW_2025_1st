using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float moveSpeed = 5f; // 기본 이동 속도
    [SerializeField] public float rotationSpeed = 2f; // 마우스 감도(좌우/상하 회전)
    [SerializeField] private Transform cameraTransform; // 카메라 Transform

    private CharacterController characterController; // 물리 충돌 및 이동 처리용
    private float verticalVelocity; // y축 중력값 저장용
    private bool isGrounded; // 바닥에 있는지 여부
    private GameObject currentFuse; // Fuse object

    private float xRotation = 0f; // 카메라 상하 회전 각도


    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        // 마우스 잠금 및 커서 숨기기 (중앙에 고정)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    private void Update()
    {
        HandleMovement(); // 이동처리
        HandleInput(); // 입력 처리
        HandleRotation(); // 마우스 회전 처리(시야)
    }

    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput);
        movement = transform.TransformDirection(movement);

        float currentSpeed = moveSpeed;

        // Shift 키로 달리기
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed *= 1.5f;
        }

        movement *= currentSpeed;

        // 중력 처리
        isGrounded = characterController.isGrounded;
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += Physics.gravity.y * Time.deltaTime;
        movement.y = verticalVelocity;

        characterController.Move(movement * Time.deltaTime);
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (currentFuse != null)
            {
                FuseItem fuseItem = currentFuse.GetComponent<FuseItem>();
                if (fuseItem != null)
                {
                    PuzzleManager.Instance.CollectFuse(fuseItem); 
                    currentFuse.SetActive(false);
                    currentFuse = null;
                }
            }
        }
    }

    private void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed; // 좌우 회전
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed; // 상하 회전

        // 좌우: 플레이어 오브젝트 회전
        transform.Rotate(Vector3.up * mouseX);

        // 상하: 카메라만 회전
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Fuse"))
            {
                currentFuse = other.gameObject;
            }   
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == currentFuse)
            {
                currentFuse = null;
            }
        }
}
