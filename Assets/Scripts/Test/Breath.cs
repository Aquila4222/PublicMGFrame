using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Breath : MonoBehaviour
{
    private TMP_Text text;
    
    private float timer;
    
    public float speed;

    private bool isDisappear;
    
    // Start is called before the first frame update
    void Start()
    {
        text =  GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDisappear)
        {
            if (timer >= 0)
            {
                timer -= Time.deltaTime*speed;
                text.color = new Color(text.color.r, text.color.g, text.color.b, timer);
            }
            else
            {
                isDisappear = false;
            }
        }
        else
        {
            if (timer <= 1)
            {
                timer+=Time.deltaTime*speed;
                text.color = new Color(text.color.r, text.color.g, text.color.b, timer);
            }
            else
            {
                isDisappear = true;
            }
        }
    }
}
