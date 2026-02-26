using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 判定结果枚举
/// </summary>
public enum JudgmentResult
{
    Perfect,
    Early,
    Late,
    Bad,
    Miss,
    Unknown
}

/// <summary>
/// Note类型枚举
/// </summary>
public enum NoteType
{
    Tap,
    Hold,
    Dash,
    Left,
    Right,
    Up,
    Down,
}

public class Note
{
    /// <summary>
    /// Note类型
    /// </summary>
    public NoteType NoteType {get; private set; }

    /// <summary>
    /// Note轨道
    /// </summary>
    private int trackIndex;

    /// <summary>
    /// 判定时间点
    /// </summary>
    private float judgmentTimePoint;
    
    /// <summary>
    /// hold的持续时间
    /// </summary>
    private float holdingTime;

    /// <summary>
    /// 判定结果
    /// </summary>
    private JudgmentResult judgmentResult;
    
    /// <summary>
    /// Note视觉效果的引用
    /// </summary>
    private NoteVE noteVE;
    
    /// <summary>
    /// 无参构造
    /// </summary>
    public Note()
    {
   
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="noteInfo">Note信息</param>
    public Note(NoteInfo noteInfo)
    {
        this.NoteType =  noteInfo.Type;
        this.trackIndex = noteInfo.TrackIndex;
        this.judgmentTimePoint = noteInfo.TimePoint;
        this.holdingTime = noteInfo.HoldingTime;
        
        //生成Note视觉效果
        noteVE = NoteVEPool.Instance.PlaceNoteVE(noteInfo);
    }
    
    /// <summary>
    /// Note被击中时的方法（自行判断结果）
    /// </summary>
    public void JudgeHitResult(float time)
    {
        JudgeHitResult(GetResult(time));
    }
    /// <summary>
    /// Note被击中时的方法（直接传入结果）
    /// </summary>
    /// <param name="result"></param>
    public void JudgeHitResult(JudgmentResult result)
    {
        //存储判定结果
        judgmentResult = result;
        
        //调用计分系统并传入判定结果（如果是Tap）
        if ((NoteType is NoteType.Tap or NoteType.Up or NoteType.Down or NoteType.Left or NoteType.Right) || result == JudgmentResult.Miss)
        {
            ScoringSystem.Instance.AddJudgment(result);
        }
        
        //调用Note视觉实例的OnHit方法
        noteVE.OnHit(result);
        
        if (result == JudgmentResult.Miss)
        {
            noteVE.HoldMiss();
        }
    }
    
    /// <summary>
    /// hold判定时方法（自行判断结果）
    /// </summary>
    public void JudgeHoldResult()
    {
        JudgeHoldResult(judgmentResult);
    }
    /// <summary>
    /// hold判定时方法（直接传入结果）
    /// </summary>
    /// <param name="result"></param>
    public void JudgeHoldResult(JudgmentResult result)
    {
        //调用计分系统并传入判定结果
        ScoringSystem.Instance.AddJudgment(result);
        
        //调用Note视觉实例的OnHit方法
        if (result == JudgmentResult.Miss)
        {
            noteVE.HoldMiss();
        }
    }
    
    /// <summary>
    /// 计算判定结果
    /// </summary>
    /// <param name="time">当前时间轴时间</param>
    /// <returns>判定结果</returns>
    private JudgmentResult GetResult(float time)
    {
        float offset = time - judgmentTimePoint;
            
        if (offset >= -Datas.PerfectOffset && offset <= Datas.PerfectOffset)
        {
            return JudgmentResult.Perfect;
        }
        else if (offset >= -Datas.GoodOffset && offset < -Datas.PerfectOffset)
        {
            //Dash宽松判定
            if (NoteType == NoteType.Dash) {return JudgmentResult.Perfect;}

            return JudgmentResult.Early;
        }
        else if (offset > Datas.PerfectOffset && offset <= Datas.GoodOffset)
        {
            //Dash宽松判定
            if (NoteType == NoteType.Dash) {return JudgmentResult.Perfect;}
            
            return JudgmentResult.Late;
        }
        else if (offset >= -Datas.BadOffset && offset < -Datas.GoodOffset)
        {
            return JudgmentResult.Bad;
        }
        else if (offset > Datas.GoodOffset)
        {
            return JudgmentResult.Miss;
        }
        else
        {
            return JudgmentResult.Unknown;
        }
    }
    
    /// <summary>
    /// 判断Note是否可以被击中的方法
    /// </summary>
    /// <param name="hit">打击结构体</param>
    /// <param name="time">时间轨时间</param>
    /// <returns></returns>
    public bool Hitable(Hit hit , float time)
    {
        if (hit.HitPos != trackIndex)
        {
            return false;
        }
        
        //判定是否处于miss~bad的可判定状态逻辑
        switch (NoteType)
        {
            case NoteType.Tap: case NoteType.Up: case NoteType.Down: case NoteType.Left: case NoteType.Right:
                return time-judgmentTimePoint >= -Datas.BadOffset && time-judgmentTimePoint <= Datas.GoodOffset;
            case NoteType.Hold:
                return time-judgmentTimePoint >= -Datas.GoodOffset && time-judgmentTimePoint <= Datas.GoodOffset;
            case NoteType.Dash:
                return time-judgmentTimePoint >= 0 && time-judgmentTimePoint <= Datas.GoodOffset;
            default:
                return true;
        }
    }

    /// <summary>
    /// AutoPlay的判定检测
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public bool AutoHitable(float time)
    {
        return time - judgmentTimePoint >= 0;
    }
    
    
    /// <summary>
    /// 判断Note是否miss的方法
    /// </summary>
    /// <returns></returns>
    public bool IsMiss(float time)
    {
        //判定是否miss的逻辑
        return time-judgmentTimePoint > Datas.GoodOffset;
    }
    
    /// <summary>
    /// 判定hold类Note是否结束
    /// </summary>
    /// <returns></returns>
    public bool IsHoldEnd(float time)
    {
        return time > judgmentTimePoint+holdingTime - Datas.HoldEarlyReleaseWindow;
    }

    /// <summary>
    /// 判断Hold是否按住的方法
    /// </summary>
    /// <param name="hits"></param>
    /// <returns></returns>
    public bool IsHolding(Hit[] hits)
    {
        bool isHolding = false;
        foreach (Hit hit in hits)
        {
            if (hit.HitPos == trackIndex && (hit.HitType == HitType.Hold|| hit.HitType == HitType.Down))
            {
                isHolding = true;
            }
        }
        return isHolding;
    }
    
    /// <summary>
    /// 销毁Note的方法
    /// </summary>
    public void Destroy()
    {
        if (noteVE)
            noteVE.ReturnNote();
    }
}
