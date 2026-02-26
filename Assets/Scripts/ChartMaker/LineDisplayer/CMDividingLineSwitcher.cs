using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMDividingLineSwitcher : MonoBehaviour
{
    public Mesh[] dividingMeshes;

    public MeshFilter meshFilter;

    public MeshRenderer meshRenderer;
    
    public void SwitchLine(int index)
    {
        meshRenderer.enabled = true;
        meshFilter.mesh = dividingMeshes[index];
    }
    
    public void CloseLine()
    {
        meshRenderer.enabled = false;
    }

    public void UpdateDividingLine()
    {
        transform.localScale = new Vector3(transform.localScale.x, 1 * CMConstDatas.DistancePerSecond / CMConstDatas.BasicDps,
            transform.localScale.z);
    }

    private void Awake()
    {
        CMControlSystem.Instance.UpdateDps += UpdateDividingLine;
    }
    
}
