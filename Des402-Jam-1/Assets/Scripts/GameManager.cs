using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    //Input
    private Vector2[] playerAxisInput = new Vector2[4];
    private bool[] m_AnyPlayerInput;

    public AudioClip playerButtonPressClip;
    public AudioClip playerButton2PressClip;

    private int[] buttonASuccessiveInputs;
    private int[] buttonBSuccessiveInputs;

    private bool[] inputBlocked;
    private bool[] portsFound;

    private bool verifiedPorts = false;

    private PlayerInput.InputManager inputManager;
    private PlayerManager playerManager;

    private void Awake()
    {
        inputManager = GetComponent<PlayerInput.InputManager>();
        if (inputManager == null)
        {
            inputManager = Instantiate<PlayerInput.InputManager>(Resources.Load<PlayerInput.InputManager>("InputManager"));
        }
        inputManager.OnPortsVerified += HandlePortVerification;

        buttonASuccessiveInputs = new int[4];
        buttonBSuccessiveInputs = new int[4];
        inputBlocked = new bool[4];
        portsFound = new bool[4];

        playerManager = GetComponent<PlayerManager>();
        //playerManager.Initialise(this);
        m_AnyPlayerInput = new bool[4];

        //Cursor.visible = false;
    }

    private void Start()
    {
        //GameAPI.ComPortLoader.SetActive(true);
        verifiedPorts = false;

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
        playerAxisInput[playerIndex] = new Vector2(direction.x, 0);
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

    public void HandlePortVerification(bool[] playerPortsFound)
    {
        for (int i = 0; i < 4; i++)
        {
            portsFound[i] = playerPortsFound[i];
            GameAPI.PlayPrompts[i].SetActive(portsFound[i]);
        }

        GameAPI.ComPortLoader.SetActive(false);
        verifiedPorts = true;
    }
}
