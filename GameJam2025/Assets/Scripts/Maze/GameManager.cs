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

    [SerializeField]
    private PlayerManager player1;
    [SerializeField]
    private PlayerManager player2;

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
        player2.TakeDamage(damage);
        Debug.Log($"Player 1 dealt {damage} damage");
        yield return new WaitForSeconds(1f);
        StartCoroutine(LevelManager.Instance.StartRound());
    }

    IEnumerator FinishMazePlayer2()
    {
        FreezePlayers(true);
        var damage = player2.DealDamage();
        yield return new WaitForSeconds(0.3f);
        player1.TakeDamage(damage);
        Debug.Log($"Player 2 dealt {damage} damage");
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
