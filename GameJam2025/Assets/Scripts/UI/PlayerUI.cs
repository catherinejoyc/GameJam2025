using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private Slider healthSlider;
    [SerializeField]
    private Animator animator;

    public void UpdateHealth(float health)
    {
        if (healthSlider.value > health/100)
        {
            animator.SetTrigger("IsHurt");
        }
        healthSlider.value = health < 0 ? 0 : health/100;
    }

    public void AttackAnimation()
    {
        animator.SetTrigger("IsAttacking");
    }
}
