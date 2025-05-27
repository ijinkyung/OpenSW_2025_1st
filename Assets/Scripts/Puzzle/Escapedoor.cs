using UnityEngine;

public class EscapeDoor : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OpenDoor()
    {
        if (animator != null)
        {
            animator.SetTrigger("Open");
            Debug.Log("탈출문이 열립니다!");
        }
    }
}