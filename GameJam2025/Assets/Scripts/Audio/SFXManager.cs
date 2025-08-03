using Assets.Scripts.Collectibles;
using Assets.Scripts.Player;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public AudioSource sfxSource;
    [Header("Collectibles")]
    public AudioClip blockerSfx;
    public AudioClip confusionSfx;
    public AudioClip decreaseViewTimeSfx;
    public AudioClip doubleAttackSfx;
    public AudioClip healSoundSfx;
    public AudioClip increaseViewTimeSfx;
    public AudioClip plusAttackDamageSfx;
    public AudioClip restartSfx;
    public AudioClip shieldSfx;
    public AudioClip increaseSpeedSfx;
    public AudioClip decreaseSpeedSfx;
    public AudioClip increaseZoomOutSfx;
    public AudioClip decreaseZoomOutSfx;

    public void PlaySfx(Effects collectibleEffect)
    {
        Debug.Log("COLLIDED WITH EFFECT: " + collectibleEffect);
        switch (collectibleEffect)
        {
            case Effects.None:
                Debug.Log("No effect on collectible");
                break;
            case Effects.ShieldBuff:
                sfxSource.clip = shieldSfx;
                break;
            case Effects.SpeedBuff:
                sfxSource.clip = increaseSpeedSfx;
                break;
            case Effects.SpeedDebuff:
                sfxSource.clip = decreaseSpeedSfx;
                break;
            case Effects.PlusAttack:
                sfxSource.clip = plusAttackDamageSfx;
                break;
            case Effects.MultiplyAttack:
                sfxSource.clip = doubleAttackSfx;
                break;
            case Effects.ZoomBuff:
                sfxSource.clip = increaseZoomOutSfx;
                break;
            case Effects.ZoomDebuff:
                sfxSource.clip = decreaseZoomOutSfx;
                break;
            case Effects.ViewBuff:
                sfxSource.clip = increaseViewTimeSfx;
                break;
            case Effects.ViewDebuff:
                sfxSource.clip = decreaseViewTimeSfx;
                break;
            case Effects.ResetStartDebuff:
                sfxSource.clip = restartSfx;
                break;
            case Effects.Heal:
                sfxSource.clip = healSoundSfx;
                break;
            case Effects.ConfusionDebuff:
                sfxSource.clip = confusionSfx;
                break;
            case Effects.BlockExitDebuff:
                sfxSource.clip = blockerSfx;
                break;
        }
        if (sfxSource.clip != null) sfxSource.Play();
    }
}
