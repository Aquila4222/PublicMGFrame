using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringSystem : SingletonHungry<ScoringSystem>
{
    /// <summary>
    /// 最大分数
    /// </summary>
    public const int FullMarks = 1000000;

    /// <summary>
    /// Good减分系数
    /// </summary>
    private const float GoodCoefficient = 0.8f;
    private const int intGoodCoefficient = 80;

    /// <summary>
    /// 当前分数（浮点）
    /// </summary>
    private float currentScore;

    /// <summary>
    /// Note数量（需在初始化的时候传入）
    /// </summary>
    private int notesNumber;

    /// <summary>
    /// 不同分段Note的数量
    /// </summary>
    private readonly int[] NotesJudgmentCount = new int[10];
    
    /// <summary>
    /// 当前整数分数（四舍五入后）
    /// </summary>
    public int Score => (int)(currentScore + 0.5f);
    
    /// <summary>
    /// 当前评价
    /// </summary>
    public string Evaluate => GetEvaluate(Score);

    /// <summary>
    /// 当前Combo数
    /// </summary>
    public int ComboCount { get; private set; }

    /// <summary>
    /// 最大Combo数
    /// </summary>
    public int MaxCombo { get; private set; }
    
    /// <summary>
    /// 是否AP
    /// </summary>
    public bool IsAllPerfect => NotesJudgmentCount[ChangeToInt(JudgmentResult.Early)] == 0
                                && NotesJudgmentCount[ChangeToInt(JudgmentResult.Late)] == 0
                                && NotesJudgmentCount[ChangeToInt(JudgmentResult.Bad)] == 0
                                && NotesJudgmentCount[ChangeToInt(JudgmentResult.Miss)] == 0
                                && NotesJudgmentCount[ChangeToInt(JudgmentResult.Unknown)] == 0;
    
    /// <summary>
    /// 是否FC
    /// </summary>
    public bool IsFullCombo => NotesJudgmentCount[ChangeToInt(JudgmentResult.Bad)] == 0
                               && NotesJudgmentCount[ChangeToInt(JudgmentResult.Miss)] == 0
                               && NotesJudgmentCount[ChangeToInt(JudgmentResult.Unknown)] == 0;

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="number"></param>
    public void Initialize(int number)
    {
        notesNumber = number;

        currentScore = 0;
        
        ComboCount = 0;

        MaxCombo = 0;

        for (int i = 0; i < NotesJudgmentCount.Length; i++)
        {
            NotesJudgmentCount[i] = 0;
        }
    }

    /// <summary>
    /// 添加判定结果
    /// </summary>
    /// <param name="judgment">判定结果</param>
    public void AddJudgment(JudgmentResult judgment)
    {
        NotesJudgmentCount[ChangeToInt(judgment)]++;

        //currentScore += ExtraScore(judgment);
        
        currentScore = CaculateScore();

        ComboJudgement(judgment);
    }

    /// <summary>
    /// 获取当前不同判定的Note的数量
    /// </summary>
    /// <param name="judgment"></param>
    /// <returns></returns>
    public int GetJudgmentCount(JudgmentResult judgment)
    {
        return NotesJudgmentCount[ChangeToInt(judgment)];
    }

    public string GetEvaluate(int score)
    {
        if (score == 1000000)
            return "P";
        if (score >= 990000)
            return "S";
        if (score >= 970000)
            return "A";
        if (score >= 950000)
            return "B";
        if (score >= 930000)
            return "C";
        if (score >= 900000)
            return "D";
        if (score >= 800000)
            return "E";
        if (score > 0)
            return "F";
        else
            return "";
    }
    
    /// <summary>
    /// 判定Combo
    /// </summary>
    /// <param name="judgment"></param>
    private void ComboJudgement(JudgmentResult judgment)
    {
        switch (judgment)
        {
            case JudgmentResult.Perfect:
            case JudgmentResult.Early:
            case JudgmentResult.Late:
                ComboCount++;
                if (MaxCombo < ComboCount)
                {
                    MaxCombo = ComboCount;
                }

                break;
            case JudgmentResult.Bad:
            case JudgmentResult.Miss:
                ComboCount = 0;
                break;
            default:
                ComboCount = -notesNumber;
                break;
        }
    }



    private float CaculateScore()
    {
        float coefficient = 0;
        coefficient += NotesJudgmentCount[ChangeToInt(JudgmentResult.Perfect)]*100;
        coefficient += NotesJudgmentCount[ChangeToInt(JudgmentResult.Early)]*intGoodCoefficient;
        coefficient += NotesJudgmentCount[ChangeToInt(JudgmentResult.Bad)]*intGoodCoefficient;
        return FullMarks*coefficient/(100*notesNumber);
    }
    

    /// <summary>
    /// 将判定结果转int
    /// </summary>
    /// <param name="judgment"></param>
    /// <returns></returns>
    private int ChangeToInt(JudgmentResult judgment)
    {
        switch (judgment)
        {
            case JudgmentResult.Perfect:
                return 0;
            case JudgmentResult.Early:
                return 1;
            case JudgmentResult.Late:
                return 2;
            case JudgmentResult.Bad:
                return 3;
            case JudgmentResult.Miss:
                return 4;
            default:
                return 5;
        }
    }

    /// <summary>
    /// 将int转为判定结果
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private JudgmentResult ChangeToJudgment(int index)
    {
        switch (index)
        {
            case 0:
                return JudgmentResult.Perfect;
            case 1:
                return JudgmentResult.Early;
            case 2:
                return JudgmentResult.Late;
            case 3:
                return JudgmentResult.Bad;
            case 4:
                return JudgmentResult.Miss;
            default:
                return JudgmentResult.Unknown;
        }
    }
}