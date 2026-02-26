using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMPreview : MonoBehaviour
{
    
    private NoteInfo noteInfo;
    private Transform timePoint;
    private Transform duration;
    private SpriteRenderer timePointSR;
    private SpriteRenderer durationSR;

    private float x;
    private float y;
    private float length;
    private float alpha = 0.5f;
    
    void Awake()
    {
        timePoint = transform.GetChild(0);
        duration = transform.GetChild(1);
        timePointSR = timePoint.GetComponent<SpriteRenderer>();
        durationSR = duration.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        switch (CMControlSystem.Instance.CurrentState)
        {
            case CMControlSystem.PlacingState.Tap:
            {
                x = CMConstDatas.FirstTrackX + CMConstDatas.TrackXDistance * CMCalculater.Instance.CalculateTrack();
                y = CMConstDatas.DistancePerSecond * CMCalculater.Instance.CalculateTime(CMControlSystem.Instance.Bpm,
                    CMControlSystem.Instance.CurrentDevidingCount);
                transform.position = new Vector3(x, y, -3f);
                timePointSR.enabled = true;
                durationSR.enabled = false;
                timePointSR.color = new Color(0f, 1f, 1f,alpha);
                timePoint.localScale = new Vector3(1.08f, timePoint.localScale.y, timePoint.localScale.z);
                break;
            }
            case CMControlSystem.PlacingState.Hold:
            {
                x = CMConstDatas.FirstTrackX + CMConstDatas.TrackXDistance * CMCalculater.Instance.CalculateTrack();
                y = CMConstDatas.DistancePerSecond * CMCalculater.Instance.CalculateTime(CMControlSystem.Instance.Bpm,
                    CMControlSystem.Instance.CurrentDevidingCount);
                transform.position = new Vector3(x, y, -3f);
                timePointSR.enabled = true;
                durationSR.enabled = false;
                timePointSR.color = new Color(0.899371f, 0.4779676f, 0.899371f,alpha);
                timePoint.localScale = new Vector3(1.08f, timePoint.localScale.y, timePoint.localScale.z);
                break;
            }
            case CMControlSystem.PlacingState.Dash:
            {
                x = CMConstDatas.FirstTrackX + CMConstDatas.TrackXDistance * CMCalculater.Instance.CalculateTrack();
                y = CMConstDatas.DistancePerSecond * CMCalculater.Instance.CalculateTime(CMControlSystem.Instance.Bpm,
                    CMControlSystem.Instance.CurrentDevidingCount);
                transform.position = new Vector3(x, y, -3f);
                timePointSR.enabled = true;
                durationSR.enabled = false;
                timePointSR.color = new Color(0.9056604f, 0.9056604f, 0.4072623f,alpha);
                timePoint.localScale = new Vector3(0.21f, timePoint.localScale.y, timePoint.localScale.z);
                break;
            }
            case CMControlSystem.PlacingState.HoldIsPlacing:
            {
                x = CMConstDatas.FirstTrackX + CMConstDatas.TrackXDistance * CMControlSystem.Instance.PreTrack;
                y = CMConstDatas.DistancePerSecond * CMControlSystem.Instance.PreTimePoint;
                transform.position = new Vector3(x, y, -3f);
                length = CMConstDatas.DistancePerSecond * CMCalculater.Instance.CalculateTime(
                    CMControlSystem.Instance.Bpm,
                    CMControlSystem.Instance.CurrentDevidingCount) - y;
                timePointSR.enabled = true;
                durationSR.enabled = true;
                duration.localScale = new Vector3(duration.localScale.x, length, duration.localScale.z);
                duration.localPosition = new Vector3(duration.localPosition.x, length/2, duration.localPosition.z);
                timePointSR.color = new Color(0.899371f, 0.4779676f, 0.899371f,alpha);
                durationSR.color = new Color(0.899371f, 0.4779676f, 0.899371f,alpha);
                timePoint.localScale = new Vector3(1.08f, timePoint.localScale.y, timePoint.localScale.z);
                break;
            }
            case CMControlSystem.PlacingState.DashIsPlacing:
            {
                x = CMConstDatas.FirstTrackX + CMConstDatas.TrackXDistance * CMControlSystem.Instance.PreTrack;
                y = CMConstDatas.DistancePerSecond * CMControlSystem.Instance.PreTimePoint;
                transform.position = new Vector3(x, y, -3f);
                length = CMConstDatas.DistancePerSecond * CMCalculater.Instance.CalculateTime(
                    CMControlSystem.Instance.Bpm,
                    CMControlSystem.Instance.CurrentDevidingCount) - y;
                timePointSR.enabled = true;
                durationSR.enabled = true;
                duration.localScale = new Vector3(duration.localScale.x, length, duration.localScale.z);
                duration.localPosition = new Vector3(duration.localPosition.x, length/2, duration.localPosition.z);
                timePoint.localScale = new Vector3(0.21f, timePoint.localScale.y, timePoint.localScale.z);
                timePointSR.color = new Color(0.9056604f, 0.9056604f, 0.4072623f,alpha);
                durationSR.color = new Color(0.9056604f, 0.9056604f, 0.4072623f,alpha);
                break;
            }
            case CMControlSystem.PlacingState.Empty:
            {
                timePointSR.enabled = false;
                durationSR.enabled = false;
                break;
            }
        }
    }
}
