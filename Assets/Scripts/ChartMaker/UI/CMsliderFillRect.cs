using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMslider : MonoBehaviour
{
    
    void Update()
    {
        transform.localScale = new Vector3(transform.localScale.x,
            CMsliderHandleRect.Instance.transform.localPosition.y + CMAudioController.Instance.sliderLength/2f, transform.localScale.z);
        transform.localPosition = new Vector3(CMAudioController.Instance.sliderXposition,
            transform.localScale.y / 2f - CMAudioController.Instance.sliderLength / 2f,
            transform.localPosition.z);
    }
}
