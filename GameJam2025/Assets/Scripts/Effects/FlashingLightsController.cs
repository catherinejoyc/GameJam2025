using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashingLightsController : MonoBehaviour
{
    private List<GameObject> lights;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lights = GetComponentInChildren<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
