using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    const int NUM_PLAYERS = 4;

    public Player[] players;

    GameManager gameManager;

   /* public void Initialise(GameManager gameManager)
    {
        players = new Player[NUM_PLAYERS];
        for (int i = 0; i < players.Length; ++i)
        {
            this.gameManager = gameManager;

            players[i] = new Player(); //fix this
            players[i].Initialise(i);
            players[i].gameManager = gameManager;
            players[i].playerManager = this;
        }
    }*/
}
