using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class CMTrackLoader
{
    private static readonly string gameFolder = Directory.GetParent(Application.dataPath).FullName;
    private static readonly string savePath = Path.Combine(gameFolder, "TrackInfo");
    private static readonly string chartPath = Path.Combine(savePath, "chart.txt");
    private static readonly string bpmPath = Path.Combine(savePath, "bpm.txt");
    private static readonly string audioPath = Path.Combine(savePath, "audio.mp3");

    public static void SaveChart(string chart)
    {
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        
        File.WriteAllText(chartPath, chart);
        Debug.Log($"已保存到游戏文件夹: {chartPath}");
    }

    public static string LoadChart()
    {
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        if (!File.Exists(chartPath))
        {
            Debug.Log("谱面文件不存在！");
            return "";
        }
        return File.ReadAllText(chartPath);
    }
    
    public static int LoadBpm()
    {
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        if (!File.Exists(bpmPath))
        {
            Debug.Log("Bpm文件不存在！");
            return 0;
        }
        string bpmString = File.ReadAllText(bpmPath);
        int bpm;
        if (int.TryParse(bpmString, out bpm))
        {
            if (bpm > 0)
            {
                return bpm;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return 0;
        }
    }
    
    public static AudioClip LoadAudio()
    {
        // 检查目录
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        
        // 检查文件
        if (!File.Exists(audioPath))
        {
            Debug.Log("音频文件不存在！");
            return null;
        }
        
        // 加载 AudioClip
        string url = "file://" + audioPath;
        using (WWW www = new WWW(url))
        {
            // 等待加载完成（会阻塞线程）
            while (!www.isDone) { }
            
            if (string.IsNullOrEmpty(www.error))
            {
                return www.GetAudioClip(false, false);
            }
            else
            {
                Debug.LogError("音频加载失败: " + www.error);
                return null;
            }
        }
    }
}
