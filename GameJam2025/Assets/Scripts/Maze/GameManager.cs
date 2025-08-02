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
        player2.TakeDamage(player1.DealDamage());
        Debug.Log($"Player 1 dealt {player1.DealDamage()} damage");
        LevelManager.Instance.StartRound();
    }

    public void Player2FinishesMaze()
    {
        player1.TakeDamage(player2.DealDamage());
        Debug.Log($"Player 2 dealt {player1.DealDamage()} damage");
        LevelManager.Instance.StartRound();

    }
}
