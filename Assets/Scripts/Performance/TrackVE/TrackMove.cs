using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackMove : MonoBehaviour
{
    public int VTrackIndex;
    
    
  
    void Update()
    {
        transform.position = new Vector3(TrackVE.Instance.VTrackPos[VTrackIndex], transform.position.y, transform.position.z);
    }
}
