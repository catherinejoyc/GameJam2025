using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private Slider healthSlider;

    public void UpdateHealth(float health)
    {
        healthSlider.value = health < 0 ? 0 : health/100;
    }
}
