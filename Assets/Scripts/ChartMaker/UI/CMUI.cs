using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMUI : MonoBehaviour
{
    private static CMUI _instance;
    public static CMUI Instance => _instance;

    private Transform bpmErrorText;
    private TextMesh bpmErrorTextTM;
    
    private Transform bpmError;
    private SpriteRenderer bpmErrorSR;
    
    private Transform audioErrorText;
    private TextMesh audioErrorTextTM;
    
    private Transform audioError;
    private TextMesh audioErrorSR;

    void Awake()
    {
        _instance = this;
        
        bpmErrorText = transform.GetChild(0);
        bpmErrorTextTM = bpmErrorText.GetComponent<TextMesh>();
        bpmError = transform.GetChild(1);
        bpmErrorSR = bpmError.GetComponent<SpriteRenderer>();
        audioErrorText = transform.GetChild(2);
        audioErrorTextTM = audioErrorText.GetComponent<TextMesh>();
        audioError = transform.GetChild(3);
        audioErrorSR = audioError.GetComponent<TextMesh>();
        
    }
    
    
}
