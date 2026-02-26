using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMVisualNote : MonoBehaviour
{
    private NoteInfo noteInfo;
    private Transform timePoint;
    private Transform duration;
    private Transform outLine;
    private SpriteRenderer timePointSR;
    private SpriteRenderer durationSR;
    private SpriteRenderer outLineSR;

    public void UpdatePosition()
    {
        float x = CMConstDatas.FirstTrackX + CMConstDatas.TrackXDistance * noteInfo.TrackIndex;
        float y = CMConstDatas.DistancePerSecond * noteInfo.TimePoint;
        float length = CMConstDatas.DistancePerSecond*noteInfo.HoldingTime;
        transform.position = new Vector3(x,y,0f);
        duration.localScale = new Vector3(duration.localScale.x, length , duration.localScale.z);
        duration.localPosition = new Vector3(duration.localPosition.x, length/2f,
            duration.localPosition.z);
    }
    
    /// <summary>
    /// 更新显示状态
    /// </summary>
    public void UpdateVisualState()
    {
        //更新位置和长度
        UpdatePosition();
        
        //更新外观
        switch (noteInfo.Type)
        {
            case NoteType.Tap:
                timePointSR.color = new Color(0f, 1f, 1f);
                transform.position += new Vector3(0f, 0f, -1f);
                break; 
            case NoteType.Hold:
                timePointSR.color = new Color(0.899371f, 0.4779676f, 0.899371f);
                durationSR.color = new Color(0.899371f, 0.4779676f, 0.899371f);
                transform.position += new Vector3(0f, 0f, -0f);
                break; 
            case NoteType.Dash:
                timePoint.localScale = new Vector3(0.15f, timePoint.localScale.y, timePoint.localScale.z);
                outLine.localScale = new Vector3(0.21f, outLine.localScale.y, outLine.localScale.z);
                timePointSR.color = new Color(0.9056604f, 0.9056604f, 0.4072623f);
                durationSR.color = new Color(0.9056604f, 0.9056604f, 0.4072623f);
                transform.position += new Vector3(0f, 0f, -2f);
                break; 
            default:
                break;    
        }
    }

    /// <summary>
    /// 获取noteInfo
    /// </summary>
    /// <param name="info"></param>
    public void SetInfo(NoteInfo info)
    {
        this.noteInfo = info;
    }

    void Awake()
    {
        timePoint = transform.GetChild(0);
        duration = transform.GetChild(1);
        outLine = transform.GetChild(2);
        timePointSR = timePoint.GetComponent<SpriteRenderer>();
        durationSR = duration.GetComponent<SpriteRenderer>();
        outLineSR = outLine.GetComponent<SpriteRenderer>();
        CMControlSystem.Instance.UpdateDps += UpdatePosition;
    }

    void OnDestroy()
    {
        CMControlSystem.Instance.UpdateDps -= UpdatePosition;
    }
    //打击特效播放
    private bool isPlayed;

    private bool isSEPlayed;

   
    
    void Update()
    {
        if (CMAudioController.Instance.TimeTrack > noteInfo.TimePoint)
        {
            if (!isPlayed)
            {
                if (CMAudioController.Instance.isPausing == false)
                   HitVEPool.Instance.PlayHitVE(transform.position,JudgmentResult.Perfect);
                isPlayed = true;
            }
        }
        else
        {
            isPlayed = false;
        }

        if (CMAudioController.Instance.TimeTrack > noteInfo.TimePoint)
        {
            isSEPlayed = true;
        }
        
        
        if (CMAudioController.Instance.TimeTrack + CMConstDatas.AudioDelay*CMAudioController.Instance.audioPitch + CMAudioController.SEDelayEX > noteInfo.TimePoint)
        {
            if (!isSEPlayed)
            {
                if (CMAudioController.Instance.isPausing == false)
                  HitSEPool.Instance.PlayHitSE();
                isSEPlayed = true;
            }
        }
        else
        {
            isSEPlayed = false;
        }
        
        if (CMAudioController.Instance.isPausing == false)
        {
            
        }
        
    }


}
