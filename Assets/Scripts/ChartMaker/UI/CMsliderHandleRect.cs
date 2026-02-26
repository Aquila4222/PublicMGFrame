using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMsliderHandleRect : MonoBehaviour
{
    private static CMsliderHandleRect _instance; 
    public static CMsliderHandleRect Instance => _instance;
    
    private SpriteRenderer sprite;

    void Awake()
    {
        _instance = this;
        sprite = GetComponent<SpriteRenderer>();
    }

    
    void Update()
    {
        if (CMAudioController.Instance.isSliding)
        {
            sprite.enabled = true;
        }
        else
        {
            sprite.enabled = false;
        }
        transform.localPosition = new Vector3(CMAudioController.Instance.sliderXposition,
            CMAudioController.Instance.sliderLength * CMAudioController.Instance.TimeTrack /
            CMAudioController.Instance.maxTime - CMAudioController.Instance.sliderLength / 2f,
            transform.localPosition.z);
    }
}
