using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMControlSystem : MonoBehaviour
{

    private static CMControlSystem _instance;
    public static CMControlSystem Instance => _instance;
    
    public int Bpm { get; private set; }
    public int BeatCount { get; private set; }
    public int CurrentDevidingCount { get; private set; }
    public int TestBpm = 170;

    public int PreTrack = 0;
    public float PreTimePoint = 0f;
    private float Timer;
    
    private CMDividingLineSwitcher dividingline;

    public float SensitivityTime = 1f;

    private GameObject beatLineObj;

    private float dpsInverseFactor = 0.125f;
    
    public Action UpdateDps;

    public enum PlacingState
    {
        Empty,
        Tap,
        Hold,
        Dash,
        HoldIsPlacing,
        DashIsPlacing,
    }
    
    public PlacingState CurrentState = PlacingState.Empty;

    private bool RecordPosition()
    {
        PreTrack = CMCalculater.Instance.CalculateTrack();
        if (PreTrack == -1)
        {
            return false;
        }
        PreTimePoint = CMCalculater.Instance.CalculateTime(Bpm, CurrentDevidingCount);
        return true;
    }
    
    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        Bpm = CMTrackLoader.LoadBpm();
        if (Bpm == 0)
        {
            Bpm = TestBpm;
        }
        BeatCount = Mathf.CeilToInt(CMAudioController.Instance.Audio.clip.length * Bpm / 60f);
        CurrentDevidingCount = CMConstDatas.DividingCounts[2];

        CMConstDatas.DistancePerSecond = CMConstDatas.BasicDps;
        
        //创建节拍线
        beatLineObj = CMLineGenerater.Instance.CreateBeatLine(Bpm, BeatCount);
        //创建分割线
        dividingline = CMLineGenerater.Instance.CreateDividingLine(Bpm, BeatCount);
        //创建节拍号
        CMLineGenerater.Instance.CreateNumber(Bpm, BeatCount);
        //加载Note
        CMNoteSystem.Instance.LoadNotes();
        // UpdateDps?.Invoke();
    }

    public float dps;
    void Update()
    {

        dps = CMConstDatas.DistancePerSecond;
        //Control+滚轮调节DPS
        if (Input.GetKey(KeyCode.LeftControl))
        {
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            if (scrollInput != 0)
            {
                dpsInverseFactor -= scrollInput * 0.1f;
                dpsInverseFactor = Mathf.Clamp(dpsInverseFactor, 1/15f, 1/3f);
                
                CMConstDatas.DistancePerSecond = 1/dpsInverseFactor;
                
                
                beatLineObj.transform.localScale = new Vector3(beatLineObj.transform.localScale.x,
                    1 * CMConstDatas.DistancePerSecond / CMConstDatas.BasicDps,
                    beatLineObj.transform.localScale.z);

                UpdateDps?.Invoke();
            }
        }

        //Shift加速滚轮滚动速度
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SensitivityTime = 3f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            SensitivityTime = 1f;
        }
        
        //切换分割线
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CurrentDevidingCount = 1;
            dividingline.CloseLine();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CurrentDevidingCount = CMConstDatas.DividingCounts[0];
            dividingline.SwitchLine(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CurrentDevidingCount = CMConstDatas.DividingCounts[1];
            dividingline.SwitchLine(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            CurrentDevidingCount = CMConstDatas.DividingCounts[2];
            dividingline.SwitchLine(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            CurrentDevidingCount = CMConstDatas.DividingCounts[3];
            dividingline.SwitchLine(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            CurrentDevidingCount = CMConstDatas.DividingCounts[4];
            dividingline.SwitchLine(4);
        }

        //状态机：
        //空状态与其他3预放置状态随意切换
        if (CurrentState != PlacingState.HoldIsPlacing && CurrentState != PlacingState.DashIsPlacing)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                CurrentState = PlacingState.Tap;
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                CurrentState = PlacingState.Hold;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                CurrentState = PlacingState.Dash;
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                CurrentState = PlacingState.Empty;
            }
        }
        //空状态下移除note
        if (CurrentState == PlacingState.Empty)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                CMNoteSystem.Instance.RemoveNote();
            }
            if (Input.GetKey(KeyCode.Backspace))
            {
                CMNoteSystem.Instance.RemoveNote();
            }
        }
        //Tap放置行为
        if (CurrentState == PlacingState.Tap)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                CMNoteSystem.Instance.AddTap();
            }
        }
        //Hold预放置行为
        if (CurrentState == PlacingState.Hold)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (RecordPosition())
                {
                    Timer = 0f;
                    CurrentState = PlacingState.HoldIsPlacing;
                }
            }
        }
        //Dash预放置行为
        if (CurrentState == PlacingState.Dash)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (RecordPosition())
                {
                    Timer = 0f;
                    CurrentState = PlacingState.DashIsPlacing;
                }
            }
        }
        //Hold放置中行为
        if (CurrentState == PlacingState.HoldIsPlacing)
        {
            if (Timer <= 0.1f)
            {
                Timer += Time.deltaTime;
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    CMNoteSystem.Instance.AddHoldOrDash(PreTrack, PreTimePoint, NoteType.Hold);
                    CurrentState = PlacingState.Hold;
                }
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    CurrentState = PlacingState.Hold;
                }
            }
        }
        //Dash放置中行为
        if (CurrentState == PlacingState.DashIsPlacing)
        {
            if (Timer <= 0.1f)
            {
                Timer += Time.deltaTime;
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    CMNoteSystem.Instance.AddHoldOrDash(PreTrack, PreTimePoint, NoteType.Dash);
                    CurrentState = PlacingState.Dash;
                }
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    CurrentState = PlacingState.Dash;
                }
            }
        }
    }
    
}
