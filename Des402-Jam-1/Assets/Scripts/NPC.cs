using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class NPC : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private GameObject prompt;
    [SerializeField] private int npcID;
    [SerializeField] private PlayerManager playerManager;

    [Header("Dialogue")]
    public string[] dialogueLines; //Fix this 
    public int lineIndex = 0;
    private bool previousLineNPC;
    [SerializeField] private TextMeshProUGUI dialogueText_NPC;
    [SerializeField] private TextMeshProUGUI dialogueText_Player;
    
    private void Start()
    {
        GameManager gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        playerManager = gameManager.GetComponent<PlayerManager>();

        dialogueText_NPC.text = dialogueLines[lineIndex];
        previousLineNPC = false;
        dialogueText_Player.gameObject.SetActive(false);
        dialogueText_NPC.gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
           // prompt.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
           // prompt.SetActive(false);
        }
    }

    public void NextLine()
    {
        if (lineIndex < dialogueLines.Length)
        {
            if (previousLineNPC)
            {
                UpdatePlayerDialgoue(lineIndex);
                previousLineNPC = false;
            }
            else
            {
                UpdateNPCDialgoue(lineIndex);
                previousLineNPC = true;
            }
            lineIndex++;
        }
        else 
        { 
            playerManager.players[npcID].ChangePlayerState(Player.PlayerState.ACTIVE);
            dialogueText_Player.gameObject.SetActive(false);
            dialogueText_NPC.gameObject.SetActive(false);
            lineIndex = 0;
            previousLineNPC = false;
        }
    }

    public void UpdatePlayerDialgoue(int lineToDisplay)
    {
        dialogueText_NPC.gameObject.SetActive(false);
        dialogueText_Player.gameObject.SetActive(true);

        dialogueText_Player.text = dialogueLines[lineToDisplay];
    }

    public void UpdateNPCDialgoue(int lineToDisplay)
    {
        dialogueText_Player.gameObject.SetActive(false);
        dialogueText_NPC.gameObject.SetActive(true);

        dialogueText_NPC.text = dialogueLines[lineToDisplay];
    }

    public void SetPlayerTextObject(TextMeshProUGUI playerTextObject)
    {
        dialogueText_Player = playerTextObject;
    }
}
