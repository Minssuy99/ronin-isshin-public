
using System;
using UnityEngine;
using System.Collections.Generic;

public class ForegroundFader : MonoBehaviour
{
    [SerializeField] private float fadeOutAlpha = 0.2f;
    [SerializeField] private float fadeDuration = 0.3f;
    
    private Dictionary<SpriteRenderer, Coroutine> fadingObjects = new Dictionary<SpriteRenderer, Coroutine>();
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        HandleFade(other, fadeOutAlpha);
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        HandleFade(other, 1f);
    }

    private void OnValidate()
    {
        fadeOutAlpha = Mathf.Clamp01(fadeOutAlpha);
    }

    private void HandleFade(Collider2D other, float targetAlpha)
    {
        if (!other.CompareTag("Props")) return;
        
        SpriteRenderer sr = other.GetComponent<SpriteRenderer>();
        if (sr == null) return;
        
        if (fadingObjects.ContainsKey(sr))
        {
            StopCoroutine(fadingObjects[sr]);
        }
        
        fadingObjects[sr] = StartCoroutine(Base_Mng.Instance.UI.Fade(sr, fadeDuration, targetAlpha));
    }
}