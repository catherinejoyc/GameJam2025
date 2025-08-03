using Assets.Scripts.Collectibles;
using Assets.Scripts.Player;
using UnityEngine;

public class RightPlayerController : MonoBehaviour
{
    public bool isConfused = false;

    public float moveSpeed = 3f;

    private Vector2 moveDirection;
    private PlayerManager otherPlayerManager;
    private PlayerManager thisPlayerManager;

    private bool frozen = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        otherPlayerManager = GameManager.Instance.player1;
        thisPlayerManager = GameManager.Instance.player2;
    }

    public void Freeze(bool freeze)
    {
        frozen = freeze;
    }

    // Update is called once per frame
    void Update()
    {
        if (frozen) return;
        float horizontal = Input.GetAxisRaw("Horizontal Right");
        float vertical = Input.GetAxisRaw("Vertical Right");

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
        Debug.Log("COLLIDED WITH EFFECT: " + collectibleEffect);
        switch (collectibleEffect)
        {
            case Effects.None:
                Debug.Log("No effect on collectible");
                break;
            case Effects.ShieldBuff:
                thisPlayerManager.IncreaseShield(1);
                thisPlayerManager.AddEffectToList(collectibleEffect);
                break;
            case Effects.SpeedBuff:
                thisPlayerManager.IncreaseSpeed(1);
                thisPlayerManager.AddEffectToList(collectibleEffect);
                break;
            case Effects.SpeedDebuff:
                otherPlayerManager.DecreaseSpeed(1);
                otherPlayerManager.AddEffectToList(collectibleEffect);
                break;
            case Effects.PlusAttack:
                thisPlayerManager.PlusAttack(3);
                thisPlayerManager.AddEffectToList(collectibleEffect);
                break;
            case Effects.MultiplyAttack:
                thisPlayerManager.MultiplyAttack(2);
                thisPlayerManager.AddEffectToList(collectibleEffect);
                break;
            case Effects.ZoomBuff:
                thisPlayerManager.ZoomOutBuff();
                thisPlayerManager.AddEffectToList(collectibleEffect);
                break;
            case Effects.ZoomDebuff:
                otherPlayerManager.ZoomInDebuff();
                otherPlayerManager.AddEffectToList(collectibleEffect);
                break;
            case Effects.ViewBuff:
                thisPlayerManager.IncreaseView();
                thisPlayerManager.AddEffectToList(collectibleEffect);
                break;
            case Effects.ViewDebuff:
                otherPlayerManager.DecreaseView(50);
                otherPlayerManager.AddEffectToList(collectibleEffect);
                break;
            case Effects.ResetStartDebuff:
                otherPlayerManager.ResetToStart(PlayerType.Player2);
                otherPlayerManager.AddEffectToList(collectibleEffect);
                break;
            case Effects.Heal:
                thisPlayerManager.Heal(10);
                //thisPlayerManager.AddEffectToList(collectibleEffect);
                break;
            case Effects.ConfusionDebuff:
                otherPlayerManager.MakeConfused();
                otherPlayerManager.AddEffectToList(collectibleEffect);
                break;
            case Effects.BlockExitDebuff:
                otherPlayerManager.BlockExit(PlayerType.Player2);
                otherPlayerManager.AddEffectToList(collectibleEffect);
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

