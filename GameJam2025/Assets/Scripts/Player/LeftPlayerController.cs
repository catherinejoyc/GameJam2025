using Assets.Scripts.Collectibles;
using Assets.Scripts.Player;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class LeftPlayerController : MonoBehaviour
{
    public bool isConfused = false;

    [SerializeField]
    private GameObject otherPlayer;
    [SerializeField]
    private GameObject thisPlayer;

    public float moveSpeed = 3f;

    private Vector2 moveDirection;
    private PlayerManager otherPlayerManager;
    private PlayerManager thisPlayerManager;

    private bool frozen = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        otherPlayerManager = otherPlayer.GetComponent<PlayerManager>();
        thisPlayerManager = thisPlayer.GetComponent<PlayerManager>();
    }

    public void Freeze(bool freeze)
    {
        frozen = freeze;
    }

    // Update is called once per frame
    void Update()
    {
        if (frozen) return;
        float horizontal = Input.GetAxisRaw("Horizontal Left");
        float vertical = Input.GetAxisRaw("Vertical Left");

        // Move on only one axis
        if (horizontal != 0)
        {
            vertical = 0;
        }

        if (isConfused)
        {
            horizontal = -horizontal;
            vertical = -vertical;
        }

        moveDirection = new Vector2(horizontal, vertical).normalized;
        transform.Translate(moveSpeed * Time.deltaTime * moveDirection);
    }

    private void TriggerCollectedEffect(Effects collectibleEffect)
    {
        switch (collectibleEffect)
        {
            case Effects.None:
                Debug.Log("No effect on collectible");
                break;
            case Effects.ShieldBuff:
                thisPlayerManager.IncreaseShield(1);
                break;
            case Effects.SpeedBuff:
                thisPlayerManager.IncreaseSpeed(1);
                break;
            case Effects.SpeedDebuff:
                otherPlayerManager.DecreaseSpeed(1);
                break;
            case Effects.PlusAttack:
                thisPlayerManager.PlusAttack(3);
                break;
            case Effects.MultiplyAttack:
                thisPlayerManager.MultiplyAttack(2);
                break;
            case Effects.ZoomBuff:
                thisPlayerManager.ZoomOutBuff();
                break;
            case Effects.ZoomDebuff:
                otherPlayerManager.ZoomInDebuff();
                break;
            case Effects.ViewBuff:
                thisPlayerManager.IncreaseView();
                break;
            case Effects.ViewDebuff:
                otherPlayerManager.DecreaseView();
                break;
            case Effects.ResetStartDebuff:
                otherPlayerManager.ResetToStart(PlayerType.Player2);
                break;
            case Effects.Heal:
                thisPlayerManager.Heal(10);
                break;
            case Effects.ConfusionDebuff:
                otherPlayerManager.MakeConfused();
                break;
            case Effects.BlockExitDebuff:
                otherPlayerManager.BlockExit(PlayerType.Player2);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Collectible"))
        {
            var controller = collidedObject.GetComponent<CollectibleController>();
            var collectibleEffect = controller.effect;
            TriggerCollectedEffect(collectibleEffect);
            controller.TriggerDespawn();
        }
    }
}
