using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Mng : Singleton<UI_Mng>
{ 
    public IEnumerator Fade(Image image, float duration, float targetAlpha)
    {
        float startAlpha = image.color.a;
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            SetAlpha(image, currentAlpha);
            yield return null;
        }
        SetAlpha(image, targetAlpha);
    }
    
    public IEnumerator Fade(SpriteRenderer spriterenderer, float duration, float targetAlpha)
    {
        float startAlpha = spriterenderer.color.a;
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            SetAlpha(spriterenderer, currentAlpha);
            yield return null;
        }
        SetAlpha(spriterenderer, targetAlpha);
    }

    public void SetAlpha(Image image, float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }

    public void SetAlpha(SpriteRenderer spriterenderer, float alpha)
    {
        Color color = spriterenderer.color;
        color.a = alpha;
        spriterenderer.color = color;
    }
}
