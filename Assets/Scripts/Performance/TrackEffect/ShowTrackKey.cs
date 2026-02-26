using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTrackKey : MonoBehaviour
{
    public TextMesh[] KeyTexts;

    private const int maxLength = 3;
    
    private void Start()
    {
        for (int i = 0; i < Datas.NoteTrackCount; i++)
        {
            string fullText = InputSystem.HitKey[i].ToString();

            if (fullText.Contains("Alpha"))
            {
                fullText = fullText.Replace("Alpha", "");
            }
            if (fullText.Contains("Keypad"))
            {
                fullText = fullText.Replace("Keypad", "K");
            }
            
            KeyTexts[i].text =  fullText.Length > maxLength ? fullText.Substring(0, maxLength) : fullText;
        }
    }
}
