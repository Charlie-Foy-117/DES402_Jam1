using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    const int NUM_PLAYERS = 4;
    const float MAX_IDLE_TIME = 20;
    const float timeToReset = 5;

    public Player[] players = new Player[4];

    [Header("Player State UI")]
    [SerializeField] GameObject[] playerIdleScreens = new GameObject[4];
    [SerializeField] GameObject[] playerCountdownScreens = new GameObject[4];
    TMPro.TMP_Text[] countdownText;

    GameManager gameManager;

    private Vector2[] curPos = new Vector2[4];
    private Vector2[] oldPos = new Vector2[4];

    private void Awake()
    {
        countdownText = new TMPro.TMP_Text[4];
        for (int i = 0; i < playerCountdownScreens.Length; i++)
        {
            string childName = "CountdownTimer" + i;
            countdownText[i] = playerCountdownScreens[i].transform.Find(childName).GetComponent<TMPro.TMP_Text>();
        }
    }

    private void Start()
    {
        StartCoroutine(CheckForIdling());
    }

    private void Update()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].idleTimer > MAX_IDLE_TIME) 
            {
                players[i].ChangePlayerState(Player.PlayerState.IDLE);
                players[i].idleTimer = 0.0f;
            }
            if (players[i].idleTimer >= MAX_IDLE_TIME - 5.0f && !countdownText[i].IsActive())
            {
                SetCountdownScreenVisibility(i, true);
            }
            if (!players[i].isIdling && players[i].state == Player.PlayerState.IDLE)
            {
                players[i].ChangePlayerState(Player.PlayerState.ACTIVE);
            }
        }
    }

    IEnumerator CheckForIdling()
    {
        for (int i = 0; i < players.Length; i++)
        {
            oldPos[i] = players[i].transform.position;
        }

        yield return new WaitForSeconds(2.0f);

        for (int i = 0; i < players.Length; i++)
        {
            curPos[i] = players[i].transform.position;

            if (players[i].state == Player.PlayerState.INTERACT){ continue; }
            else if (curPos[i] != oldPos[i] && players[i].state != Player.PlayerState.IDLE)
            {
                players[i].isIdling = true;
            }
        }

        StartCoroutine(CheckForIdling());
    }

    public void SetCountdownScreenVisibility(int index, bool visibility)
    {
        playerCountdownScreens[index].SetActive(visibility);
        players[index].showingCountdown = visibility;
    }

    public void SetIdleScreenVisibility(int index, bool visibility)
    {
        playerIdleScreens[index].SetActive(visibility);
        if (visibility)
        {
            players[index].PlayerReset();
        }
    }
}
