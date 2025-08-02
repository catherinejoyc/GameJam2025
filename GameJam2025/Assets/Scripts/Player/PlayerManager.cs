using UnityEngine;

public enum Player {
    Player1,
    Player2
}

public class PlayerManager : MonoBehaviour
{
    public Player player;
    public PlayerStats playerStats;
    public PlayerUI playerUI;
    public Camera playerCamera;

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
        playerUI.AttackAnimation();
        return playerStats.damage;
    }

    private void Update()
    {
        switch (player)
        {
            case Player.Player1:
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    UseViewGauge();
                }
                else
                {
                    ResetViewGauge();
                }
                break;
            case Player.Player2:
                if (Input.GetKey(KeyCode.RightShift))
                {
                    UseViewGauge();
                }
                else
                {
                    ResetViewGauge();
                }
                break;
        }
    }

    private void UseViewGauge()
    {
        if (playerStats.viewGauge == 0)
        {
            ResetViewGauge();
            return;
        }
        if (playerCamera.transform.localPosition.z > -60)
        {
            var moveDistance = 100 + playerCamera.transform.localPosition.z;
            playerCamera.transform.Translate(Vector3.back * Time.deltaTime * moveDistance);
        }

        var newValue = playerStats.viewGauge - Time.deltaTime * 10;
        playerStats.viewGauge = newValue < 0 ? 0 : newValue; 
        playerUI.UpdateViewGauge(newValue);
    }

    private void ResetViewGauge()
    {
        if (playerCamera.transform.localPosition.z < -7)
        {
            var moveDistance = 7 + playerCamera.transform.localPosition.z;
            playerCamera.transform.Translate(Vector3.back * Time.deltaTime * moveDistance);
        }
    }
}
