using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 输入类型枚举
/// </summary>
public enum HitType
{
    None,
    Down,
    Hold,
    Up
}

public struct Hit
{
    public int HitPos;
    
    public HitType HitType;
    
    public Hit(int hitPos, HitType hitType)
    {
        HitPos = hitPos;
        HitType = hitType;
    }
}


public class NotesTrack
{
    /// <summary>
    /// 存放参与点击判定的Note
    /// </summary>
    private readonly List<Note> tapNotes = new List<Note>();

    /// <summary>
    /// 存放需要参与判定的hold
    /// </summary>
    private readonly List<Note> holdingNotes =  new List<Note>();

    /// <summary>
    /// 存放还未判定的Dash
    /// </summary>
    private readonly List<Note> dashNotes =  new List<Note>();

    
    private readonly float[] dashTimer = new float[4];

    

    /// <summary>
    /// 加入点击判定的列表
    /// </summary>
    /// <param name="note"></param>
    public void EnterTapList(Note note)
    {
        tapNotes.Add(note);
    }
    
    /// <summary>
    /// 加入Hold列表
    /// </summary>
    /// <param name="note"></param>
    public void EnterHoldList(Note note)
    {
        holdingNotes.Add(note);
    }

    /// <summary>
    /// 加入Dash列表
    /// </summary>
    /// <param name="note"></param>
    public void EnterDashList(Note note)
    {
        dashNotes.Add(note);
    }

    /// <summary>
    /// 当前轨道更新逻辑，需要传入输入类型（此方法需要频繁调用）
    /// </summary>
    /// <param name="hits"></param>
    /// <param name="timeTrack"></param>
    public void TrackUpdate(Hit[] hits , float timeTrack)
    {
        //尝试打击note
        foreach (Hit hit in hits)
        {
            if (hit.HitType == HitType.Down)
            {
                TryHit(hit,timeTrack);
            }
        }
        //判断Note是否miss
        MissUpdate(timeTrack);
        
        //更新dash
        DashUpdate(hits,timeTrack);
        
        //更新hold
        HoldUpdate(hits,timeTrack);
    }

    /// <summary>
    /// AutoPlay轨道更新
    /// </summary>
    /// <param name="timeTrack"></param>
    public void AutoPlayUpdate(float timeTrack)
    {
        TryAutoHit(timeTrack);
        
        Hit[] autoHits = new Hit[Datas.NoteTrackCount];
        for (int i = 0; i < autoHits.Length; i++)
        {
            autoHits[i] =  new Hit(i, HitType.Hold);
        }
        DashUpdate(autoHits,timeTrack);
        
        //更新hold
        HoldUpdate(autoHits,timeTrack);
    }
    
    /// <summary>
    /// Miss判定相关
    /// </summary>
    private void MissUpdate(float timeTrack)
    {
        //miss判定
        while (tapNotes.Count != 0 && tapNotes[0].IsMiss(timeTrack) == true)
        {
            if (tapNotes[0].NoteType == NoteType.Hold)
            {
                holdingNotes.Add(tapNotes[0]);
            }
            tapNotes[0].JudgeHitResult(JudgmentResult.Miss);//使Note Miss
            
            tapNotes.RemoveAt(0);
        }

        //判断Dash的miss
        foreach (Note dash in dashNotes.ToList())
        {
            if (dash.IsMiss(timeTrack) == true)
            {
                dash.JudgeHitResult(JudgmentResult.Miss);//使Note Miss
                dashNotes.Remove(dash);
            }
        }
    }

    /// <summary>
    /// 点击指令，当触发点击时调用
    /// </summary>
    private void TryHit(Hit hit , float timeTrack)
    {
        if (tapNotes.Count == 0)
        {
            return;
        }

        foreach (Note note in tapNotes.ToList())
        {
            if (note.Hitable(hit,timeTrack) == true)
            {
                note.JudgeHitResult(timeTrack);//击中Note的调用
                if (note.NoteType == NoteType.Hold)
                {
                    holdingNotes.Add(note);
                }
                tapNotes.Remove(note);
                
                break;
            }
        }
    }


    /// <summary>
    /// AutoPlay尝试打击
    /// </summary>
    /// <param name="timeTrack"></param>
    private void TryAutoHit(float timeTrack)
    {
        if (tapNotes.Count == 0)
        {
            return;
        }

        foreach (Note note in tapNotes.ToList())
        {
            if (note.AutoHitable(timeTrack) == true)
            {
                note.JudgeHitResult(JudgmentResult.Perfect);//击中Note的调用
                if (note.NoteType == NoteType.Hold)
                {
                    holdingNotes.Add(note);
                }
                tapNotes.Remove(note);
                
                break;
            }
        }
    }

    /// <summary>
    /// 更新需要长按的逻辑（需要频繁调用）
    /// </summary>
    /// <param name="hits"></param>
    /// <param name="timeTrack"></param>
    private void HoldUpdate(Hit[] hits , float timeTrack)
    {
        foreach (Note holdingNote in holdingNotes.ToList())
        {
            if (holdingNote.IsHoldEnd(timeTrack) == true)
            {
                holdingNote.JudgeHoldResult();
                holdingNotes.Remove(holdingNote);
            }
        }

        //实现更新Hold的逻辑
        foreach (Note holdingNote in holdingNotes.ToList())
        {
            if (holdingNote.IsHolding(hits) == false)
            {
                holdingNote.JudgeHoldResult(JudgmentResult.Miss);
                holdingNotes.Remove(holdingNote);
            }
        }
    }

    /// <summary>
    /// 更新Dash列表
    /// </summary>
    /// <param name="hits"></param>
    /// <param name="timeTrack"></param>
    private void DashUpdate(Hit[] hits , float timeTrack)
    {
        foreach (Hit hit in hits)
        {
            if (hit.HitType == HitType.Down || hit.HitType == HitType.Hold)
            {
                dashTimer[hit.HitPos] = Datas.GoodOffset;
            }
        }
        

        for (int i = 0; i < dashTimer.Length; i++)
        {
            if (dashTimer[i] >= 0)
            {
                dashTimer[i] -=  Time.deltaTime;
                foreach (Note dash in dashNotes.ToList())
                {
                    if (dash.Hitable(new Hit(i,HitType.Down),timeTrack) == true)
                    {
                        dash.JudgeHitResult(JudgmentResult.Perfect);//击中Note的调用
                        holdingNotes.Add(dash);
                        dashNotes.Remove(dash);
                    }
                }
            }
        }
    }
    
    
    /// <summary>
    /// 清空
    /// </summary>
    public void Clear()
    {
        foreach (Note note in tapNotes)
        {
            note.Destroy();
        }
        foreach (Note note in holdingNotes)
        {
            note.Destroy();
        }
        foreach (Note note in dashNotes)
        {
            note.Destroy();
        }
        
        
        tapNotes.Clear();
        holdingNotes.Clear();
        dashNotes.Clear();
    }
}
    
