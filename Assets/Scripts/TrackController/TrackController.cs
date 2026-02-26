using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class TrackController : MonoBehaviour
{
    #region Mono饿汉单例代码

    private static TrackController _instance;
    public static TrackController Instance => _instance;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeRuntime()
    {
        GameObject obj = new GameObject("TrackController");
        _instance = obj.AddComponent<TrackController>();
        DontDestroyOnLoad(obj);
    }
    
    #endregion


    public bool IsTrackEnd
    {
        get => IsStarted && TimeTrack + Datas.Delay + 0.5f > _audioSource.clip.length;
        private set => throw new NotImplementedException();
    }
    
    
    
    public float TimeTrack { get; private set; }

    private NotesTrack notesTrack = new NotesTrack();
    
    private int notesNumber;
    
    private List<NoteInfo> notesInfos =  new List<NoteInfo>();

    private int PlacingNoteIndex = 0;
    
    private int PlaySENoteIndex = 0;

    private AudioSource _audioSource;
    
    public float hitDelay;
    
    private AudioSource AudioSource
    {
        get
        {
            if (!_audioSource)
            {
                {
                    _audioSource =  gameObject.AddComponent<AudioSource>();
                    _audioSource.playOnAwake = false;
                    _audioSource.volume = 0.4f;
                    return _audioSource;
                }
            }
            else
            {
                return _audioSource;
            }
        }
        set => _audioSource = value;
    }
    
    public bool isPausing;
    
    public bool IsStarted = false;
    
    public bool IsAutoPlay = false;
    
    /// <summary>
    /// 放置一个Note的方法
    /// </summary>
    /// <param name="noteInfo"></param>
    private void placeNote(NoteInfo noteInfo)
    {
        Note n = new Note(noteInfo);
        
        if (notesTrack != null)
        {
            switch (n.NoteType)
            {
                case NoteType.Tap: case NoteType.Up: case NoteType.Down: case NoteType.Left: case NoteType.Right:
                case NoteType.Hold:
                    notesTrack.EnterTapList(n);
                    break;
                case NoteType.Dash:
                    notesTrack.EnterDashList(n);
                    break;
            }
        }
        else
        {
            Debug.LogError("尝试将Note加入不存在的轨道！");
        }
    }

    /// <summary>
    /// 每帧调用尝试放置Note的方法
    /// </summary>
    /// <returns></returns>
    private bool TryPlaceNote()
    {
        if (PlacingNoteIndex < notesInfos.Count)
        {
            while (PlacingNoteIndex < notesInfos.Count && TimeTrack > notesInfos[PlacingNoteIndex].TimePoint)
            {
                PlacingNoteIndex++;
            }
            while (PlacingNoteIndex < notesInfos.Count && TimeTrack > notesInfos[PlacingNoteIndex].TimePoint - Datas.EarlyPlaceNoteTime)
            {
                placeNote(notesInfos[PlacingNoteIndex]);
                PlacingNoteIndex++;
            }
            return true;
        }
        else
        {
            return false;
        }
    }

   
    
    /// <summary>
    /// 初始化谱面
    /// </summary>
    /// <param name="trackInfo">乐曲信息</param>
    /// <param name="chartInfo">谱面信息</param>
    /// <param name="trackTime">起始时间（默认为零）</param>
    /// <returns>是否初始化成功</returns>
    private IEnumerator Initialize(TrackInfo trackInfo , ChartInfo chartInfo , float trackTime = 0)
    {
        TimeTrack = trackTime;
        
                
        if (notesTrack != null)
            notesTrack.Clear();
        
        //新建Note轨道
        notesTrack = new NotesTrack();
        
        
        
       
        
        //录入谱面（安全）
        notesInfos = ChartTransform.ToNoteInfoList(chartInfo.ChartContent);
        notesNumber = notesInfos.Count;

        //重置下标
        PlacingNoteIndex = 0;
        PlaySENoteIndex = 0;
        
        //初始化计分系统
        ScoringSystem.Instance.Initialize(notesNumber);
        
        AudioSource.clip = trackInfo.MusicClip;
        
        AudioSource.Play();

        PauseTrack();
        
        
        TrackVE.Instance.Initialize(trackInfo.BPM);
        
        yield return new WaitForSeconds(1f);
        
        ResumeTrack();
        Debug.Log("Start!");
        IsStarted = true;
    }

    /// <summary>
    /// 开始乐曲
    /// </summary>
    /// <param name="trackInfo">乐曲信息</param>
    /// <param name="chartInfo">谱面信息</param>
    /// <param name="trackTime">起始时间（默认为零）</param>
    public bool TrackStart(TrackInfo trackInfo, ChartInfo chartInfo , float trackTime = 0)
    {
        //防止传空引用
        if (trackInfo == null || chartInfo == null)
        {
            return false;
        }
        
        StopAllCoroutines();
        
        StartCoroutine(Initialize(trackInfo, chartInfo,trackTime));
        
        return true;
    }

    public void TrackStop()
    {
        StopAllCoroutines();
        IsStarted = false;
        AudioSource.Stop();
        
        if (notesTrack != null)
            notesTrack.Clear();
    }
    
    

    /// <summary>
    /// 暂停乐曲
    /// </summary>
    private void PauseTrack()
    {
        AudioSource.Pause();
        isPausing = true;
        AudioSource.time = TimeTrack + Datas.Delay;
    }

    /// <summary>
    /// 继续乐曲
    /// </summary>
    private void ResumeTrack()
    {
        AudioSource.UnPause();
        isPausing = false; 
        AudioSource.time = TimeTrack + Datas.Delay;
    }
    
    public void Update()
    {
        //TryPlayHitSE();
        
        if (IsStarted == true)
        {
            TryPlaceNote();
            
            if (isPausing == false)
            {
                TimeTrack += Time.deltaTime;
            }

            // if (Input.GetKeyDown(KeyCode.Space))
            // {
            //     if (isPausing == true)
            //     {
            //         ResumeTrack();
            //     }
            //     else if (isPausing == false)
            //     {
            //         PauseTrack();
            //     }
            // }

            if (isPausing == false && IsStarted == true)
            {
                List<Hit> hits = new List<Hit>();
                
                for (int i = 0; i < Datas.NoteTrackCount; i++)
                {
                    if (InputSystem.HitDown(i))
                        hits.Add(new Hit(i,HitType.Down));
                    if (InputSystem.Hit(i))
                        hits.Add(new Hit(i,HitType.Hold));
                }


                if (IsAutoPlay == false)
                {
                    notesTrack.TrackUpdate(hits.ToArray() , TimeTrack);
                }
                else
                {
                    notesTrack.AutoPlayUpdate(TimeTrack);
                }
            }
        }
    }

    
    /// <summary>
    /// 播放音效
    /// </summary>
    private void TryPlayHitSE()
    {
        if (IsStarted == true && isPausing == false)
        {
            if (PlaySENoteIndex < notesInfos.Count)
            {
                while (PlaySENoteIndex < notesInfos.Count && TimeTrack > notesInfos[PlaySENoteIndex].TimePoint + Datas.BadOffset)
                {
                    PlaySENoteIndex++;
                }
                while (PlaySENoteIndex < notesInfos.Count && TimeTrack + Datas.Delay + hitDelay > notesInfos[PlaySENoteIndex].TimePoint)
                {
                    HitSEPool.Instance.PlayHitSE();
                    PlaySENoteIndex++;
                }
            }
        }
    }
}

