using UnityEngine;

// 아이템 획득 플로우
// 1. 플레이어가 아이템을 근처에 있는지 확인 -> PlayerController파일 OnTrigerStay 함수
// 2. 아이템을 획득 -> PlayerActions파일 PickUpItem 함수
// 3. 아이템을 삭제
// 4. 아이템을 획득한 리스트에 추가 -> items[]에 아이템 추가 & hasItem[]에 true로 변경

// 아이템 사용 플로우
// 1. 플레이어가 아이템을 근처에 있는지 확인 -> PlayerController파일 OnTrigerStay 함수
// 2. 아이템을 사용 -> PlayerActions파일 UseItem 함수
// 3. 아이템을 사용한 리스트에서 제거 -> items[]에서 아이템 제거 & hasItem[]에 false로 변경

public class PlayerActions : MonoBehaviour
{
    // 아이템 변수
    public GameObject[] items; // 아이템 리스트
    public bool[] hasItem; // 획득한 아이템 리스트
    public GameObject nearObject;   // 현재 근처에 있는 오브젝트 

    // 아이템 획득 함수
    public void PickUpItem()
    {
        // 아이템 줍기
        if (nearObject != null && nearObject.CompareTag("Item"))
        {
            Item item = nearObject.GetComponent<Item>();
            int itemIndex = item.value;
            hasItem[itemIndex] = true; // 아이템 획득
            items[itemIndex] = nearObject;


            Destroy(nearObject); // 아이템 삭제
            nearObject = null; // 참조 초기화
        }
    }

    // 아이템 사용 함수
    public void UseItem()
    {
        if (nearObject != null && nearObject.CompareTag("Item"))
        {
            // 예시: 특정 아이템을 가지고 있어야 사용 가능 
            int requiredItemIndex = 0; // 예시: 0번 아이템
            if (hasItem[requiredItemIndex])
            {
                // 아이템 사용 로직
                Debug.Log("아이템 사용");

                // 아이템 사용 후 제거
                hasItem[requiredItemIndex] = false;
                items[requiredItemIndex] = null;

                nearObject = null; // 참조 초기화               
            }
            else
            {
                Debug.Log("아이템이 없습니다.");
            }
        }

    }

    public void OpenDoor()
    {
        if (nearObject != null && nearObject.CompareTag("Door"))
        {
            DoorController door = nearObject.GetComponent<DoorController>();
            if (door != null)
            {
                door.ToggleDoor(); // 문 열기
                nearObject = null; // 참조 초기화
            }
        }
    }
}

// public void OpenInventory() => Debug.Log("인벤토리 열기");
    
