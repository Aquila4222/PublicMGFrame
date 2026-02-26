using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class ChartButton : MonoBehaviour
{
    [SerializeField]private TMP_Text difficultyText;
    [SerializeField]private TMP_Text difficultyNumberText;
    [SerializeField]private Image buttonImage;
    [SerializeField]private TMP_Text scoreText;
    [SerializeField]private TMP_Text EvaluateText;
    
    [SerializeField]private UnityEngine.UI.Button button;

    private Color buttonColor;

    private void Awake()
    {
        buttonImage = GetComponent<Image>();
        button =  GetComponent<UnityEngine.UI.Button>();
    }

    public void UpdateContent(ChartInfo chartInfo)
    {
        button.enabled = true;
        
        difficultyText.text = chartInfo.ChartDifficulty.ToString();
        difficultyNumberText.text = chartInfo.ChartDifficultyNumber.ToString();

        scoreText.text = chartInfo.ScoreSave.Score.ToString("D7");
        EvaluateText.text = ScoringSystem.Instance.GetEvaluate(chartInfo.ScoreSave.Score);

     
        
        if (chartInfo.ScoreSave.Score == ScoringSystem.FullMarks)
        {
            buttonColor = Color.yellow;
        }
        else if (chartInfo.ScoreSave.IsFullCombo == true)
        {
            buttonColor = Color.cyan;
        }
        else
        {
            buttonColor = Color.white;
        }
        
        buttonImage.color = new Color(0, 0, 0, 0.74f);
        scoreText.color = Color.white;
        EvaluateText.color = buttonColor;
    }

    public void Lock(ChartInfo chartInfo)
    {
        button.enabled = false;
        buttonImage.color = new Color(0.2f, 0.2f, 0.2f, 0.74f);
        difficultyText.text = chartInfo.ChartDifficulty.ToString();
        difficultyNumberText.text = "?";
        
        scoreText.text = "Locked";
        EvaluateText.text = "";
        buttonColor = new Color(0.2f, 0.2f, 0.2f, 1);
    }
    
    
    public void OnSelect()
    {
        StopAllCoroutines();
        StartCoroutine(OnSelectEnumerator());
        
        // buttonImage.color = new Color(1, 1, 1, buttonImage.color.a);
        // scoreText.color = Color.black;
        // EvaluateText.color = Color.black;
        // transform.localPosition = new Vector3(25,transform.localPosition.y,transform.localPosition.z);
        // transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
    }
    
    IEnumerator OnSelectEnumerator()
    {
        for(int i = 0; i < 11; i++)
        {
            float f = (i/10f)*(2-i/10f);

            float a = 0.74f + f*f * (0.05f - 0.74f);
            
            
            buttonImage.color = new Color(f, f, f, 0.05f);
  
            transform.localPosition = new Vector3(25*f,transform.localPosition.y,transform.localPosition.z);

            
            float s = (1.05f - 1f) * f + 1f;
            transform.localScale = new Vector3(s, s, s);
            
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void OnUnselect()
    {
        StopAllCoroutines();
        buttonImage.color = new Color(0, 0, 0, 0.74f);
        scoreText.color = Color.white;
        EvaluateText.color = buttonColor;
        transform.localPosition = new Vector3(0,transform.localPosition.y,transform.localPosition.z);
        transform.localScale = new Vector3(1, 1, 1);
    }

    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
