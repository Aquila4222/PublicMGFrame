using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class notepisotiontest : MonoBehaviour
{
    
    private Renderer renderer;
    [SerializeField] private float x;
    [SerializeField] private float y;
    
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }


    void Update()
    {
        
        
        if (CMCalculater.Instance.CalculateTrack() == -1 || CMControlSystem.Instance.CurrentState!=CMControlSystem.PlacingState.Tap)
        {
            renderer.enabled = false;
        }
        else
        {
            renderer.enabled = true;
            x = CMConstDatas.FirstTrackX+CMConstDatas.TrackXDistance*CMCalculater.Instance.CalculateTrack();
            y = CMConstDatas.DistancePerSecond * CMCalculater.Instance.CalculateTime(CMControlSystem.Instance.Bpm,
                CMControlSystem.Instance.CurrentDevidingCount);
            transform.position = new Vector3(x,y,0f);
        }
        
    }
}
