using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[AutoSaveSO]
[CreateAssetMenu(fileName = "SettingSave", menuName = "Save/SettingSave")]
public class SettingSave : ScriptableObject
{
    public float Speed;
    public int Delay;
    
    public KeyCode[] Keys;
    
    public bool IndicatorEnabled;

    public void ResetData()
    {
        Speed = 12;
        Delay = 160;
        Keys[0] = KeyCode.D;
        Keys[1] = KeyCode.F;
        Keys[2] = KeyCode.J;
        Keys[3] = KeyCode.K;
        
        IndicatorEnabled = false;
    }
}