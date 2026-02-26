using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[AutoSaveSO]
[CreateAssetMenu(fileName = "ScoreSave", menuName = "Save/ScoreSave")]
public class ScoreSave : ScriptableObject
{
    [Header("分数")]
    public int Score;
    
    [Header("是否全连")]
    public bool IsFullCombo;

    public void ResetData()
    {
        Score = 0;
        IsFullCombo = false;
    }
}