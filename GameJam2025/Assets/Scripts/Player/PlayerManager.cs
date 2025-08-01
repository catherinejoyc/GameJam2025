using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerStats playerStats;
    public PlayerUI playerUI;

    public void TakeDamage(float damage)
    {
        var newHealth = playerStats.health - damage;
        playerUI.UpdateHealth(newHealth);
        playerStats.health = newHealth < 0 ? 0 : newHealth;
        if (newHealth <= 0)
        {
            // Game Over
        }
    }

    public float DealDamage()
    {
        return playerStats.damage;
    }
}
