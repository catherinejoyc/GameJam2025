using Assets.Scripts.Collectibles;
using Assets.Scripts.Player;
using System.Collections.Generic;
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
    public EffectDisplayUI effectDisplayUI;
    public Camera playerCamera;
    public GameObject mazePlayerObject;
    public GameObject blockPrefab;
    public List<Effects> currentEffects = new List<Effects>();

    public GameObject switchPrefab;

    public void TakeDamage(float damage)
    {
        var calculatedDamage = damage - playerStats.shield > 0 ? damage - playerStats.shield : 0;
        var newHealth = playerStats.health - calculatedDamage;
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
        RefillViewGauge(1);
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

    private void UpdateUI()
    {
        playerUI.UpdateHealth(playerStats.health);
        playerUI.UpdateViewGauge(playerStats.viewGauge);
        effectDisplayUI.RefreshUI(currentEffects);
    }

    public void AddEffectToList(Effects effect)
    {
        this.currentEffects.Add(effect);
        effectDisplayUI.RefreshUI(currentEffects);
    }

    public void ResetStats()
    {
        //When round ends
        RemoveConfused();
        ResetCamera();
        playerStats.ResetStats();

        currentEffects.Clear();
        UpdateUI();

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
        if(playerStats.health + amount < 50)
        {
            playerStats.health += amount;
        }
        UpdateUI();
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

        var newValue = playerStats.viewGauge - Time.deltaTime * 20;
        playerStats.viewGauge = newValue < 0 ? 0 : newValue;
        playerUI.UpdateViewGauge(newValue);
    }

    private void ResetViewGauge()
    {
        if (playerCamera.transform.localPosition.z < -playerStats.zoom)
        {
            var moveDistance = playerStats.zoom + playerCamera.transform.localPosition.z;
            playerCamera.transform.Translate(Vector3.back * Time.deltaTime * moveDistance);
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
        playerStats.viewGauge += 30;
        playerStats.viewGauge = Mathf.Min(100, playerStats.viewGauge);
        UpdateUI();
    }

    public void DecreaseView(int amount)
    {
        playerStats.viewGauge = playerStats.viewGauge - amount < 0 ? 0 : playerStats.viewGauge - amount;
        UpdateUI();

    }

    public void ResetCamera()
    {
        playerCamera.transform.position = new Vector3(playerCamera.transform.position.x, playerCamera.transform.position.y, Mathf.Min(playerCamera.transform.position.z, -playerStats.zoom));
    }

    public void ZoomOutBuff()
    {
        playerStats.zoom += 2;
        ResetCamera();
    }

    public void ZoomInDebuff()
    {
        playerStats.zoom -= 2;
        playerStats.zoom = Mathf.Max(1, playerStats.zoom); // Ensure zoom does not go below 1
    }

    public void BlockExit(PlayerType forPlayer)
    {
        var tileToGet = new Tile();
        var switchTile = new Tile();
        Maze maze = null;
        switch (forPlayer)
        {
            case PlayerType.Player1:
                tileToGet = LevelManager.Instance.FinishingTilePlayer1;
                maze = LevelManager.Instance.Maze1Container.GetComponentInChildren<Maze>();
                switchTile = maze.getHardToReachTile();
                break;
            case PlayerType.Player2:
                tileToGet = LevelManager.Instance.FinishingTilePlayer2;
                maze = LevelManager.Instance.Maze2Container.GetComponentInChildren<Maze>();
                switchTile = maze.getHardToReachTile();
                break;
        }
        var barricade = Instantiate(blockPrefab, tileToGet.transform.position + Vector3.right * 0.85f, Quaternion.identity, tileToGet.transform);
        var switchObject = Instantiate(switchPrefab, switchTile.transform.position, Quaternion.identity, switchTile.transform);
        maze.SetBarricade(barricade, switchObject);
    }

    public void RefillViewGauge(int amount)
    {
        playerStats.viewGauge += amount * Time.deltaTime;
        playerStats.viewGauge = Mathf.Min(100, playerStats.viewGauge);
        UpdateUI();
    }
}
