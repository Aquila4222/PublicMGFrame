using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public static class ChartTransform
{
    /// <summary>
    /// 将NoteInfo列表转为字符串
    /// </summary>
    /// <param name="notesInfos"></param>
    /// <returns></returns>
    public static string ListToString(List<NoteInfo> notesInfos)
    {
        List<NoteInfo> orderedList = notesInfos.OrderBy(x => x.TimePoint).ToList();
        
        string chart = "";
        foreach (NoteInfo noteInfo in orderedList)
        {
            chart += noteInfo.TimePoint.ToString(CultureInfo.InvariantCulture);
            chart += ":";
            chart += noteInfo.TrackIndex.ToString(CultureInfo.InvariantCulture);
            chart += ",";
            switch (noteInfo.Type)
            {
                case NoteType.Tap:
                    chart += "t";
                    break;
                case NoteType.Up:
                    chart += "↑";
                    break;
                case NoteType.Down:
                    chart += "↓";
                    break;
                case NoteType.Left:
                    chart += "←";
                    break;
                case NoteType.Right:
                    chart += "→";
                    break;
                case NoteType.Hold:
                    chart += "h";
                    break;
                case NoteType.Dash:
                    chart += "d";
                    break;
            }
            switch (noteInfo.Type)
            {
                case NoteType.Hold:
                case NoteType.Dash:
                    chart += ",";
                    chart += noteInfo.HoldingTime.ToString(CultureInfo.InvariantCulture);
                    break;
            }
            chart += "\n";
        }
        return chart;
    }
    
    /// <summary>
    /// 将字符串转为NoteInfo列表
    /// </summary>
    /// <param name="chart">谱面string</param>
    /// <returns></returns>
    /// <exception cref="NoteException">谱面读取异常</exception>
    public static List<NoteInfo> ToNoteInfoList(string chart)
    {
        try
        {
            return UnsafeToNoteInfoList(chart);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            throw;
        }
    }
    
    //Unsafe
    private static List<NoteInfo> UnsafeToNoteInfoList(string chart)
    {
        List<NoteInfo> notesInfos = new List<NoteInfo>();
        
        //将行切割
        string[] lines = chart.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        
        for (int i = 0; i < lines.Length; i++)
        {
            //创建结构体
            NoteInfo noteInfo = new NoteInfo();
            
            //切割时间点信息
            string[] firstSplit = lines[i].Split(':');
            
            //尝试录入时间点信息
            if (float.TryParse(firstSplit[0], out noteInfo.TimePoint))
            {
    
            }
            else
            {
                throw new NoteException($"第{i+1}行：判定时间点读取异常！");
            }
            
            //尝试录入Note信息
            if (firstSplit.Length < 2)
            {
                throw new NoteException($"第{i+1}行：不存在Note！");
            }
            string[] thirdSplit = firstSplit[1].Split(',');
            

            //尝试录入轨道信息
            if (int.TryParse(thirdSplit[0], out noteInfo.TrackIndex))
            {
                        
            }
            else
            {
                throw new NoteException($"第{i+1}行：Note轨道读取异常！");
            }

            //检测轨道信息
            if (noteInfo.TrackIndex < 0 || noteInfo.TrackIndex >= Datas.NoteTrackCount)
            {
                throw new NoteException($"第{i+1}行：Note轨道超出限制！");
            }


            //尝试录入类别信息
            if (thirdSplit.Length < 2)
            {
                throw new NoteException($"第{i+1}行：Note缺少类别信息！");
            }
            switch (thirdSplit[1])
            {
                case "t":
                    noteInfo.Type = NoteType.Tap;
                    break;
                case "↑":
                    noteInfo.Type = NoteType.Up;
                    break;
                case "↓":
                    noteInfo.Type = NoteType.Down;
                    break;
                case "←":
                    noteInfo.Type = NoteType.Left;
                    break;
                case "→":
                    noteInfo.Type = NoteType.Right;
                    break;
                case "h":
                {
                    noteInfo.Type = NoteType.Hold;
                
                    //尝试录入hold时间
                    if (thirdSplit.Length < 3)
                    {
                        throw new NoteException($"第{i+1}行：Note（hold）缺少Hold信息！");
                    }
                
                    if (float.TryParse(thirdSplit[2], out noteInfo.HoldingTime))
                    {
    
                    }
                    else
                    {
                        throw new NoteException($"第{i+1}行：HoldingTime读取异常！");
                    }

                    break;
                }
                case "d":
                {
                    noteInfo.Type = NoteType.Dash;
                
                    //尝试录入dash时间
                    if (thirdSplit.Length < 3)
                    {
                        throw new NoteException($"第{i+1}行：Note（dash）缺少Hold信息！");
                    }
                
                    if (float.TryParse(thirdSplit[2], out noteInfo.HoldingTime))
                    {
    
                    }
                    else
                    {
                        throw new NoteException($"第{i+1}行：HoldingTime读取异常！");
                    }

                    break;
                }
            }
            
            
            //添加Note信息到表
            notesInfos.Add(noteInfo);
        }
        return  notesInfos;
    }
}

public struct NoteInfo 
{
    public NoteType Type;
    public int TrackIndex;
    public float TimePoint;
    public float HoldingTime;

    public NoteInfo(NoteType type,int trackIndex,float timePoint,float holdingTime)
    {
        Type =  type;
        TrackIndex = trackIndex;
        TimePoint = timePoint;
        HoldingTime = holdingTime;
    }
}