using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI shieldText;
    public TextMeshProUGUI zoomText;

    public void UpdateCurrentStats(PlayerStats playerStats)
    {
        attackText.text = ":" + playerStats.damage;
        speedText.text = ":" + playerStats.speed;
        shieldText.text = ":" + playerStats.shield;
        zoomText.text = ":" + playerStats.zoom;
    }
}
