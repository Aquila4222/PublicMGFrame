using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMConstDatas
{
    public const float BasicDps = 8f;
    public static float DistancePerSecond = 8f;
    public static float AudioDelay = 0.21f;
    public static readonly int[] DividingCounts = { 2, 3, 4, 6, 8 };
    public const float FirstTrackX = -3.934661f;
    public const float TrackXDistance = 1.699345f;
    public const float RemoveYRange = 0.25f;

}

public struct CmNote
{
    public NoteInfo NoteInfo;
    public GameObject NoteObj;

    public CmNote(NoteInfo noteInfo, GameObject noteObj)
    {
        NoteInfo = noteInfo;
        NoteObj = noteObj;
    }
    
}