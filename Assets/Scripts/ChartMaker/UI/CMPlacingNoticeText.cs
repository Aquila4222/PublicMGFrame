using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMPlacingNoticeText : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    [SerializeField] CMControlSystem.PlacingState placingState;
    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    
    void Update()
    {
        if (CMControlSystem.Instance.CurrentState == placingState)
        {
            meshRenderer.enabled = true;
        }
        else
        {
            meshRenderer.enabled = false;
        }
        
    }
}
