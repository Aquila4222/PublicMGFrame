using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Menu,
    Playing,
    Score
}

public class GameController : MonoBehaviour
{
    #region Mono饿汉单例代码

    private static GameController _instance;
    public static GameController Instance => _instance;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void InitializeRuntime()
    {
        GameObject obj = new GameObject("GameController");
        _instance = obj.AddComponent<GameController>();
        DontDestroyOnLoad(obj);
    }

    #endregion

    private GameState gameState = GameState.Menu;
    
    private Scene currentScene;

    public TrackInfo localTrackInfo{get; private set;}
    public ChartInfo localChartInfo{get; private set;}

    public static int Delay { get; private set; }

    public float cmTime;

    private bool isFullScreen = true;
    
    void Awake()
    {
        //Debug.Log("[GameController] Awake");
        //AutoSaveSO.Instance.LoadAllAutoSaveSOs();
        
        
        Screen.SetResolution(2560, 1440, true);
        
        //获取当前场景信息
        // currentScene = SceneManager.GetActiveScene();
        // if (currentScene.name == "ChartMaker")
        // {
        //     Destroy(gameObject);
        //     Destroy(TrackController.Instance.gameObject);
        //     return;
        // }

        // Delay = 160;
        //
        // Delay = SaveSettings.LoadMusicDelay();
        
        // CMConstDatas.AudioDelay = Delay/1000f;
        // ConstDatas.Delay = Delay/1000f;
        
        //加载乐曲信息
        TrackLoader.Instance.LoadTracks();
        
        localTrackInfo = TrackLoader.Instance.Tracks[3];

        localChartInfo = TrackLoader.Instance.Tracks[3].ChartInfos[0];
    }
    
    
    
    void Start()
    {
        
    }

    IEnumerator CMRestoreTime()
    {
        yield return new WaitForSeconds(0.5f);
        if (CMAudioController.Instance)
          CMAudioController.Instance.TimeTrack = cmTime;
    }

    public void StartTrack(TrackInfo  trackInfo , ChartInfo chartInfo)
    {
        localTrackInfo = trackInfo;
        localChartInfo = chartInfo;
        SceneManager.LoadScene("Scenes/GameScene");
        TrackController.Instance.TrackStart(localTrackInfo,localChartInfo);
        gameState =  GameState.Playing;
        //AutoSaveSO.Instance.SaveAllAutoSaveSOs();
    }

    public void BackToMenuWithBlack()
    {
        BlackScreenTransition.Instance.Transition(0.5f,BackToMenu);
    }

    public void BackToMenu()
    {
        if (gameState == GameState.Score)
        {
            TrackController.Instance.TrackStop();

            if (TrackController.Instance.IsAutoPlay == false)
            {
                if (ScoringSystem.Instance.Score > localChartInfo.ScoreSave.Score)
                {
                    localChartInfo.ScoreSave.Score = ScoringSystem.Instance.Score;
                }
                if (ScoringSystem.Instance.IsFullCombo == true)
                {
                    localChartInfo.ScoreSave.IsFullCombo = true;
                }
            }
           
            
            SceneManager.LoadScene("Scenes/MenuScene");
        }
        else if (gameState == GameState.Playing)
        {
            TrackController.Instance.TrackStop();
            SceneManager.LoadScene("Scenes/MenuScene");
        }
        //AutoSaveSO.Instance.SaveAllAutoSaveSOs();
        
        gameState =  GameState.Menu;
    }
    
    void Update()
    {
        if (gameState == GameState.Playing)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        
        
        
        if (gameState == GameState.Playing)
        {
            if (TrackController.Instance.IsTrackEnd)
            {
                TrackController.Instance.TrackStop();
                BlackScreenTransition.Instance.Transition(0.5f,VisualScore.Instance.ShowSettle);
                gameState = GameState.Score;
            }

            if (InputSystem.EscDown)
            {
                BackToMenuWithBlack();
            }
        }
        
        
        
        
        // if (Input.GetKeyDown(KeyCode.Equals))
        // {
        //     TrackLoader.Instance.Tracks[3].ChartInfos[0].ChartContent =
        //         ChartTransform.ListToString(CMNoteSystem.Instance.noteInfos);
        //     cmTime = CMAudioController.Instance.TimeTrack;
        //     SceneManager.LoadScene("Scenes/GameScene");
        //     TrackController.Instance.TrackStart(localTrackInfo,localChartInfo,cmTime);
        //
        // }
        //
        // if (Input.GetKeyDown(KeyCode.Minus))
        // {
        //     TrackController.Instance.TrackStop();
        //     SceneManager.LoadScene("Scenes/Chartmaker");
        //     StartCoroutine(CMRestoreTime());
        // }
        
        if (Input.GetKeyDown(KeyCode.F12))
        {
            if (isFullScreen)
            {
                Screen.SetResolution(1280, 720, false);
                isFullScreen = false;
            }
            else
            {
        
                Screen.SetResolution(2560, 1440, true);
                isFullScreen = true;
                
            }
        }


        // if (Input.GetKeyDown(KeyCode.UpArrow))
        // {
        //     Delay += 10;
        //     CMConstDatas.AudioDelay = Delay/1000f;
        //     ConstDatas.Delay = Delay/1000f;
        // }
        //
        // if (Input.GetKeyDown(KeyCode.DownArrow))
        // {
        //     Delay -= 10;
        //     CMConstDatas.AudioDelay = Delay/1000f;
        //     ConstDatas.Delay = Delay/1000f;
        // }        
    }

    public void ChangeDelay(int delay)
    {
        Delay = delay;
        CMConstDatas.AudioDelay = Delay/1000f;
        Datas.Delay = Delay/1000f;
        SaveSettings.SaveMusicDelay(delay);
    }
}
