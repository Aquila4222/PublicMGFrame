using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CMNoteSystem : MonoBehaviour
{
    private static CMNoteSystem _instance;
    public static CMNoteSystem Instance => _instance;

    public List<NoteInfo> noteInfos = new List<NoteInfo>();
    public List<CmNote> cmNotes = new List<CmNote>();
    
    
    void Awake()
    {
        _instance = this;
        
        //生成测试铺面（应替换为读取铺面）
        // TestChart1 += $"{60f/170*1f}:0,t\n";
        // TestChart1 += $"{60f/170*1f}:1,h,{60f/170*1f}\n";
        // TestChart1 += $"{60f/170*1f}:2,d,{60f/170*2f}\n";

        //铺面加载为NoteList
        noteInfos = ChartTransform.ToNoteInfoList(CMTrackLoader.LoadChart());
        
    }

    /// <summary>
    /// 生成NoteList中所有NoteInfo的视觉物体
    /// </summary>
    public void LoadNotes()
    {
        GameObject noteObj;
        foreach (NoteInfo noteInfo in noteInfos)
        {
            noteObj = CMVisualNoteGenerator.Instance.GenerateNote(noteInfo);
            CmNote cmNote = new CmNote(noteInfo, noteObj);
            cmNotes.Add(cmNote);
        }
    }
    

    /// <summary>
    /// 移除Note
    /// </summary>
    /// <returns></returns>
    public bool RemoveNote()
    {
        Vector3 mouseposition = CMCalculater.Instance.MousePosition;
        int trackIndex = CMCalculater.Instance.CalculateTrack();
        if (trackIndex == -1)
        {
            return false;
        }

        float bottomTime = (mouseposition.y - CMConstDatas.RemoveYRange / 2) / CMConstDatas.DistancePerSecond;
        float topTime = (mouseposition.y + CMConstDatas.RemoveYRange / 2) / CMConstDatas.DistancePerSecond;
        
        foreach (CmNote cmNote in cmNotes.ToList())
        {
            if (cmNote.NoteInfo.TrackIndex == trackIndex && cmNote.NoteInfo.TimePoint >= bottomTime &&
                cmNote.NoteInfo.TimePoint <= topTime)
            {
                cmNotes.Remove(cmNote);
                noteInfos.Remove(cmNote.NoteInfo);
                Destroy(cmNote.NoteObj);
            }
            
        }
        //保存
        CMTrackLoader.SaveChart(ChartTransform.ListToString(CMNoteSystem.Instance.noteInfos));
        return true;
    }
    
    /// <summary>
    /// 生成Tap
    /// </summary>
    /// <returns></returns>
    public bool AddTap()
    {
        int trackIndex = CMCalculater.Instance.CalculateTrack();
        if (trackIndex == -1)
        {
            return false;
        }
        float time = CMCalculater.Instance.CalculateTime(CMControlSystem.Instance.Bpm,
            CMControlSystem.Instance.CurrentDevidingCount);
        NoteInfo noteInfo = new NoteInfo(NoteType.Tap, trackIndex, time,0);
        return AddCmNote(noteInfo);
    }
    
    /// <summary>
    /// 生成Hold或者Dash
    /// </summary>
    /// <param name="trackIndex"></param>
    /// <param name="startTime"></param>
    /// <param name="noteType"></param>
    /// <returns></returns>
    public bool AddHoldOrDash(int trackIndex, float startTime, NoteType noteType)
    {
        float endTime = CMCalculater.Instance.CalculateTime(CMControlSystem.Instance.Bpm,
            CMControlSystem.Instance.CurrentDevidingCount);
        if (endTime < startTime)
        {
            return false;
        }
        NoteInfo noteInfo = new NoteInfo(noteType, trackIndex, startTime,endTime-startTime);
        return AddCmNote(noteInfo);
    }
    
    /// <summary>
    /// 输入NoteInfo，生成NoteObj和CmMote
    /// </summary>
    /// <param name="noteInfo"></param>
    /// <returns></returns>
    private bool AddCmNote(NoteInfo noteInfo)
    {
        foreach (NoteInfo info in noteInfos)
        {
            if (info.TimePoint - noteInfo.TimePoint is > -0.001f and < 0.001f)
            {
                if (info.TrackIndex == noteInfo.TrackIndex)
                {
                    if (info.Type != NoteType.Dash && noteInfo.Type != NoteType.Dash)
                    {
                        return false;
                    }

                    if (info.Type == NoteType.Dash && noteInfo.Type == NoteType.Dash)
                    {
                        if (info.HoldingTime - noteInfo.HoldingTime  is > -0.001f and < 0.001f)
                        {
                            return false;
                        }
                    }
                }
            }
        }
        
        
        if (noteInfos.Contains(noteInfo))
        {
            return false;
        }
        noteInfos.Add(noteInfo);
        GameObject noteObj =CMVisualNoteGenerator.Instance.GenerateNote(noteInfo);
        CmNote cmNote = new CmNote(noteInfo, noteObj);
        cmNotes.Add(cmNote);
        //保存
        CMTrackLoader.SaveChart(ChartTransform.ListToString(CMNoteSystem.Instance.noteInfos));
        return true;
    }
    
}
