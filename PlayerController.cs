using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] public float rotationSpeed = 2f;

    private CharacterController characterController;
    private float verticalVelocity;
    private bool isGrounded;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerActions = GetComponent<PlayerActions>();

    }

    private void Update()
    {
        HandleMovement();
        HandleInput();
        HandleRotation();
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
        // 입력 로그
        if(Input.GetKeyDown(KeyCode.F))
        {
            playerActions.PickUpItem(); // F키: 아이템 줍기
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            playerActions.OpenDoor(); // E키: 문 열기
        }

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            playerActions.OpenInventory(); // Tab: 인벤토리창 열기
        }

        if(Input.GetMouseButton(0))
        {
            playerActions.UseItem(); // 왼쪽 마우스 버튼: 아이템 사용
        }
    }

    private void HandleRotation()
    {
        if (Input.GetMouseButton(1)) // 오른쪽 마우스 버튼
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            transform.Rotate(Vector3.up, mouseX);
        }
    }

}
