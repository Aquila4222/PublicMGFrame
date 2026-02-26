using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackLoader : SingletonHungry<TrackLoader>
{
    private TrackInfo[] tracks;
    
    public TrackInfo[] Tracks =>  tracks;

    /// <summary>
    /// 加载乐曲信息
    /// </summary>
    public void LoadTracks()
    {
        tracks = Resources.LoadAll<TrackInfo>("Tracks");
        foreach (TrackInfo track in tracks)
        {
            foreach (ChartInfo chartInfo in track.ChartInfos)
            {
                if (chartInfo.ChartText)
                {
                    chartInfo.ChartContent = chartInfo.ChartText.text;
                }
            }
        }
    }
}
