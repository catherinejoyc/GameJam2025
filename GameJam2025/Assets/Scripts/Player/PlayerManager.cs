using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerStats playerStats;
    public PlayerUI playerUI;
    public GameObject miniPlayerObject;

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

    public void ResetStats()
    {
        //When round ends
        playerStats.ResetStats();
    }

    public void Heal()
    {

    }

    public void PlusAttack(int addition)
    {
        playerStats.damage += addition;
    }

    public void MultiplyAttack(int multiplier)
    {
        playerStats.damage *=  multiplier;
    }

    public void IncreaseShield()
    {
        playerStats.shield += 1; //reset after round?
    }

    public void ResetToStart()
    {

    }

    public void IsConfused()
    {
        miniPlayerObject.GetComponent<LeftPlayerController>();
    }

    public void IncreaseView()
    {

    }

    public void DecreaseView()
    {

    }

    public void ZoomOutBuff()
    {
        //Only until round ends
    }

    public void ZoomInDebuff()
    {

    }

    public void BlockExit()
    {

    }
}
