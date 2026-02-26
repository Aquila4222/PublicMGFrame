using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMPlacingNotice : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] CMControlSystem.PlacingState placingState;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        if (CMControlSystem.Instance.CurrentState == placingState)
        {
            spriteRenderer.enabled = true;
        }
        else
        {
            spriteRenderer.enabled = false;
        }
        
    }
}
