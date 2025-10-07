using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private PlayerManager playerManager;
    public NPC[] npcList = new NPC[4];

    private void Start()
    {
        for (int i = 0; i < playerManager.players.Length; i++)
        {
            npcList[i].SetPlayerTextObject(playerManager.players[i].playerText);
        }
    }
    public void UpdateDialogue(int playerIndex)
    {
        npcList[playerIndex].NextLine();
    }

    public void LeaveDialogue(int playerIndex)
    {
        npcList[playerIndex].lineIndex = 0;
    }

}
