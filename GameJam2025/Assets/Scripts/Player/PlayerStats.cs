using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public float health = 100;
    public float damage = 1;
    public float speed = 1;
    public float shield = 0;


    public void ResetStats()
    {
        damage = 1;
        speed = 1;
        shield = 0;
    }
}
