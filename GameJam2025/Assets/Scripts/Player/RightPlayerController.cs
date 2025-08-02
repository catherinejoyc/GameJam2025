using UnityEngine;

public class RightPlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 3f;
    private Vector2 moveDirection;

    private bool frozen = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void Freeze(bool freeze)
    {
        frozen = freeze;
    }

    // Update is called once per frame
    void Update()
    {
        if (frozen) return;
        float horizontal = Input.GetAxisRaw("Horizontal Right");
        float vertical = Input.GetAxisRaw("Vertical Right");

        // Move on only one axis
        if (horizontal != 0)
        {
            vertical = 0;
        }

        moveDirection = new Vector2(horizontal, vertical).normalized;
        transform.Translate(moveSpeed * Time.deltaTime * moveDirection);
    }

}
