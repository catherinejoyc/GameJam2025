using Assets.Scripts.Player;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }

    }

    public PlayerManager player1;
    public PlayerManager player2;

    public Player winner;

    public void EndGame(Player loser)
    {
        if(loser == Player.Player1)
        {
            winner = Player.Player2;
        } else
        {
            winner = Player.Player1;
        }
        SceneManager.LoadScene("End Screen");
    }

    public void Player1FinishesMaze()
    {
        StartCoroutine(FinishMazePlayer1());
    }

    

    public void Player2FinishesMaze()
    {
        StartCoroutine(FinishMazePlayer2());
    }

    IEnumerator FinishMazePlayer1()
    {
        FreezePlayers(true);
        var damage = player1.DealDamage();
        yield return new WaitForSeconds(0.3f);
        var health = player2.TakeDamage(damage);
        Debug.Log($"Player 1 dealt {damage} damage");
        if (health == 0)
        {
            yield return new WaitForSeconds(1f);
            player2.DieAnimationSound();
            yield return new WaitForSeconds(1f);
            player2.Die();
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(LevelManager.Instance.StartRound());
    }

    IEnumerator FinishMazePlayer2()
    {
        FreezePlayers(true);
        var damage = player2.DealDamage();
        yield return new WaitForSeconds(0.3f);
        var health = player1.TakeDamage(damage);
        Debug.Log($"Player 2 dealt {damage} damage");
        if (health == 0)
        {
            yield return new WaitForSeconds(1f);
            player1.DieAnimationSound();
            yield return new WaitForSeconds(1f);
            player1.Die();
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(LevelManager.Instance.StartRound());
    }

    public void FreezePlayers(bool freeze)
    {
        var movement1 = player1.gameObject.GetComponentInChildren<LeftPlayerController>();
        var movement2 = player2.gameObject.GetComponentInChildren<RightPlayerController>();

        movement1.Freeze(freeze);
        movement2.Freeze(freeze);
    }
}
