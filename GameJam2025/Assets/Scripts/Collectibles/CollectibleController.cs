using Assets.Scripts.Collectibles;
using UnityEngine;

public class FloatingAnimation : MonoBehaviour
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
}
