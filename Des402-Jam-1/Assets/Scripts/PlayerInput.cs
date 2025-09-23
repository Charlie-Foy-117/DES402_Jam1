using UnityEngine;
using UnityEngine.InputSystem;

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

    [System.Serializable]
    public class ControllerInputs
    {
        public string A;
        public string B;

        public string Vertical;
        public string Horizontal;
    }
    [System.Serializable]
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

        public KeyboardInputs Keyboard;
        public ControllerInputs Controller;
        public COMInputs COM;

        private int PlayerID = -1;

        private SingleComPortController Comtroller;

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

        public void SetComtroller(ref SingleComPortController masterComtroller)
        {
            Comtroller = masterComtroller;
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
            //TODO: This could be nicer
            State.A = Input.GetKey(Keyboard.A) ||
                Input.GetButton(Controller.A) ||
                GetCOMState(COM.A);
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

            State.B = Input.GetKey(Keyboard.B) ||
                Input.GetButton(Controller.B) ||
                GetCOMState(COM.B);
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


            Vector2 Dir = new Vector2((Input.GetKey(Keyboard.Left) ? -1 : 0) + (Input.GetKey(Keyboard.Right) ? 1 : 0)
                + Input.GetAxis(Controller.Horizontal)
                + (GetCOMState(COM.Left) ? -1 : 0) + (GetCOMState(COM.Right) ? 1 : 0),

                (Input.GetKey(Keyboard.Up) ? 1 : 0) + (Input.GetKey(Keyboard.Down) ? -1 : 0)
                + Input.GetAxis(Controller.Vertical)
               + (GetCOMState(COM.Up) ? 1 : 0) + (GetCOMState(COM.Down) ? -1 : 0)).normalized;
            State.Direction = Dir;
            if (onDirectionInput != null)
            {
                onDirectionInput(PlayerID, Dir);
            }
        }

        bool GetCOMState(int keyValue)
        {
            return Comtroller.ButtonHeld(keyValue);
        }
    }
}
