using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Input
    private Vector2[] playerAxisInput = new Vector2[4];
    private bool[] m_AnyPlayerInput;

    public AudioClip playerButtonPressClip;
    public AudioClip playerButton2PressClip;

    private int[] buttonASuccessiveInputs;
    private int[] buttonBSuccessiveInputs;

    private PlayerInput.InputManager inputManager;
    private PlayerManager playerManager;

    private void Awake()
    {
        inputManager = GetComponent<PlayerInput.InputManager>();
        if (inputManager == null)
        {
            inputManager = Instantiate<PlayerInput.InputManager>(Resources.Load<PlayerInput.InputManager>("InputManager"));
        }

        playerManager = GetComponent<PlayerManager>();
        //playerManager.Initialise(this);
    }

    private void Start()
    {
        inputManager.InitialiseActions(new PlayerInput.InputActions
        {
            APressed = OnStartJump,
            ADown = OnJumping,
            AReleased = OnJumpFinished,

            //BPressed = ,
            //BDown = ,
            //BReleased = ,

            DirectionInput = OnDirectionalInput
        });
    }
    public void OnDirectionalInput(int playerIndex, Vector2 direction)
    {
        playerAxisInput[playerIndex] = new Vector2(direction.x,1 );
        playerManager.players[playerIndex].OnDirectionalInput(direction);
    }

    public void OnStartJump(int playerIndex)
    {
        playerManager.players[playerIndex].OnStartJump();
    }

    public void OnJumping(int playerIndex)
    {

    }

    public void OnJumpFinished(int playerIndex)
    {

    }
}
