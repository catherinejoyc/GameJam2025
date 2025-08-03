using TMPro;
using UnityEngine;

public class EndScreenManager : MonoBehaviour
{
    public GameObject player1Slime;
    public GameObject player2Slime;

    public TextMeshProUGUI winnerText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player1Slime.SetActive(false);
        player2Slime.SetActive(false);
        if(GameManager.Instance.winner == Player.Player1)
        {
            player1Slime.SetActive(true);
            winnerText.text = "Red Slime Won!";
        }
        else
        {
            player2Slime.SetActive(true);
            winnerText.text = "Blue Slime Won!";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
