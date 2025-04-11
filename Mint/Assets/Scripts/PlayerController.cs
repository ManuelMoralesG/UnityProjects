using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D RB;
    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    private float x;
    private bool isGrounded;
    private bool jumpRequested;

    void Start()
    {
        RB = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxisRaw("Horizontal");

        if (Input.GetButton("Jump") && isGrounded) {
            jumpRequested = true;
            Debug.Log("Jump!");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) {
            isGrounded = true;
            Debug.Log("Grounded");
        }
    }

    void FixedUpdate()
    {
        RB.linearVelocity = new Vector2(playerSpeed * x, RB.linearVelocityY);

        if (jumpRequested) {
            RB.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            isGrounded = false;
            jumpRequested = false;
        }

    }
}
