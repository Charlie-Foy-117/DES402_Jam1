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
    public string[] act1Dialogue; //Need to update with a more efficient method
    public string[] act2Dialogue; 
    public string[] act3Dialogue; 
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
        string[] actDialogue = new string[1];

        if (dialogueActVal == 0) { actDialogue = act1Dialogue; }
        else if (dialogueActVal == 1) { actDialogue = act2Dialogue; }
        else if (dialogueActVal == 2) { actDialogue = act3Dialogue; }
        else { Debug.LogWarning("Outside of act index"); }
            
        if (lineIndex < actDialogue.Length)
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

        dialogueText_Player.text = act1Dialogue[lineToDisplay];
    }

    public void UpdateNPCDialgoue(int lineToDisplay)
    {
        dialogueText_Player.gameObject.SetActive(false);
        dialogueText_NPC.gameObject.SetActive(true);

        dialogueText_NPC.text = act1Dialogue[lineToDisplay];
    }

    public void SetPlayerTextObject(TextMeshProUGUI playerTextObject)
    {
        dialogueText_Player = playerTextObject;
    }
}
