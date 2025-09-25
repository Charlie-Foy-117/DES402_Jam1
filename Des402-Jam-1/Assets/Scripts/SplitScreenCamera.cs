using UnityEngine;

public class SplitScreenCamera : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private PlayerManager playerManager;
    private Player playerRef;

    [Header("Settings")]
    [SerializeField] private int camID;
    [SerializeField] private float trackSpeed;
    [SerializeField] private Vector2 trackBegin;
    [SerializeField] private Vector2 startPoint;

    private void Awake()
    {
        if (playerManager == null)
        {
            GameObject gameManager = GameObject.FindWithTag("GameManager");
            playerManager = gameManager.GetComponent<PlayerManager>();
        }
        if (playerRef == null)
        {
            foreach (Player player in playerManager.players)
            {
                if (player.playerID == camID)
                {
                    playerRef = player;
                }
            }    
        }
    }

    private void Start()
    {
        startPoint = transform.position;
    }

    private Vector3 target
    {
        get
        {
            return new Vector3(transform.position.x, playerRef.transform.position.y, 0);
        }
    }

    private void Update()
    {
        if (target.y >= startPoint.y)
        {
            transform.position = Vector3.Lerp(transform.position,target, trackSpeed * Time.deltaTime);
        }
    }
}
