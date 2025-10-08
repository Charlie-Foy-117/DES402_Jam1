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
    [SerializeField] private string[] act1Dialogue; //Need to update with a more efficient method
    [SerializeField] private string[] act2Dialogue;
    [SerializeField] private string[] act3Dialogue;
    [SerializeField] private string[] currentDialogue;
    public int lineIndex = 0;
    private bool previousLineNPC;
    [SerializeField] private TextMeshProUGUI dialogueText_NPC;
    [SerializeField] private TextMeshProUGUI dialogueText_Player;

    [Header("Location")]
    [SerializeField] private Transform[] dialoguePositions; //positions for different story acts
    [SerializeField] private Vector2 currentPos;
    [SerializeField] private int dialogueActVal = 0; //decides what location index npc should be at depending on current story act
    
    private void Start()
    {
        GameManager gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        playerManager = gameManager.GetComponent<PlayerManager>();

        //currentPos = dialoguePositions[dialogueActVal].position;
        dialogueText_NPC.text = act1Dialogue[lineIndex];
        previousLineNPC = false;
        dialogueText_Player.gameObject.SetActive(false);
        dialogueText_NPC.gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
           prompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
           prompt.SetActive(false);
        }
    }

    public void NextLine()
    {
        if (prompt.activeInHierarchy) { prompt.SetActive(false); }

        if (dialogueActVal == 0) { currentDialogue = act1Dialogue; }
        else if (dialogueActVal == 1) { currentDialogue = act2Dialogue; }
        else if (dialogueActVal == 2) { currentDialogue = act3Dialogue; }
        else { Debug.LogWarning("Outside of act index"); }
        

        if (lineIndex < currentDialogue.Length)
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
        else if (dialogueActVal == 2)
        {
            playerManager.EndDemo(npcID);
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

    private void UpdatePlayerDialgoue(int lineToDisplay)
    {
        dialogueText_NPC.gameObject.SetActive(false);
        dialogueText_Player.gameObject.SetActive(true);

        dialogueText_Player.text = currentDialogue[lineToDisplay];
    }

    private void UpdateNPCDialgoue(int lineToDisplay)
    {
        dialogueText_Player.gameObject.SetActive(false);
        dialogueText_NPC.gameObject.SetActive(true);

        dialogueText_NPC.text = currentDialogue[lineToDisplay];
    }

    private void UpdateNPCPosition(int currentAct)
    {
        transform.position = new Vector2(transform.position.x, dialoguePositions[currentAct].position.y);
    }

    public void UpdateAct()
    {
        dialogueActVal++;
        UpdateNPCPosition(dialogueActVal);
    }
    public void UpdateAct(int currentAct)
    {
        UpdateNPCPosition(currentAct);
    }

    public void SetPlayerTextObject(TextMeshProUGUI playerTextObject)
    {
        dialogueText_Player = playerTextObject;
    }

    public void DisableText()
    {
        previousLineNPC = false;
        dialogueText_Player.gameObject.SetActive(false);
        dialogueText_NPC.gameObject.SetActive(false);
    }
}
