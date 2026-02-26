using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CMAudioController : MonoBehaviour
{
    
    private static CMAudioController _instance; 
    public static CMAudioController Instance => _instance;
    
    [SerializeField] private float BasicScrollSensitivity = 3f;  // 基础滚动灵敏度
    public float minTime;           // 最小值
    public float maxTime;           // 最大值

    public float sliderLength = 9.3f;
    public float sliderXposition = 5.77f;
    public float sliderWidth = 1f;
    public bool isSliding = false;
    
    public float audioPitch = 1f; //播放倍速
    
    
    public AudioSource Audio;

    public bool isPausing;

    public float TimeTrack;//{ get; private set; }

    private float replayTime;

    public static float SEDelayEX
    {
        get => intSEDelay / 1000f;
        private set => throw new System.NotImplementedException();
    }

    public static int intSEDelay = -30;
    
    public TMP_InputField SEDelayInput;

    public void ChangeSEDelayEX(string str)
    {
        int.TryParse(str, out int  intDelay);
        
        intSEDelay = intDelay;
        SaveSettings.SaveSEDelay(intSEDelay);
    }
    
    void Awake()
    {
        _instance = this;
        
        Audio = GetComponent<AudioSource>();
        AudioClip audioClip = CMTrackLoader.LoadAudio();
        
       
        
        if (audioClip != null)
        {
            Audio.clip = audioClip;
        }
        
        //测试语句
        Resources.Load<TrackInfo>("Tracks/9-Test/TrackInfo").MusicClip = Audio.clip;

        intSEDelay = SaveSettings.LoadSEDelay();
    }

    //暂停
    private void Pause()
    {
        Audio.Pause();
        isPausing = true;
    }

    //播放
    private void UnPause()
    {
        Audio.time = TimeTrack + CMConstDatas.AudioDelay*audioPitch;
        isPausing = false;
        Audio.UnPause();
    }
    
    void Start()
    {
        minTime = 0f;
        maxTime = Audio.clip.length - CMConstDatas.AudioDelay;

        Audio.Play();
        Pause();

        SEDelayInput.text = intSEDelay.ToString();
    }
    
    void Update()
    {
        //拖进度条
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (CMCalculater.Instance.MousePosition.x - CMCameraMove.Instance.transform.position.x - sliderXposition >=
                sliderWidth / -2f &&
                CMCalculater.Instance.MousePosition.x - CMCameraMove.Instance.transform.position.x - sliderXposition <=
                sliderWidth / 2f && CMCalculater.Instance.MousePosition.y -
                CMCameraMove.Instance.transform.position.y >=
                sliderLength / -2f && CMCalculater.Instance.MousePosition.y -
                CMCameraMove.Instance.transform.position.y <=
                sliderLength / 2f)
            {
                Pause();
                isSliding = true;
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            isSliding = false;
        }
        if (isSliding)
        {
            TimeTrack = maxTime *
                Mathf.Clamp(
                    CMCalculater.Instance.MousePosition.y - CMCameraMove.Instance.transform.position.y +
                    sliderLength / 2f, 0, sliderLength-0.1f) / sliderLength;
        }
        
        //调倍速
        if (audioPitch > 0.9f)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                audioPitch = 0.5f;
                Audio.pitch = audioPitch;
                Audio.time = TimeTrack + CMConstDatas.AudioDelay*audioPitch;
            }
        }
        else if (audioPitch is <= 0.9f and > 0.4f)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                audioPitch = 0.25f;
                Audio.pitch = audioPitch;
                Audio.time = TimeTrack + CMConstDatas.AudioDelay*audioPitch;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                audioPitch = 1f;
                Audio.pitch = audioPitch;
                Audio.time = TimeTrack + CMConstDatas.AudioDelay*audioPitch;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                audioPitch = 0.5f;
                Audio.pitch = audioPitch;
                Audio.time = TimeTrack + CMConstDatas.AudioDelay*audioPitch;
            }
        }
        
        //结尾暂停
        if (Audio.isPlaying && TimeTrack >= maxTime) // 接近结束时
        {
            Pause();
        }
        
        //时间轴播放
        if (isPausing == false)
        {
            TimeTrack += Time.deltaTime * audioPitch;
        }
        
        //暂停功能
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isPausing == true && TimeTrack < maxTime)
            {
                UnPause(); 
            }
            else if (isPausing == false)
            {
                Pause();
            }
        }

        //replay功能
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (isPausing == true && TimeTrack < maxTime)
            {
                UnPause(); 
                replayTime = TimeTrack;
            }
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            Pause();
            TimeTrack = replayTime;
            
        }

            // 读取鼠标滚轮输入
        if (!Input.GetKey(KeyCode.LeftControl))
        {
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            if (scrollInput != 0)
            {
                if (isPausing == false)
                {
                    Pause();
                }
                // 根据滚动方向调整值
                TimeTrack += scrollInput * BasicScrollSensitivity * CMControlSystem.Instance.SensitivityTime;
                // 限制范围
                TimeTrack = Mathf.Clamp(TimeTrack, minTime, maxTime);
            }
        }
        
        
        
        
    }
    
    
}
