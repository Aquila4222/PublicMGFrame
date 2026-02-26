using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDelay : MonoBehaviour
{
    public TextMesh textMesh;
    
    private string text = "延迟：";
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        textMesh.text = text + $"{Datas.Delay:0.00}";
    }
}
