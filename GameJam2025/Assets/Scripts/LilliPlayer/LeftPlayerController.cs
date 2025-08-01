using UnityEngine;

public class LeftPlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 3f;
    private Vector2 moveDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal Left");
        float vertical = Input.GetAxisRaw("Vertical Left");

        // Move on only one axis
        if (horizontal != 0)
        {
            vertical = 0;
        }

        moveDirection = new Vector2(horizontal, vertical).normalized;
        transform.Translate(moveSpeed * Time.deltaTime * moveDirection);
    }
}
