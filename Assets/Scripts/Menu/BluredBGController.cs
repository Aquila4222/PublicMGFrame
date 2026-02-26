using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BluredBGController : MonoBehaviour
{
    private static BluredBGController _instance; 
    public static BluredBGController Instance => _instance;
    
    [SerializeField]private Sprite[] sprites;
    
    [SerializeField]private Image oldSpriteRenderer;
    [SerializeField]private Image newSpriteRenderer;
    
    [SerializeField]private Sprite oldSprite;
    [SerializeField]private Sprite newSprite;

    private void Awake()
    {
        _instance = this;
    }

    public void ChangeBackground(int index)
    {
        ChangeSprite(sprites[index]);
    }
    
    private void ChangeSprite(Sprite sprite)
    {
        if (oldSprite == null)
        {
            oldSprite = sprite;
            newSprite = sprite;
        }
        else
        {
            oldSprite = newSprite;
            newSprite = sprite;
        }

        oldSpriteRenderer.sprite = oldSprite;
        newSpriteRenderer.sprite = newSprite;
        
        timer = ChangeTime;
    }

    private float timer;
    public float ChangeTime;
   
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            oldSpriteRenderer.color = new Color(oldSpriteRenderer.color.r, oldSpriteRenderer.color.g,
                oldSpriteRenderer.color.b, 1);
            newSpriteRenderer.color = new Color(newSpriteRenderer.color.r, newSpriteRenderer.color.g,
                newSpriteRenderer.color.b, 1 - timer/ChangeTime);
        }
        else
        {
            newSpriteRenderer.color = new Color(newSpriteRenderer.color.r, newSpriteRenderer.color.g,
                newSpriteRenderer.color.b, 1);
        }
    }
}
