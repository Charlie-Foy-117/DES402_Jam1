using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private GameObject prompt;

    private void Start()
    {
        
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
}
