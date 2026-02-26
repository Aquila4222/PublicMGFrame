using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class VisualScore : MonoBehaviour
{
    private static VisualScore _instance;
    public static VisualScore Instance => _instance;
    
    
    public int Score;

    public int Combo;
    
    public int MaxCombo;

    public int PerfectCount;

    public int EarlyCount;
    
    public int LateCount;

    public int BadCount;
    
    public int MissCount;

    public Text ScoreText;
    
    public Text ComboText;
    
    public Text ComboWord;

    public GameObject SettleCanvas;
    
    public TMP_Text SettleScoreText;
    public TMP_Text SettleJudgmentText;
    
    public TMP_Text SettleNewBest;

    public TMP_Text SettleEvaluate;
    
    public TMP_Text SettleTitle;
    public Image SettleCover;
    
    public TMP_Text SettleAutoPlayText;
    
    public TMP_Text GameMusicInfoText;
    
    public TMP_Text GameAutoPlayText;

    private float musicFullTime;
    
    public RectTransform processingBarRect;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        _instance = this;
        SettleCanvas.SetActive(false);
        
        GameMusicInfoText.text = GameController.Instance.localTrackInfo.MusicTitle;
        GameMusicInfoText.text += " : ";
        GameMusicInfoText.text += GameController.Instance.localChartInfo.ChartDifficulty;
        GameMusicInfoText.text += GameController.Instance.localChartInfo.ChartDifficultyNumber;
        
        musicFullTime = GameController.Instance.localTrackInfo.MusicClip.length;

        GameAutoPlayText.enabled = TrackController.Instance.IsAutoPlay;
    }

    public void ShowSettle()
    {
        SettleCanvas.SetActive(true);
        
        SettleScoreText.text = "Score : ";
        SettleScoreText.text += Score.ToString("D7");
        
        SettleJudgmentText.text = "perfect : ";
        SettleJudgmentText.text += PerfectCount.ToString();
        SettleJudgmentText.text += "\nearly : ";
        SettleJudgmentText.text += EarlyCount.ToString();
        SettleJudgmentText.text += "\nlate : ";
        SettleJudgmentText.text += LateCount.ToString();
        SettleJudgmentText.text += "\nbad : ";
        SettleJudgmentText.text += BadCount.ToString();
        SettleJudgmentText.text += "\nmiss : ";
        SettleJudgmentText.text += MissCount.ToString();
        SettleJudgmentText.text += "\n\nmaxCombo : ";
        SettleJudgmentText.text += MaxCombo.ToString();

        SettleTitle.text = GameMusicInfoText.text;
        SettleCover.sprite = GameController.Instance.localTrackInfo.MusicCover;
        

        SettleEvaluate.text = ScoringSystem.Instance.Evaluate;
        if (ScoringSystem.Instance.IsAllPerfect)
        {
            SettleEvaluate.color = Color.yellow;
        }
        else if (ScoringSystem.Instance.IsFullCombo)
        {
            SettleEvaluate.color = Color.cyan;
        }
        else
        {
            SettleEvaluate.color = Color.white;
        }

        if (TrackController.Instance.IsAutoPlay)
        {
            SettleAutoPlayText.enabled = true;
            SettleScoreText.enabled = false;
            SettleJudgmentText.enabled = false;
            SettleNewBest.enabled = false;
            SettleEvaluate.enabled = false;
        }
        else
        {
            SettleAutoPlayText.enabled = false;
            SettleScoreText.enabled = true;
            SettleJudgmentText.enabled = true;
            SettleNewBest.enabled = Score > GameController.Instance.localChartInfo.ScoreSave.Score;
            SettleEvaluate.enabled = true;
        }
    }

    public void BackToMenu()
    {
        GameController.Instance.BackToMenuWithBlack();
    }
    
    
    // Update is called once per frame
    void Update()
    {
        if (Combo >= 3)
        {
            ComboText.enabled = true;
            ComboWord.enabled = true;
            ComboText.text = Combo.ToString();
            
            if (ScoringSystem.Instance.IsAllPerfect)
            {
                ComboWord.text = "AllPerfect";
            }
            else if (ScoringSystem.Instance.IsFullCombo)
            {
                ComboWord.text = "FullCombo";
            }
            else
            {
                ComboWord.text = "Combo";
            }
            
        }
        else
        {
            ComboText.enabled = false;
            ComboWord.enabled = false;
        }
        
        ScoreText.text = Score.ToString("D7");


        processingBarRect.sizeDelta = new Vector2(5120 * (TrackController.Instance.TimeTrack / musicFullTime), 100);
        
        Score = ScoringSystem.Instance.Score;
        Combo = ScoringSystem.Instance.ComboCount;
        MaxCombo = ScoringSystem.Instance.MaxCombo;
        PerfectCount = ScoringSystem.Instance.GetJudgmentCount(JudgmentResult.Perfect);
        EarlyCount = ScoringSystem.Instance.GetJudgmentCount(JudgmentResult.Early);
        LateCount = ScoringSystem.Instance.GetJudgmentCount(JudgmentResult.Late);
        BadCount = ScoringSystem.Instance.GetJudgmentCount(JudgmentResult.Bad);
        MissCount = ScoringSystem.Instance.GetJudgmentCount(JudgmentResult.Miss);
    }
}
