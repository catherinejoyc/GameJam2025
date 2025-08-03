using System;
using UnityEngine;

public class SwitchScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Rigidbody2D>() == null) return;
        Maze maze = transform.parent.parent.gameObject.GetComponent<Maze>();
        maze.RemoveBarricade();
    }
}
