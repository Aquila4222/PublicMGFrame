using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitVE : MonoBehaviour
{
    public Sprite circle;
    public Sprite ring;
    
    public SpriteRenderer SpriteRenderer;

    private float localScale;
    
    public void PlayEffect(Vector3 position , JudgmentResult result ,float scale = 1)
    {
        localScale = scale;
        
        
        if (result == JudgmentResult.Perfect)
        {
            SpriteRenderer.color = Color.yellow;
        }
        else if (result == JudgmentResult.Early || result == JudgmentResult.Late)
        {
            SpriteRenderer.color = Color.cyan;
        }
        else if (result == JudgmentResult.Bad || result == JudgmentResult.Miss)
        {
            SpriteRenderer.color = Color.red;
        }
        transform.position = position;
        
        if (result == JudgmentResult.Bad)
        {
            StartCoroutine(PlayBad());
        }
        else if (result != JudgmentResult.Miss)
        {
            StartCoroutine(Play());
        }
        else
        {
            HitVEPool.Instance.ReturnObject(gameObject);
        }
    }


    private void Awake()
    {
        if (Camera.main != null)
            transform.parent = Camera.main.gameObject.transform;
    }

    IEnumerator Play()
    {
        SpriteRenderer.sprite = ring;
        
        SpriteRenderer.enabled = true;
        float t = 0f;
        for (int i = 0; i < 20; i++)
        {
            SpriteRenderer.enabled = true;
            t = i / 20f;
            float y = t * (2-t);
            transform.localScale =localScale* new Vector3((0.5f+y)*1.5f, (0.5f+y)*1.5f, 1f);
            SpriteRenderer.color = new Color(SpriteRenderer.color.r, SpriteRenderer.color.g, SpriteRenderer.color.b, 0.5f*(1-y));
            yield return new WaitForSeconds(0.008f);
        }
        HitVEPool.Instance.ReturnObject(gameObject);
    }
    
    IEnumerator PlayBad()
    {
        SpriteRenderer.sprite = circle;
        
        SpriteRenderer.enabled = true;
        transform.localScale =new Vector3(1.4f,1.4f,1.4f);
        float t = 0f;
        for (int i = 0; i < 20; i++)
        {
            SpriteRenderer.enabled = true;
            t = i / 20f;
            float y = t * (2-t);
            SpriteRenderer.color = new Color(SpriteRenderer.color.r, SpriteRenderer.color.g, SpriteRenderer.color.b, 0.5f*(1-y));
            yield return new WaitForSeconds(0.008f);
        }
        HitVEPool.Instance.ReturnObject(gameObject);
    }
    
}
