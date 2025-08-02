using System.Collections;
using UnityEngine;

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
        var damage = player1.DealDamage();
        yield return new WaitForSeconds(0.5f);
        player2.TakeDamage(damage);
        Debug.Log($"Player 1 dealt {damage} damage");
        yield return new WaitForSeconds(1f);
        LevelManager.Instance.StartRound();
    }

    IEnumerator FinishMazePlayer2()
    {
        var damage = player2.DealDamage();
        yield return new WaitForSeconds(0.5f);
        player1.TakeDamage(damage);
        Debug.Log($"Player 2 dealt {damage} damage");
        yield return new WaitForSeconds(1f);
        LevelManager.Instance.StartRound();
    }
}
