using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Player : MonoBehaviour
{
    [HideInInspector]
    public enum PlayerState { IDLE, ACTIVE, INTERACT };

    [Header("Player Stats")]
    public int playerID = 0;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3 jump;
    [SerializeField] private float jumpForce = 2.0f;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float weight = 0;
    [SerializeField] private int screenID;
    [SerializeField] private float coyoteTime;
    [SerializeField] private bool interactActive;

    [Header("Player State")]
    public PlayerState state;
    public bool isIdling;
    public float idleTimer = 0.0f;
    public bool showingCountdown = false;

    [Header("Player Refs")]
    public GameManager gameManager;
    public PlayerManager playerManager;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    //public Player() { }

    public void Initialise(int index)
    {
        this.playerID = index;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        state = PlayerState.IDLE;
    }

    private void Update()
    {
        if (isIdling && state == PlayerState.ACTIVE)
        {
            idleTimer += Time.deltaTime;
        }
        else { idleTimer = 0.0f; }
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

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            StartCoroutine(CoyoteTimeCoroutine());
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
        if (other.gameObject.CompareTag("NPC"))
        {
            interactActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            interactActive = false;
        }
    }

    public void ChangePlayerState(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.IDLE:
                OnIdleState();
                break;
            case PlayerState.ACTIVE:
                OnActiveState();
                break;
            case PlayerState.INTERACT:
                OnInteractState();
                break;
        }
    }

    private void OnIdleState()
    {
        playerManager.SetIdleScreenVisibility(playerID, true);
        state = PlayerState.IDLE;
    }

    private void OnActiveState()
    {
        playerManager.SetIdleScreenVisibility(playerID, false);
        state = PlayerState.ACTIVE;
    }

    private void OnInteractState()
    {
        state = PlayerState.INTERACT;
    }

    IEnumerator CoyoteTimeCoroutine()
    {
        yield return new WaitForSeconds(coyoteTime);
        isGrounded = false;
    }

    public void OnStartJump()
    {
        if (state != PlayerState.IDLE)
        {
            //Debug.Log("Player " + index + " jumped");
            if (isGrounded)
            {
                rb.AddForce(jump * jumpForce, ForceMode2D.Impulse);
                isGrounded = false;
            }
        }
    }

    public void OnDirectionalInput(Vector2 direction)
    {
        if (state != PlayerState.IDLE)
        {
            //Debug.Log($"rb {( rb == null ? "null" : "not null")}");
            transform.position += moveSpeed * (Vector3)direction * Time.deltaTime;
            transform.position = ScreenUtility.ClampToScreen(transform.position, screenID, 0.5f);
        }
    }

    public void OnInteractPressed()
    {
        if (interactActive && state != PlayerState.IDLE)
        {
            ChangePlayerState(PlayerState.INTERACT);
            Debug.Log("InteractWorking");
        }
    }

    private void UpdateJumpForce()
    {
        if (state != PlayerState.IDLE)
        {
            float jumpMultiplier = 1 - weight;
            jumpForce = jumpForce * jumpMultiplier;
        }
    }
}
