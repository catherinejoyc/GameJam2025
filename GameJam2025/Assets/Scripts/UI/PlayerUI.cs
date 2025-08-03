using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private Slider healthSlider;
    [SerializeField]
    private Slider viewGaugeSlider;
    [SerializeField]
    private Animator animator;

    public void UpdateHealth(float health)
    {
        if (healthSlider.value > health/50)
        {
            animator.SetTrigger("IsHurt");
        }
        healthSlider.value = health < 0 ? 0 : health/50;
    }

    public void AttackAnimation()
    {
        animator.SetTrigger("IsAttacking");
    }
    
    public void UpdateViewGauge(float value)
    {
        viewGaugeSlider.value = value < 0 ? 0 : value / 100;
    }
}
