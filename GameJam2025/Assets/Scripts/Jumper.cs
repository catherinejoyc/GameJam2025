using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public LayerMask groundMask;
    public Vector2 groundCheckOffset;
    public float groundCheckRadius;
    public float jumpMultiplier = 3;
    public float pitchMultiplier = 1;
    public AudioSpectrum audioSpectrum;
    public float[] peakLevels;
    public int pitch;
    public float maxValue;
    public int jumpTolerance = 3;
    public string jumpTimestamp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        peakLevels = audioSpectrum.PeakLevels.Select(x => Mathf.Round(x * 100)).ToArray();
        
        var newMaxValue = peakLevels.Max();
        var newPitch = Array.IndexOf(peakLevels, newMaxValue);
        if (Mathf.Abs(newPitch - pitch) > jumpTolerance && IsGrounded())
        {
            maxValue = newMaxValue;
            pitch = newPitch;
            jumpTimestamp = DateTime.Now.ToString() + " " + DateTime.Now.Ticks;
            rigidBody.AddForce(Vector2.up * (jumpMultiplier + pitch * pitchMultiplier), ForceMode2D.Impulse);
        }
    }

    private bool IsGrounded()
    {
        if (Physics2D.OverlapCircle((Vector2)this.transform.position + groundCheckOffset, groundCheckRadius, groundMask))
        {
            return true;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere((Vector2)this.transform.position + groundCheckOffset, groundCheckRadius);
    }
}
