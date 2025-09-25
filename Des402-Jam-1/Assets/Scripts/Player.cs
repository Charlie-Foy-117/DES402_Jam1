using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Stats")]
    public int playerID = 0;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3 jump;
    [SerializeField] private float jumpForce = 2.0f;

    [SerializeField] private bool isGrounded;

    [Header("PlayerRefs")]
    public GameManager gameManager;
    public PlayerManager playerManager;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    [SerializeField] private float weight = 0;
    [SerializeField] private int screenID;

    //public Player() { }

    public void Initialise(int index)
    {
        this.playerID = index;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform") || other.gameObject.CompareTag("Floor"))
        {
            Vector3 normal = other.GetContact(0).normal;
            if (normal == Vector3.up)
            {
                isGrounded = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Collectable"))
        {
            Debug.Log("Collectable picked up");
            Destroy(other.gameObject);
            UpdateJumpForce();
        }
    }

    public void OnStartJump()
    {
        //Debug.Log("Player " + index + " jumped");
        if (isGrounded)
        {
            rb.AddForce(jump * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    public void OnDirectionalInput(Vector2 direction)
    {
        //Debug.Log($"rb {( rb == null ? "null" : "not null")}");
        transform.position += moveSpeed * (Vector3)direction * Time.deltaTime;
        transform.position = ScreenUtility.ClampToScreen(transform.position, screenID, 0.5f);
    }

    private void UpdateJumpForce()
    {
        float jumpMultiplier = 1 - weight;
        jumpForce = jumpForce * jumpMultiplier; 
    }
}
