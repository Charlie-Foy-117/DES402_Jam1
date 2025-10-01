using Unity.VisualScripting;
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
    [SerializeField] private Vector3 startPoint;
    [SerializeField] private float camToRest = 0.01f; //set the threshold when the camera should return to 0y 

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
            return new Vector3(transform.position.x, playerRef.transform.position.y, transform.position.z);
        }
    }

    private void UpdateCamera()
    {
        if (target.y > startPoint.y)
        {
            transform.position = Vector3.Lerp(transform.position, target, trackSpeed * Time.deltaTime);
            //Debug.Log("Camera " + camID.ToString() + ": Tracking player");
        }
        else if (target.y < startPoint.y && transform.position.y > camToRest)
        {
            transform.position = Vector3.Lerp(transform.position, startPoint, trackSpeed * Time.deltaTime);
            //Debug.Log("Camera " + camID.ToString() + ":Returning to StartPoint");
        }
        else if (transform.position.y <= camToRest)
        {
            transform.position = startPoint;
            //Debug.Log("Camera " + camID.ToString() + ":At Rest");
        }
        else if (transform.position.y == startPoint.y)
        {
            //do nothing
        }
        else
        {
            Debug.LogWarning("Camera " + camID.ToString() + ": Tracking Nothing");
        }
    }

    private void Update()
    {
        UpdateCamera();
    }
}
