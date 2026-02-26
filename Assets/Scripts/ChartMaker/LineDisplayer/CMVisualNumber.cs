using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMVisualNumber : MonoBehaviour
{
    public int index;
    TextMesh textMesh ;

    public void Initialize()
    {
        textMesh = GetComponent<TextMesh>();
        textMesh.text = (index+1).ToString();
        CMControlSystem.Instance.UpdateDps += UpdatePosition;
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        transform.position = CMLineGenerater.Instance.NumberPosition + new Vector3(0f,
            index * 60f * CMConstDatas.DistancePerSecond / CMControlSystem.Instance.Bpm, 0f);
    }

}
