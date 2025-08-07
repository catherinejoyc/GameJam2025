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
    public StatsUI statsUI;
    public Camera playerCamera;
    public GameObject mazePlayerObject;
    public GameObject blockPrefab;
    public List<Effects> currentEffects = new List<Effects>();

    public GameObject switchPrefab;
    private bool penalize = false;

    public float TakeDamage(float damage)
    {
        var calculatedDamage = damage - playerStats.shield > 0 ? damage - playerStats.shield : 0;
        var newHealth = playerStats.health - calculatedDamage;
        playerUI.UpdateHealth(newHealth);
        playerUI.TriggerHurtAnimation();
        playerStats.health = newHealth < 0 ? 0 : newHealth;
        return playerStats.health;
    }


    public void DieAnimationSound()
    {
        SFXManager.Instance.PlayDieSound();
    }

    public void Die()
    {
        GameManager.Instance.EndGame(player);
    }

    public float DealDamage()
    {
        SFXManager.Instance.PlayHurtSound();
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
                    penalize = true;
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
                    penalize = true;
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
        statsUI.UpdateCurrentStats(playerStats);
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
        UpdateUI();
        Debug.LogWarning("No player controller found on maze player!");
    }

    public void DecreaseSpeed(int amount)
    {
        playerStats.speed -= amount;

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
        UpdateUI();
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
        UpdateUI();
    }

    public void MultiplyAttack(int multiplier)
    {
        playerStats.damage *=  multiplier;
        UpdateUI();
    }

    public void IncreaseShield(int amount)
    {
        playerStats.shield += amount; //reset after round?
        UpdateUI();
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
        if (playerStats.viewGauge < 5)
        {
            ResetViewGauge();
            return;
        }
        if (playerCamera.transform.localPosition.z > -60)
        {
            var moveDistance = 100 + playerCamera.transform.localPosition.z;
            playerCamera.transform.Translate(Vector3.back * Time.deltaTime * moveDistance * 2);
        }

        var newValue = playerStats.viewGauge - Time.deltaTime * 15;
        playerStats.viewGauge = newValue < 0 ? 0 : newValue;
        playerUI.UpdateViewGauge(newValue);
    }

    private void ResetViewGauge()
    {
        var targetZPos = -(playerStats.zoom);
        var transitionSpeed = 0.3f;
        playerCamera.transform.localPosition = Vector3.MoveTowards(playerCamera.transform.localPosition, 
            new Vector3(playerCamera.transform.localPosition.x, playerCamera.transform.localPosition.y, targetZPos), transitionSpeed);

        if (penalize)
        {
            var newValue = playerStats.viewGauge - Time.deltaTime * 15;
            playerStats.viewGauge = newValue < 0 ? 0 : newValue;
        }

        var maxDistanceBetween = 0.1f;
        var distanceDifference = Mathf.Abs(playerCamera.transform.localPosition.z - (-playerStats.zoom));
        if (distanceDifference < maxDistanceBetween)
        {
            penalize = false;
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
        playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, playerCamera.transform.localPosition.y, -playerStats.zoom);
    }

    public void ZoomOutBuff()
    {
        playerStats.zoom += 2;
        ResetCamera();
        UpdateUI();
    }

    public void ZoomInDebuff()
    {
        playerStats.zoom -= 2;
        playerStats.zoom = Mathf.Max(1, playerStats.zoom); // Ensure zoom does not go below 1
        UpdateUI();
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
