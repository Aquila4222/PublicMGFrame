using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingControl : MonoBehaviour
{
    public static SettingSave saveSettings;

    public TMP_Text speedText;

    public GameObject KeyInputCanvas;
    
    public Image IndicatorImage;
    
    public TMP_Text[] KeyTexts;
    public int waitingForKey = -1;
    
    public Scrollbar speedStroll;
    public static float Speed { get; set; }// = 0.1f;
    
    public static float NoteSpeed;

    void Awake()
    {
        saveSettings = Resources.Load<SettingSave>("Tracks/SettingSave");
        NoteSpeed = saveSettings.Speed;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Datas.IndicatorEnabled = saveSettings.IndicatorEnabled;
        IndicatorImage.enabled = saveSettings.IndicatorEnabled;
        
        
        
        Delay = saveSettings.Delay;
        DelayText.text = Delay.ToString();
        GameController.Instance.ChangeDelay(Delay);


        NoteSpeed = saveSettings.Speed;
        Datas.NoteSpeed = saveSettings.Speed;
        //NoteSpeed = ConstDatas.NoteSpeed;
        speedText.text = ((int)NoteSpeed).ToString();
        Speed  = (NoteSpeed-10)/20f;
        speedStroll.value = Speed;

        for (int i = 0; i < Datas.NoteTrackCount; i++)
        {
            InputSystem.HitKey[i] = saveSettings.Keys[i];

            KeyTexts[i].text = InputSystem.HitKey[i].ToString();
        }
        

        waitingForKey = -1;
        KeyInputCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        NoteSpeed = Speed*(20)+10;
        speedText.text = ((int)NoteSpeed).ToString();
        Datas.NoteSpeed = NoteSpeed;
        saveSettings.Speed = NoteSpeed;
    }

    // void LateUpdate()
    // {
    //     saveSettings.Speed = NoteSpeed;
    // }
    
    public TMP_InputField DelayText;
    
    public int Delay;
    
    public void ChangeMusicDelay(string delay)
    {
        int.TryParse(delay, out int delayInt);
        GameController.Instance.ChangeDelay(delayInt);
        Delay = delayInt;
        saveSettings.Delay = Delay;
    }
    
    public void ChangeKeyText(int keyIndex)
    {
        waitingForKey = keyIndex;
        KeyInputCanvas.SetActive(true);
    }

    public void ChangeIndicator()
    {
        if (Datas.IndicatorEnabled == false)
        {
            Datas.IndicatorEnabled = true;
            saveSettings.IndicatorEnabled = true;
            IndicatorImage.enabled =  true;
        }
        else
        {
            Datas.IndicatorEnabled = false;
            saveSettings.IndicatorEnabled = false;
            IndicatorImage.enabled = false;
        }
    }
    
    
    private void OnGUI()
    {
        if (waitingForKey != -1)
        {
            Event e = Event.current;

            if (e.isKey)
            {
                if (e.keyCode == KeyCode.Escape)
                {
                    waitingForKey = -1;
                    e.Use();
                    KeyInputCanvas.SetActive(false);
                }
                else
                {
                    Debug.Log(e.keyCode);
                    KeyTexts[waitingForKey].text = e.keyCode.ToString();
                    InputSystem.HitKey[waitingForKey] = e.keyCode;
                    saveSettings.Keys[waitingForKey] = e.keyCode;
                    
                    
                    waitingForKey = -1;
                    e.Use();
                    KeyInputCanvas.SetActive(false);
                }
            }
            else if (e.type == EventType.MouseDown)
            {
                KeyCode c;

                switch (e.button)
                {
                    case 0:
                        c = KeyCode.Mouse0;
                        break;
                    case 1:
                        c = KeyCode.Mouse1;
                        break;
                    case 2:
                        c = KeyCode.Mouse2;
                        break;
                    case 3:
                        c = KeyCode.Mouse3;
                        break;
                    case 4:
                        c = KeyCode.Mouse4;
                        break;
                    case 5:
                        c = KeyCode.Mouse5;
                        break;
                    case 6:
                        c = KeyCode.Mouse6;
                        break;
                    default:
                        c = KeyCode.None;
                        break;
                }
                
                Debug.Log(c);
                KeyTexts[waitingForKey].text = c.ToString();
                
                InputSystem.HitKey[waitingForKey] = c;
                saveSettings.Keys[waitingForKey] = c;
                
                waitingForKey = -1;
                e.Use();
                KeyInputCanvas.SetActive(false);
            }
        }
    }
    
    
}
