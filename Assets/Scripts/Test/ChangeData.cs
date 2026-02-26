using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DelayText.text = GameController.Delay.ToString();
        
        NoteSpeed = Datas.NoteSpeed;
        speedText.text = ((int)NoteSpeed).ToString();
        Speed  = (NoteSpeed-10)/20f;
        speedStroll.value = Speed;
    }

    // Update is called once per frame
    void Update()
    {
        HitSEPool.Instance.Volume = SEVolume;

        NoteSpeed = Speed*(20)+10;
        speedText.text = ((int)NoteSpeed).ToString();
        //ConstDatas.NoteSpeed = NoteSpeed;
    }

    public int Delay;

    public float SEVolume { get; set; } = 1;



    public float Speed { get; set; } = 0.1f;

    public float NoteSpeed;
    
    public TMP_Text speedText;

    public Scrollbar speedStroll;

    public TMP_InputField DelayText;
    
    
    
    public void ChangeMusicDelay(string delay)
    {
        int.TryParse(delay, out int delayInt);
        GameController.Instance.ChangeDelay(delayInt);
        Delay = delayInt;
    }
    
}
