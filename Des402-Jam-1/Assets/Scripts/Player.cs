using System;
using System.Collections;
using TMPro;
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
    [SerializeField] private float jumpForce;
    private float startMoveSpeed;
    private float startJumpForce;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float weight = 0;
    [SerializeField] private int screenID;
    [SerializeField] private float coyoteTime;
    private float coyoteTimer;
    [SerializeField] private bool interactActive;
    public TextMeshProUGUI playerText;
    private Vector2 startPos;

    [Header("Player State")]
    public PlayerState state;
    public bool isIdling;
    public float idleTimer = 0.0f;
    public bool showingCountdown = false;
    private float countdownTimer;
    [SerializeField] private float countdownTimerMax;

    [Header("Player Refs")]
    public GameManager gameManager;
    public PlayerManager playerManager;
    public DialogueManager dialogueManager;
    [SerializeField] private SplitScreenCamera splitScreenCamera;
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
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        playerManager = gameManager.GetComponent<PlayerManager>();
        dialogueManager = gameManager.GetComponent<DialogueManager>();
        startPos = transform.position;
        startJumpForce = jumpForce;
        startMoveSpeed = moveSpeed;
    }

    private void Update()
    {
        if (isIdling && state == PlayerState.ACTIVE)
        {
            idleTimer += Time.deltaTime;
        }
        else { idleTimer = 0.0f; }

        if (isGrounded)
        {
            coyoteTimer = coyoteTime;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }

        if (showingCountdown && isIdling)
        {
            if (countdownTimer > 0) 
            { 
                countdownTimer -= Time.deltaTime; 
                playerManager.UpdateCountDownClock(playerID, countdownTimer);
            }
            else
            {
                countdownTimer = 5;
                playerManager.SetCountdownScreenVisibility(playerID, false);
            }
        }
        else
        {
            countdownTimer = 5;
            playerManager.SetCountdownScreenVisibility(playerID, false);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform") || other.gameObject.CompareTag("Floor") && !isGrounded)
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
        if (other.gameObject.CompareTag("Platform") && rb.linearVelocityY <= 0 && isGrounded)
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Collectable"))
        {
            Debug.Log("Collectable picked up");
            if (other.GetComponent<Collectable>() != null)
            {
                weight += other.GetComponent<Collectable>().GetWeight();
            }
            Destroy(other.gameObject);
            UpdateJumpForce();
        }
        if (other.gameObject.CompareTag("NPC"))
        {
            interactActive = true;
        }
        if (other.gameObject.CompareTag("ActTrigger"))
        {
            dialogueManager.npcList[playerID].UpdateAct();
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            interactActive = false;
            dialogueManager.LeaveDialogue(playerID);
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
        playerManager.SetCountdownScreenVisibility(playerID, false);
        state = PlayerState.ACTIVE;
    }

    private void OnInteractState()
    {
        state = PlayerState.INTERACT;
    }

    public void OnStartJump()
    {
        if (state != PlayerState.IDLE)
        {
            //Debug.Log("Player " + index + " jumped");
            if (isGrounded || coyoteTimer > 0)
            {
                isGrounded = false;
                rb.AddForce(jump * jumpForce, ForceMode2D.Impulse);
            }
        }
    }

    public void OnDirectionalInput(Vector2 direction)
    {
        if (state != PlayerState.IDLE)
        {
            //Debug.Log($"rb {( rb == null ? "null" : "not null")}");
            transform.position += moveSpeed * (Vector3)direction * Time.deltaTime;
            transform.position = ScreenUtility.ClampToScreen(transform.position, screenID, 0.3f);
        }
    }

    public void OnInteractPressed()
    {
        if (interactActive && state != PlayerState.IDLE)
        {
            ChangePlayerState(PlayerState.INTERACT);
            dialogueManager.UpdateDialogue(playerID);
            weight = 0;
            jumpForce = startJumpForce; 
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

    public void PlayerReset()
    {
        moveSpeed = startMoveSpeed;
        jumpForce = startJumpForce;
        isGrounded = true;
        weight = 0;
        interactActive = false;
        //ChangePlayerState(PlayerState.IDLE);
        isIdling = true;
        idleTimer = 0;
        splitScreenCamera.Reset();
        transform.position = startPos;
        dialogueManager.npcList[playerID].UpdateAct(0);
    }

    public float GetWeight()
    {
        return weight;
    }

    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
        jumpForce = speed;
    }

}
