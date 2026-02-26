using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField]private MusicButton[] musicButtons;

    private static int mActiveIndex;
    
    [SerializeField]private ChartButton[] chartButtons;
    
    private static int cActiveIndex;

    [SerializeField]private Image musicCover;
    
    
    [SerializeField]private TMP_Text titleText;
    [SerializeField]private TMP_Text InfoText;
    
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject startCanvas;

    [SerializeField] private AudioSource loopMusicAudio;
    
    [SerializeField] private Image AutoPlayImage;
    
    private static bool isInMenu;
    
    void Start()
    {
        UpdateMB();

        if (isInMenu == false)
        {
            SwitchToStart();
        }
        else
        {
            SwitchToMenu();
        }
  
        SwitchAutoPlay(false);
    }

    
    private void SwitchToMenu()
    {
        isInMenu = true;
        menuCanvas.SetActive(true);
        startCanvas.SetActive(false);
        loopMusicAudio.enabled = true;
        SelectMusicButton(mActiveIndex);
    }

    public void SwitchToStartWithBlack()
    {
        BlackScreenTransition.Instance.Transition(0.5f,SwitchToStart);
    }
    
    public void SwitchToStart()
    {
        isInMenu = false;
        startCanvas.SetActive(true);
        menuCanvas.SetActive(false);
        loopMusicAudio.enabled = false;
    }

    private bool anyKeyPressing;
    
    private void Update()
    {
        if (isInMenu)
        {
            if (InputSystem.EscDown)
            {
                SwitchToStartWithBlack();
            }
        }
        else
        {
            if (Input.anyKeyDown )
            {
                anyKeyPressing = true;
            }
            else
            {
                if (anyKeyPressing && !Input.anyKey)
                {
                    BlackScreenTransition.Instance.Transition(0.5f,SwitchToMenu);
                    anyKeyPressing = false;
                }
            }
        }
    }


    public void BlackTransitionStartTrack()
    {
        BlackScreenTransition.Instance.Transition(0.5f,StartTrack);
    }
    

    /// <summary>
    /// 开始乐曲
    /// </summary>
    public void StartTrack()
    {
        TrackInfo trackInfo = TrackLoader.Instance.Tracks[mActiveIndex];
        ChartInfo chartInfo = TrackLoader.Instance.Tracks[mActiveIndex].ChartInfos[cActiveIndex];
        GameController.Instance.StartTrack(trackInfo, chartInfo);
    }
    
    /// <summary>
    /// 加载乐曲按钮
    /// </summary>
    private void UpdateMB()
    {
        for (int i = 0; i < musicButtons.Length; i++)
        {
            musicButtons[i].UpdateContent(TrackLoader.Instance.Tracks[i]);
        }
    }

    /// <summary>
    /// 加载谱面按钮
    /// </summary>
    /// <param name="trackInfo"></param>
    private void UpdateCB(TrackInfo trackInfo)
    {
        int i = 0;
        for (; i < trackInfo.ChartInfos.Length; i++)
        {
            chartButtons[i].gameObject.SetActive(true);
            chartButtons[i].UpdateContent(trackInfo.ChartInfos[i]);

            if (i > 0 && trackInfo.ChartInfos[i - 1].ChartDifficulty == ChartDif.HD &&
                trackInfo.ChartInfos[i - 1].ScoreSave.Score < 900000 && trackInfo.ChartInfos[i].ChartDifficulty == ChartDif.EX)
            {
                chartButtons[i].Lock(trackInfo.ChartInfos[i]);
                if (cActiveIndex == i)
                {
                    cActiveIndex = i - 1;
                }
            }
        }

        for (; i < chartButtons.Length; i++)
        {
            chartButtons[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 更新Title信息
    /// </summary>
    private void UpdateTitleAndInfo()
    {
        titleText.text = TrackLoader.Instance.Tracks[mActiveIndex].MusicTitle;
        titleText.text += "-";
        titleText.text += TrackLoader.Instance.Tracks[mActiveIndex].ChartInfos[cActiveIndex].ChartDifficulty.ToString();
        titleText.text += ":";
        titleText.text += TrackLoader.Instance.Tracks[mActiveIndex].ChartInfos[cActiveIndex].ChartDifficultyNumber
            .ToString();

        InfoText.text = "曲师：";
        InfoText.text += TrackLoader.Instance.Tracks[mActiveIndex].MusicArtist;
        InfoText.text += "\nbpm：";
        InfoText.text += TrackLoader.Instance.Tracks[mActiveIndex].BPM;
        InfoText.text += "\n谱师：";
        InfoText.text += TrackLoader.Instance.Tracks[mActiveIndex].ChartInfos[cActiveIndex].ChartArtist;
    }

    /// <summary>
    /// 选择乐曲
    /// </summary>
    /// <param name="index"></param>
    public void SelectMusicButton(int index)
    {
        foreach (var musicButton in musicButtons)
        {
            musicButton.OnUnselect();
        }
        
        mActiveIndex = index;
        musicButtons[mActiveIndex].OnSelect();

        UpdateCB(TrackLoader.Instance.Tracks[mActiveIndex]);

        if (cActiveIndex > TrackLoader.Instance.Tracks[mActiveIndex].ChartInfos.Length - 1)
        {
            cActiveIndex = TrackLoader.Instance.Tracks[mActiveIndex].ChartInfos.Length - 1;
        }
        SelectChartButton(cActiveIndex);
        
        musicCover.sprite = TrackLoader.Instance.Tracks[mActiveIndex].MusicCover;
        
        UpdateTitleAndInfo();
        
        BluredBGController.Instance.ChangeBackground(mActiveIndex);

        LoopMusicPlayer.Instance.PlayLoop(TrackLoader.Instance.Tracks[mActiveIndex].MusicClip,
            TrackLoader.Instance.Tracks[mActiveIndex].MusicExcerpt.x,
            TrackLoader.Instance.Tracks[mActiveIndex].MusicExcerpt.y);
    }
    
    /// <summary>
    /// 选择谱面
    /// </summary>
    /// <param name="index"></param>
    public void SelectChartButton(int index)
    {
        foreach (var chartButton in chartButtons)
        {
            chartButton.OnUnselect();
        }
        
        cActiveIndex = index;
        chartButtons[cActiveIndex].OnSelect();

        UpdateTitleAndInfo();
    }

    public void SwitchAutoPlay()
    {
        if (TrackController.Instance.IsAutoPlay == false)
        {
            TrackController.Instance.IsAutoPlay = true;
            AutoPlayImage.enabled = true;
        }
        else
        {
            TrackController.Instance.IsAutoPlay = false;
            AutoPlayImage.enabled = false;
        }
    }
    
    public void SwitchAutoPlay(bool value)
    {
        TrackController.Instance.IsAutoPlay = value;
        AutoPlayImage.enabled = value;
    }

    public void GameExit()
    {
        AutoSaveSO.Instance.SaveAllAutoSaveSOs();
        
#if UNITY_EDITOR
        // 如果在Unity编辑器中，停止播放模式
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 如果是打包后的游戏，退出应用程序
        Application.Quit();
#endif
    }

}
