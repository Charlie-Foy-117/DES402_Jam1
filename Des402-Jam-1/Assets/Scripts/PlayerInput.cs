using UnityEngine;

namespace PlayerInput
{
    [System.Serializable]
    public class KeyboardInputs
    {
        public KeyCode A;
        public KeyCode B;

        public KeyCode Up;
        public KeyCode Down;
        public KeyCode Left;
        public KeyCode Right;
    }
    public class COMInputs
    {
        public int A;
        public int B;

        public int Up;
        public int Down;
        public int Left;
        public int Right;
    }
    public struct InputState
    {
        public bool A;
        public bool B;
        public Vector2 Direction;
    }

    [System.Serializable]
    public class PlayerInputController
    {
        public System.Action<int> onButtonAPressedEvents;
        public System.Action<int> onButtonADownEvents;
        public System.Action<int> onButtonAReleasedEvents;

        public System.Action<int> onButtonBPressedEvents;
        public System.Action<int> onButtonBDownEvents;
        public System.Action<int> onButtonBReleasedEvents;

        public System.Action<int, Vector2> onDirectionInput;

        public KeyboardInputs keyboard;
        public COMInputs COM;

        private int PlayerID = -1;

        private InputState State;
        private InputState LastState;

        public bool GetButtonDown(PlayerButtons btn)
        {
            switch (btn)
            {
                case PlayerButtons.Up:
                    return (State.Direction.y > 0 && Mathf.Approximately(LastState.Direction.y, 0.0f));
                case PlayerButtons.Down:
                    return (State.Direction.y < 0 && Mathf.Approximately(LastState.Direction.y, 0.0f));
                case PlayerButtons.Left:
                    return (State.Direction.x < 0 && Mathf.Approximately(LastState.Direction.x, 0.0f));
                case PlayerButtons.Right:
                    return (State.Direction.x > 0 && Mathf.Approximately(LastState.Direction.x, 0.0f));

                case PlayerButtons.L_Button:
                    return (State.A && !LastState.A);
                case PlayerButtons.R_Button:
                    return (State.B && !LastState.B);
                default:
                    return false;
            }
        }
        public bool GetButtonUp(PlayerButtons btn)
        {
            switch (btn)
            {
                case PlayerButtons.Up:
                    return (LastState.Direction.y > 0 && Mathf.Approximately(State.Direction.y, 0.0f));
                case PlayerButtons.Down:
                    return (LastState.Direction.y < 0 && Mathf.Approximately(State.Direction.y, 0.0f));
                case PlayerButtons.Left:
                    return (LastState.Direction.x < 0 && Mathf.Approximately(State.Direction.x, 0.0f));
                case PlayerButtons.Right:
                    return (LastState.Direction.x > 0 && Mathf.Approximately(State.Direction.x, 0.0f));

                case PlayerButtons.L_Button:
                    return (LastState.A && !State.A);
                case PlayerButtons.R_Button:
                    return (LastState.B && !State.B);
                default:
                    return false;
            }
        }
        public bool GetButton(PlayerButtons btn)
        {
            switch (btn)
            {
                case PlayerButtons.Up:
                    return State.Direction.y > 0;
                case PlayerButtons.Down:
                    return State.Direction.y < 0;
                case PlayerButtons.Left:
                    return State.Direction.x < 0;
                case PlayerButtons.Right:
                    return State.Direction.x > 0;

                case PlayerButtons.L_Button:
                    return State.A;
                case PlayerButtons.R_Button:
                    return State.B;
                default:
                    return false;
            }
        }

        public void Initialise(int[] btns)
        {
            COM.A = btns[0];
            COM.B = btns[1];
            COM.Up = btns[2];
            COM.Down = btns[3];
            COM.Left = btns[4];
            COM.Right = btns[5];
        }
        public void SetListeners(InputActions actions, int playerID)
        {
            onButtonAPressedEvents += actions.APressed;
            onButtonADownEvents += actions.ADown;
            onButtonAReleasedEvents += actions.AReleased;

            onButtonBPressedEvents += actions.BPressed;
            onButtonBDownEvents += actions.BDown;
            onButtonBReleasedEvents += actions.BReleased;

            onDirectionInput += actions.DirectionInput;

            PlayerID = playerID;
        }

        public void Update()
        {
            LastState = State;
            State.A = Input.GetKey(keyboard.A);
            if (State.A && !LastState.A)
            {
                onButtonAPressedEvents?.Invoke(PlayerID);
            }
            else if (!State.A && LastState.A)
            {
                onButtonAReleasedEvents?.Invoke(PlayerID);
            }
            else if (State.A && LastState.A)
            {
                onButtonADownEvents?.Invoke(PlayerID);
            }

            State.B = Input.GetKey(keyboard.B);
            if (State.B && !LastState.B)
            {
                onButtonBPressedEvents?.Invoke(PlayerID);
            }
            else if (!State.B && LastState.B)
            {
                onButtonBReleasedEvents?.Invoke(PlayerID);
            }
            else if (State.B && LastState.B)
            {
                onButtonBDownEvents?.Invoke(PlayerID);
            }

            Vector2 Dir = new Vector2((Input.GetKey(keyboard.Left) ? -1 : 0) + (Input.GetKey(keyboard.Right) ? 1 : 0), 0);
            State.Direction = Dir;
            if (onDirectionInput != null)
            {
                onDirectionInput(PlayerID, Dir);
            }
        }
    }
}
