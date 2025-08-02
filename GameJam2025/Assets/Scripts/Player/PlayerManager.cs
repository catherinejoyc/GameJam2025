using Assets.Scripts.Player;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerStats playerStats;
    public PlayerUI playerUI;
    public GameObject mazePlayerObject;
    public GameObject blockPrefab;

    public void TakeDamage(float damage)
    {
        var newHealth = playerStats.health + playerStats.shield - damage;
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
        RemoveConfused();
        playerStats.ResetStats();

        var left = mazePlayerObject.GetComponent<LeftPlayerController>();
        if (left != null)
        {
            left.moveSpeed = 3.0f;
            return;
        }

        var right = mazePlayerObject.GetComponent<RightPlayerController>();
        if (right != null)
        {
            right.moveSpeed = 3.0f;
            return;
        }

        Debug.LogWarning("No player controller found on maze player!");
    }

    public void IncreaseSpeed(int amount)
    {
        playerStats.speed += amount;
        var left = mazePlayerObject.GetComponent<LeftPlayerController>();
        if (left != null)
        {
            left.moveSpeed += amount;
            return;
        }

        var right = mazePlayerObject.GetComponent<RightPlayerController>();
        if (right != null)
        {
            right.moveSpeed += amount;
            return;
        }

        Debug.LogWarning("No player controller found on maze player!");
    }

    public void DecreaseSpeed(int amount)
    {
        if(playerStats.speed > 0)
        { 
            playerStats.speed -= amount; 
        }

        var left = mazePlayerObject.GetComponent<LeftPlayerController>();
        if (left != null)
        {
            left.moveSpeed -= amount;
            return;
        }

        var right = mazePlayerObject.GetComponent<RightPlayerController>();
        if (right != null)
        {
            right.moveSpeed -= amount;
            return;
        }

        Debug.LogWarning("No player controller found on maze player!");
    }

    public void Heal(int amount)
    {
        if(playerStats.health < 100)
        {
            playerStats.health += amount;
        }
    }

    public void PlusAttack(int addition)
    {
        playerStats.damage += addition;
    }

    public void MultiplyAttack(int multiplier)
    {
        playerStats.damage *=  multiplier;
    }

    public void IncreaseShield(int amount)
    {
        playerStats.shield += amount; //reset after round?
    }

    public void ResetToStart(PlayerType forPlayer)
    {
        switch (forPlayer)
        {
            case PlayerType.Player1:
                mazePlayerObject.transform.position = LevelManager.Instance.StartingTilePlayer1.transform.position;
                break;
            case PlayerType.Player2:
                mazePlayerObject.transform.position = LevelManager.Instance.StartingTilePlayer2.transform.position;
                break;
        }
    }

    public void MakeConfused()
    {
        // Check if left or right player and set confused
        var left = mazePlayerObject.GetComponent<LeftPlayerController>();
        if (left != null)
        {
            left.isConfused = true;
            return;
        }

        var right = mazePlayerObject.GetComponent<RightPlayerController>();
        if (right != null)
        {
            right.isConfused = true;
            return;
        }

        Debug.LogWarning("No player controller found on maze player!");
    }

    public void RemoveConfused()
    {
        var left = mazePlayerObject.GetComponent<LeftPlayerController>();
        if (left != null)
        {
            left.isConfused = false;
            return;
        }

        var right = mazePlayerObject.GetComponent<RightPlayerController>();
        if (right != null)
        {
            right.isConfused = false;
            return;
        }

        Debug.LogWarning("No player controller found on maze player!");
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

    public void BlockExit(PlayerType forPlayer)
    {
        var tileToGet = new Tile();
        switch (forPlayer)
        {
            case PlayerType.Player1:
                tileToGet = LevelManager.Instance.FinishingTilePlayer1;
                break;
            case PlayerType.Player2:
                tileToGet = LevelManager.Instance.FinishingTilePlayer2;
                break;
        }
        var position = tileToGet.transform.position;
        Instantiate(blockPrefab, position, tileToGet.transform.rotation);
    }
}
