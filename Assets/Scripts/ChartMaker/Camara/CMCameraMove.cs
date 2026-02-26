using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMCameraMove : MonoBehaviour
{
    private const float Yoffset = 3f;
    
    private static CMCameraMove _instance; 
    public static CMCameraMove Instance => _instance;
    void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        transform.position=new Vector3 (0, 0, -10);
    }
    
    void Update()
    {
        transform.position = new Vector3(0, CMAudioController.Instance.TimeTrack * CMConstDatas.DistancePerSecond+Yoffset, -10);
    }
}
