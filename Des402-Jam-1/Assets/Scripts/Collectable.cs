using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private float weight;

    public float GetWeight()
    {
        return weight;
    }
}
