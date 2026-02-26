using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Datas
{
    /// <summary>
    /// 轨道数量
    /// </summary>
    public const int NoteTrackCount = 4;
    
    /// <summary>
    /// Perfect判定偏差（正负）
    /// </summary>
    public const float PerfectOffset = 0.060f;
    
    /// <summary>
    /// Good判定偏差（正负）
    /// </summary>
    public const float GoodOffset = 0.100f;
    
    /// <summary>
    /// Bad判定偏差（正负）
    /// </summary>
    public const float BadOffset = 0.150f;
    
    /// <summary>
    /// hold可以提前松开的判定时间
    /// </summary>
    public const float HoldEarlyReleaseWindow = 0.3f;
    
    /// <summary>
    /// 提前放置Note的时间
    /// </summary>
    public const float EarlyPlaceNoteTime = 3f;
    
    public static float Delay = 0.21f;

    public static float NoteSpeed = 10;
    
    public static bool IndicatorEnabled = false;
}
