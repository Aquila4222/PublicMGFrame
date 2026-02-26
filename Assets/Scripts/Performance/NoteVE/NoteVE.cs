using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteVE : MonoBehaviour
{
    public static float Speed = 12;

    public static float TrackSpacing = 1.6f;
    
    public static float YOffset = -3;
    
    public const float LineWidth = 0.05f;
    
    public const float minDashLength = 0.35f;

    public GameObject Dot;
    public GameObject DotOutline;
    public GameObject Dash;
    public GameObject DashOutline;
    public GameObject Hold;
    public GameObject Arrow;
    
    public SpriteRenderer DotSprite;
    public SpriteRenderer DashOutlineSprite;
    public SpriteRenderer HoldSprite;
    
    
    private float TimePoint;
    private int TrackIndex;
    private NoteType NoteType;
    private float HoldingTime;

    private int VTrackIndex;
    
    private float xPosition;
    
    private bool isHitted = false;
    public bool isHoldMissed = false;
    private JudgmentResult judgmentResult;
    private float timer;
    
    public void Initialize(NoteInfo noteInfo)
    {
        TimePoint = noteInfo.TimePoint;
        TrackIndex = noteInfo.TrackIndex;
        HoldingTime = noteInfo.HoldingTime;
        NoteType = noteInfo.Type;

        VTrackIndex = TrackVE.Instance.CalculateVTrackIndex(TrackIndex);
        if (VTrackIndex == -1 || VTrackIndex > 3)
        {
            Debug.Log($"视觉轨道绑定错误:{VTrackIndex}");
        }

        if (NoteType is NoteType.Right or NoteType.Left)
        {
            TrackVE.Instance.AddArrowNote(new ArrowNoteInfo(NoteType,TimePoint,VTrackIndex));
        }

        
        xPosition = (TrackIndex-1.5f) * TrackSpacing;
        
        DotSprite.color = Color.white;
        DashOutlineSprite.color = Color.white;
        HoldSprite.color = Color.white;
        isHitted = false;
        isHoldMissed = false;
        
        transform.eulerAngles = new Vector3(0, 0, 0);
        
        switch (NoteType)
        {
            case NoteType.Tap:
                Dot.SetActive(true);
                DotOutline.SetActive(true);
                Dash.SetActive(false);
                DashOutline.SetActive(false);
                Hold.SetActive(false);
                Arrow.SetActive(false);
                break;
            case NoteType.Hold:
                Dot.SetActive(true);
                DotOutline.SetActive(true);
                Dash.SetActive(false);
                DashOutline.SetActive(false);
                Hold.SetActive(true);
                Arrow.SetActive(false);
                break;
            case NoteType.Dash:
                Dot.SetActive(false);
                DotOutline.SetActive(false);
                Dash.SetActive(true);
                DashOutline.SetActive(true);
                Hold.SetActive(false);
                Arrow.SetActive(false);
                break;
            case NoteType.Up: case NoteType.Down: case NoteType.Left : case NoteType.Right:
                Dot.SetActive(false);
                DotOutline.SetActive(false);
                Dash.SetActive(false);
                DashOutline.SetActive(false);
                Hold.SetActive(false);
                Arrow.SetActive(true);
                switch (NoteType)
                {
                    case NoteType.Up:
                        transform.eulerAngles = new Vector3(0, 0, 0);
                        break;
                    case NoteType.Down:
                        transform.eulerAngles = new Vector3(0, 0, 180);
                        break;
                    case NoteType.Left:
                        transform.eulerAngles = new Vector3(0, 0, 90);
                        break;
                    case NoteType.Right:
                        transform.eulerAngles = new Vector3(0, 0, -90);
                        break;
                }
                break;
                
        }
        

        
        
    }

    public void OnHit(JudgmentResult result)
    {
        if (result == JudgmentResult.Bad)
        {
            HitVEPool.Instance.PlayHitVE(new Vector3(transform.position.x, transform.position.y,-1), result);
        }
        else
        {
            HitVEPool.Instance.PlayHitVE(new Vector3(xPosition, YOffset,-1), result);
        }
        if (NoteType is NoteType.Tap or NoteType.Up or NoteType.Down or NoteType.Left or NoteType.Right)
        {
            NoteVEPool.Instance.ReturnObject(gameObject);
        }

        isHitted = true;

        judgmentResult = result;
    }

    public void HoldMiss()
    {
        DotSprite.color = new Color(0.3f,0.3f,0.3f,1);
        DashOutlineSprite.color = new Color(0.3f,0.3f,0.3f,1);
        HoldSprite.color = new Color(0.3f,0.3f,0.3f,1);
        isHoldMissed = true;
    }
    
    
    
    void Awake()
    {
        DotSprite = Dot.GetComponent<SpriteRenderer>();
        DashOutlineSprite = DashOutline.GetComponent<SpriteRenderer>();
        HoldSprite =  Hold.GetComponent<SpriteRenderer>();
        
        //DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        
    }


    void Update()
    {
        xPosition = TrackVE.Instance.VTrackPos[VTrackIndex];
        
        
        Speed = Datas.NoteSpeed;

        float timeTrack = TrackController.Instance.TimeTrack;
        
        //更新Note形状与位置
        switch (NoteType)
        {
            case NoteType.Tap: case NoteType.Up: case NoteType.Down: case NoteType.Left: case NoteType.Right: 
                transform.position = new Vector2(xPosition, YOffset+(TimePoint - timeTrack)*Speed);
                break;
            case NoteType.Hold:
                if (timeTrack < TimePoint)
                {
                    transform.position = new Vector2(xPosition, YOffset+(TimePoint - timeTrack)*Speed);
                    Hold.transform.localScale = new Vector3(Hold.transform.localScale.x, HoldingTime*Speed, Hold.transform.localScale.z);
                    Hold.transform.localPosition = new Vector3(Hold.transform.localPosition.x, HoldingTime*Speed/2, Hold.transform.localPosition.z);
                }
                else
                {
                    transform.position = new Vector2(xPosition, YOffset);
                    Hold.transform.localScale = new Vector3(Hold.transform.localScale.x,
                        (TimePoint + HoldingTime - timeTrack) * Speed,
                        Hold.transform.localScale.z);
                    Hold.transform.localPosition = new Vector3(Hold.transform.localPosition.x,
                        (TimePoint + HoldingTime - timeTrack) * Speed / 2,
                        Hold.transform.localPosition.z);
                }
                break;
            case NoteType.Dash:
                if (timeTrack < TimePoint)
                {
                    transform.position = new Vector2(xPosition, YOffset+(TimePoint - timeTrack)*Speed);
                    Dash.transform.localScale = new Vector3(Dash.transform.localScale.x, HoldingTime*Speed-LineWidth, Dash.transform.localScale.z);
                    Dash.transform.localPosition = new Vector3(Dash.transform.localPosition.x, HoldingTime*Speed/2, Dash.transform.localPosition.z);
                    DashOutline.transform.localScale = new Vector3(DashOutline.transform.localScale.x, HoldingTime*Speed+LineWidth, DashOutline.transform.localScale.z);
                    DashOutline.transform.localPosition = new Vector3(DashOutline.transform.localPosition.x, HoldingTime*Speed/2, DashOutline.transform.localPosition.z);
                    
                    if (HoldingTime*Speed < minDashLength)
                    {
                        Dash.transform.localScale = new Vector3(Dash.transform.localScale.x, minDashLength-LineWidth, Dash.transform.localScale.z);
                        Dash.transform.localPosition = new Vector3(Dash.transform.localPosition.x, minDashLength/2, Dash.transform.localPosition.z);
                        DashOutline.transform.localScale = new Vector3(DashOutline.transform.localScale.x, minDashLength+LineWidth, DashOutline.transform.localScale.z);
                        DashOutline.transform.localPosition = new Vector3(DashOutline.transform.localPosition.x, minDashLength/2, DashOutline.transform.localPosition.z);
                    }
                }
                else
                {
                    transform.position = new Vector2(xPosition, YOffset);
                    Dash.transform.localScale = new Vector3(Dash.transform.localScale.x,
                        (TimePoint + HoldingTime - timeTrack) * Speed - LineWidth,
                        Dash.transform.localScale.z);
                    Dash.transform.localPosition = new Vector3(Dash.transform.localPosition.x,
                        (TimePoint + HoldingTime - timeTrack) * Speed / 2,
                        Dash.transform.localPosition.z);
                    DashOutline.transform.localScale = new Vector3(DashOutline.transform.localScale.x,
                        (TimePoint + HoldingTime - timeTrack) * Speed + LineWidth,
                        DashOutline.transform.localScale.z);
                    DashOutline.transform.localPosition = new Vector3(DashOutline.transform.localPosition.x,
                        (TimePoint + HoldingTime - timeTrack) * Speed / 2,
                        DashOutline.transform.localPosition.z);
                }
                break;
        }
        
        //摧毁过时的长条
        if (timeTrack > TimePoint + HoldingTime && (NoteType == NoteType.Hold || NoteType == NoteType.Dash))
        {
            NoteVEPool.Instance.ReturnObject(gameObject);
        }
        
        //持续播放长条特效
        if ((NoteType == NoteType.Hold || NoteType == NoteType.Dash) && isHoldMissed == false && isHitted == true && timeTrack < TimePoint + HoldingTime)
        {
            if (timer < 0.15f)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0f;
                // if (NoteType == NoteType.Dash)
                // {
                //     if (TrackController.Instance.TimeTrack >= TimePoint)
                //     {
                //         HitVEPool.Instance.PlayHitVE(new Vector3(xPosition, YOffset,-1), judgmentResult);
                //     }
                // }
                // else
                // {
                //     HitVEPool.Instance.PlayHitVE(new Vector3(xPosition, YOffset,-1), judgmentResult);
                // }
                
                HitVEPool.Instance.PlayHitVE(new Vector3(xPosition, YOffset,-1), judgmentResult,0.6f);
            }
        }
    }

    public void ReturnNote()
    {
        NoteVEPool.Instance.ReturnObject(gameObject);
    }


    void OnDisable()
    {
        transform.position = new Vector3(-1000, 0,transform.position.z);
    }
}
