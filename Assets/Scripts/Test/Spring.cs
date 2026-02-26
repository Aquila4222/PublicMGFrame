using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    public float speed;


    private void Update()
    {
        transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z+speed*Time.deltaTime); 
    }
}
