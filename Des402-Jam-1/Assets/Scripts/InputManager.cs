using UnityEngine;
using PlayerInput;
using System.IO;

public enum PlayerButtons
{
    Up,
    Down,
    Left,
    Right,
    L_Button,
    R_Button
};

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


    public class InputManager : MonoBehaviour
    {
        [SerializeField] private PlayerInputController[] playerInput;
        private SingleComPortController Comtroller;

        public System.Action<bool[]> OnPortsVerified;
        private bool m_Initialised = false;

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

        public void Awake()
        {
            if (!m_Initialised)
            {
                Initialise(false);
            }
        }

        public void InitialiseActions(InputActions actions)
        {
            for (int i = 0; i < playerInput.Length; i++)
            {
                playerInput[i].SetListeners(actions, i);
            }
        }

        public void Initialise(bool debug)
        {
            GameJam1Input.Initialise(this);
            bool good = true;
            int[] btns = new int[24];
            string path = "";
#if !UNITY_EDITOR
            path = Application.dataPath + "/../../Buttons.txt";
#else
            path = Application.dataPath + "/Buttons.txt";
#endif
            string truePath = "";
            if (File.Exists(path))
            {
                truePath = path;
            }
            else if (File.Exists(Application.dataPath + "/../Buttons.txt"))
            {
                truePath = Application.dataPath + "/../Buttons.txt";
            }

            if (File.Exists(truePath))
            {
                var sr = new StreamReader(truePath);

                for (int i = 0; i < 24; i++)
                {
                    int res = -1;
                    good = int.TryParse(sr.ReadLine(), out res);

                    if (!good)
                        break;
                    else
                    {
                        if (res < 0 || res > 32)
                        {
                            good = false;
                            break;
                        }
                    }
                    btns[i] = res;
                }
                sr.Close();

            }
            else
            {
                good = false;
            }
            if (good)
            {
                Debug.Log("Inputs loaded successfully from " + truePath);
            }
            else
            {
                Debug.LogWarning("Inputs NOT loaded from " + truePath);
            }
            for (int i = 0; i < playerInput.Length; i++)
            {
                if (good)
                {
                    int[] bt = { btns[i * 6], btns[(i * 6) + 1], btns[(i * 6) + 2], btns[(i * 6) + 3], btns[(i * 6) + 4], btns[(i * 6) + 5] };
                    playerInput[i].Initialise(bt);
                }
                else
                {
                    print("Default input init");
                }
            }

            Comtroller = GetComponent<SingleComPortController>();
            for (int i = 0; i < playerInput.Length; i++)
            {
                playerInput[i].SetComtroller(ref Comtroller);
            }
            //TODO: Do this properly!

            bool[] playerPortsVerified = new bool[4] { true, true, true, true };
            if (OnPortsVerified != null)
            {
                OnPortsVerified(playerPortsVerified);
            }

            if (debug)
            {
                InputDebugger ID = Instantiate<InputDebugger>(Resources.Load<InputDebugger>("InputDebugger"));
                ID.comtroller = Comtroller;
            }

            m_Initialised = true;
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
