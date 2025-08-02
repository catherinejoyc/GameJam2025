using Assets.Scripts.Collectibles;
using System.Collections;
using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    public Effects effect = Effects.None;

    [SerializeField]
    private float frequency = 2;
    [SerializeField]
    private float height = 1;

    private float originalY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalY = transform.position.y;
    } 

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos.y = originalY + Mathf.Sin(Time.time * frequency) * height;
        transform.position = pos;
    }

    public void TriggerDespawn()
    {
        var animation = transform.GetChild(2);
        animation.gameObject.SetActive(true);
        StartCoroutine(DestroyAfterSeconds(0.3f));
    }

    IEnumerator DestroyAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }

    public float GetFrequency()
    {
        return frequency;
    }
}
