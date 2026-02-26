using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChartDif
{
    EZ,
    ST,
    HD,
    EX
}


[CreateAssetMenu(fileName = "ChartInfo" , menuName = "Info/ChartInfo")]
public class ChartInfo : ScriptableObject
{
    [Header("谱师")]
    public string ChartArtist;
    [Header("难度")]
    public ChartDif ChartDifficulty;
    [Header("定数")]
    public int ChartDifficultyNumber;
    [Header("谱面文件")]
    public TextAsset ChartText;
    [Header("保存的文件")]
    public ScoreSave ScoreSave;
    
    /// <summary>
    /// 谱面文本
    /// </summary>
    [HideInInspector] public string ChartContent;
}
