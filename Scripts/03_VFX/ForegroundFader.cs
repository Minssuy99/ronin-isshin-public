using System;
using UnityEngine;
using System.Collections.Generic;

public class ForegroundFader : MonoBehaviour
{
    [SerializeField, Tooltip("플레이어와 겹치는 구조물의 투명도")] private float fadeOutAlpha = 0.2f;
    [SerializeField, Tooltip("구조물의 페이드인/페이드아웃이 완료되는 시간")] private float fadeDuration = 0.3f;
    
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