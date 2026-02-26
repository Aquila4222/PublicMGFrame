using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ArrowNoteInfo
{
    public NoteType noteType;
    public float TimePoint;
    public int VTrackIndex;

    public ArrowNoteInfo(NoteType noteType, float timePoint, int vTrackIndex)
    {
        this.noteType = noteType;
        this.TimePoint = timePoint;
        this.VTrackIndex = vTrackIndex;
    }
}


public class TrackVE : MonoBehaviour
{
    #region Mono饿汉单例代码

    private static TrackVE _instance;
    public static TrackVE Instance => _instance;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeRuntime()
    {
        GameObject obj = new GameObject("TrackVE");
        _instance = obj.AddComponent<TrackVE>();
        DontDestroyOnLoad(obj);
    }
    
    #endregion

    private int BPM;

    private float timeTrack;
    
    /// <summary>
    /// 视觉轨道顺序
    /// </summary>
    public int[] VTrackPosIndex = new int[Datas.NoteTrackCount];
    
    //public int[] PlaceVTrackPosIndex = new int[ConstDatas.NoteTrackCount];
    
    /// <summary>
    /// 视觉轨道具体位置
    /// </summary>
    public float[] VTrackPos = new float[Datas.NoteTrackCount];

    private readonly float[] VTrackFixedPos = {-2.4f,-0.8f,0.8f,2.4f};

    private readonly List<ArrowNoteInfo> HorizontalChange = new List<ArrowNoteInfo>();

    public float Speed = 50f;


    /// <summary>
    /// 加入换轨效果
    /// </summary>
    /// <param name="arrowNoteInfo"></param>
    public void AddArrowNote(ArrowNoteInfo arrowNoteInfo)
    {
        HorizontalChange.Add(arrowNoteInfo);
        
        //ChangeHorizontal(arrowNoteInfo, PlaceVTrackPosIndex);
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="bpm"></param>
    public void Initialize(int bpm)
    {
        BPM = bpm;
        
        HorizontalChange.Clear();
        
        for (int i = 0; i < VTrackPosIndex.Length; i++)
        {
            VTrackPosIndex[i] = i;
            VTrackPos[i] = VTrackFixedPos[i];
            
            //PlaceVTrackPosIndex[i] = i;
        }
    }

    private int GetVTrackIndex(int trackPosIndex , int[] indexs)
    {
        if (indexs.Length != VTrackPosIndex.Length)
        {
            return -1;
        }
        
        for (int i = 0;  i < indexs.Length; i++)
        {
            if (indexs[i] == trackPosIndex)
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// 改变水平轨道下标
    /// </summary>
    /// <param name="arrowNoteInfo"></param>
    /// <param name="indexs"></param>
    private void ChangeHorizontal(ArrowNoteInfo arrowNoteInfo , int[] indexs)
    {
        if (indexs.Length != VTrackPosIndex.Length)
        {
            return;
        }
        
        int thisTrackPosIndex = indexs[arrowNoteInfo.VTrackIndex];
        int otherTrackPosIndex = 0;
 
        
        if (arrowNoteInfo.noteType == NoteType.Right)
        {
            if (thisTrackPosIndex != indexs.Length - 1)
            {
                otherTrackPosIndex = thisTrackPosIndex + 1;
            }
            else
            {
                otherTrackPosIndex = 0;
            }
        }
        else if (arrowNoteInfo.noteType == NoteType.Left)
        {
            if (thisTrackPosIndex != 0)
            {
                otherTrackPosIndex = thisTrackPosIndex - 1;
            }
            else
            {
                otherTrackPosIndex = indexs.Length - 1;
            }
        }
        else
        {
            return;
        }

    
        int otherVTrackIndex = GetVTrackIndex(otherTrackPosIndex,indexs);
      
        Debug.Log(thisTrackPosIndex+","+otherTrackPosIndex);
        
        Debug.Log(arrowNoteInfo.VTrackIndex+","+otherVTrackIndex);
        
        indexs[otherVTrackIndex] =  thisTrackPosIndex;

        indexs[arrowNoteInfo.VTrackIndex] = otherTrackPosIndex;

    }

    public int CalculateVTrackIndex(int trackIndex)
    {
        int[] indexs = new int[VTrackPosIndex.Length];
        for (int i = 0; i < VTrackPosIndex.Length; i++)
        {
            indexs[i] = VTrackPosIndex[i];
        }
 
        
     
        
        foreach (var change in HorizontalChange)
        {
            ChangeHorizontal(change, indexs);
        }

        
        
        for (int i = 0;  i < indexs.Length; i++)
        {
            if (indexs[i] == trackIndex)
            {
                return i;
            }
        }
        
        return -1;
    }

    void Update()
    {
        timeTrack = TrackController.Instance.TimeTrack;

        if (HorizontalChange.Count > 0)
        {
            while (HorizontalChange.Count > 0 && timeTrack >= HorizontalChange[0].TimePoint)
            {
                ChangeHorizontal(HorizontalChange[0],VTrackPosIndex);
                HorizontalChange.RemoveAt(0);
            }

        }
 
        for (int i = 0; i < VTrackPosIndex.Length; i++)
        {
            float targetPos = VTrackFixedPos[VTrackPosIndex[i]];

            if (targetPos - VTrackPos[i] is > -0.01f and < 0.01f)
            {
                VTrackPos[i] = targetPos;
            }
            else
            {
                VTrackPos[i] += (targetPos-VTrackPos[i])*Time.deltaTime*Speed;
            }
        }
    }
}
