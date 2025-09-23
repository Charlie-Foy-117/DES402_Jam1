using UnityEngine;
using PlayerInput;

public enum PlayerButtons
{
    Up,
    Down,
    Left,
    Right,
    L_Button,
    R_Button
};

namespace PlayerInput
{
    public struct InputActions
    {
        public System.Action<int> APressed;
        public System.Action<int> ADown;
        public System.Action<int> AReleased;

        public System.Action<int> BPressed;
        public System.Action<int> BDown;
        public System.Action<int> BReleased;

        public System.Action<int, Vector2> DirectionInput;
    }

    public static class GameJam1Input
    {
        private static PlayerInput.InputManager inputManager;
        public static void Initialise(PlayerInput.InputManager inMan)
        {
            if (inputManager == null)
            {
                inputManager = inMan;
            }
        }

        public static bool GetButtonDown(int PlayerID, PlayerButtons btn)
        {
            if (PlayerID > 3)
            {
                Debug.LogError("PlayerID is " + PlayerID + ". It must be from 0-3.");
                return false;
            }
            return inputManager.GetButtonDown(PlayerID, btn);
        }
        public static bool GetButtonUp(int PlayerID, PlayerButtons btn)
        {
            if (PlayerID > 3)
            {
                Debug.LogError("PlayerID is " + PlayerID + ". It must be from 0-3.");
                return false;
            }
            return inputManager.GetButtonUp(PlayerID, btn);
        }
        public static bool GetButton(int PlayerID, PlayerButtons btn)
        {
            if (PlayerID > 3)
            {
                Debug.LogError("PlayerID is " + PlayerID + ". It must be from 0-3.");
                return false;
            }
            return inputManager.GetButton(PlayerID, btn);
        }
    }

    public class InputManager : MonoBehaviour
    {
        [SerializeField] private PlayerInputController[] playerInput;

        public bool GetButtonDown(int player, PlayerButtons button)
        {
            return playerInput[player].GetButtonDown(button);
        }
        public bool GetButtonUp(int player, PlayerButtons button)
        {
            return playerInput[player].GetButtonUp(button);
        }
        public bool GetButton(int player, PlayerButtons button)
        {
            return playerInput[player].GetButton(button);
        }

        public void InitialiseActions(InputActions actions)
        {
            for (int i = 0; i < playerInput.Length; i++)
            {
                playerInput[i].SetListeners(actions, i);
            }
        }
       
        private void Update()
        {
            for (int i = 0; i < playerInput.Length; i++)
            {
                playerInput[i].Update();
            }
        }
    }
}
