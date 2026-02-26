using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackClickEffect : MonoBehaviour
{
    private Vector3 originalPosition;
    
    public SpriteRenderer TrackSprite;
    
    public int TrackIndex;

    public float MoveTime;
    public float Offset;
    private float timer;
    
    private Color effectColor = Color.white;
    
    private bool isAutoPlay;
    
    private void Awake()
    {
        originalPosition =  transform.position;
        TrackSprite = GetComponent<SpriteRenderer>();
        
        isAutoPlay = TrackController.Instance.IsAutoPlay;
    }
    
    

    private void Update()
    {
        if (Datas.IndicatorEnabled == true)
        {
            if (ScoringSystem.Instance.IsAllPerfect)
            {
                effectColor = Color.yellow;
            }
            else if (ScoringSystem.Instance.IsFullCombo)
            {
                effectColor = Color.cyan;
            }
            else
            {
                effectColor = Color.white;
            }
        }
        else
        {
            effectColor = Color.white;
        }
        
        
        if (InputSystem.HitDown(TrackIndex) && isAutoPlay == false)
        {
            transform.position =  originalPosition;
            TrackSprite.color = new Color(effectColor.r,effectColor.g,effectColor.b, 0.1f);
            timer = MoveTime;
        }
        else
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            transform.position = originalPosition + new Vector3(0, Offset*(1-timer/MoveTime), 0);
            TrackSprite.color = new Color(effectColor.r,effectColor.g,effectColor.b, (timer / MoveTime)*0.1f);
        }
    }
}
