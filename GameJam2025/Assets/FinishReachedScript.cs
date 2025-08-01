using System;
using UnityEngine;

public class FinishReachedScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private LevelManager.FinishReachedDelegate _finishReachedDelegate;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDelegate(LevelManager.FinishReachedDelegate finishReachedDelegate)
    {
        this._finishReachedDelegate = finishReachedDelegate;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _finishReachedDelegate?.Invoke();
    }
}
