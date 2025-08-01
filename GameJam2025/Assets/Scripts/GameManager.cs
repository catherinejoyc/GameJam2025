using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayerManager player1;
    [SerializeField]
    private PlayerManager player2;

    public void Player1FinishesMaze()
    {
        player2.TakeDamage(player1.DealDamage());
    }

    public void Player2FinishesMaze()
    {
        player1.TakeDamage(player2.DealDamage());
    }
}
