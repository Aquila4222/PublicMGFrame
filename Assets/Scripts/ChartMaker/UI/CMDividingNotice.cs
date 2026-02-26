using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMDividingNotice : MonoBehaviour
{
    private TextMesh textMesh;
    [SerializeField] private int dividingCount;
    void Awake()
    {
        textMesh = GetComponent<TextMesh>();
    }
    
    void Update()
    {
        if (CMControlSystem.Instance.CurrentDevidingCount == dividingCount)
        {
            textMesh.color = new Color(0.2f, 1f, 1f);
        }
        else
        {
            textMesh.color = new Color(1f, 1f, 1f);
        }
        
    }
}
