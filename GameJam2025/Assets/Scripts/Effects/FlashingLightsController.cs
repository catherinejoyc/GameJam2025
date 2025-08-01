using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlashingLightsController : MonoBehaviour
{
    public List<Light2D> neonLights;
    public float minOnTime = 0.2f;
    public float maxOnTime = 1.2f;
    public float minOffTime = 0.3f;
    public float maxOffTime = 2.0f;

    void Start()
    {
        foreach (Light2D light in neonLights)
        {
            StartCoroutine(FlickerRoutine(light));
        }
    }

    IEnumerator FlickerRoutine(Light2D light)
    {
        while (true)
        {
            float onDelay = Random.Range(minOffTime, maxOffTime);
            yield return new WaitForSeconds(onDelay);
            light.enabled = true;

            float onTime = Random.Range(minOnTime, maxOnTime);
            yield return new WaitForSeconds(onTime);
            light.enabled = false;
        }
    }
}
