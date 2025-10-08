using System;
using UnityEngine;
using UnityEngine.UI;

public class WeightUI : MonoBehaviour
{
    [SerializeField] private Slider weightSlider;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private Player player;
    [SerializeField] int maxW;
    [SerializeField] int minW;
    [SerializeField] private int screenID;

    private void Start()
    {
        if (playerManager == null) { playerManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerManager>(); }
        if (player == null) { player =  playerManager.players[screenID]; }
        weightSlider.maxValue = maxW;
        weightSlider.minValue = minW;
    }
    private void Update()
    {
        UpdateSlider();
    }

    private void UpdateSlider()
    {
        weightSlider.value = player.GetWeight() * 10;
    }
}
