using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveSettings
{
    private static readonly string persistentPath = Application.persistentDataPath;
    private static readonly string settingsPath = Path.Combine(persistentPath, "Settings");
    private static readonly string musicDelayPath = Path.Combine(settingsPath, "MusicDelay.txt");
    private static readonly string SEDelayPath = Path.Combine(settingsPath, "SEDelay.txt");
    
    public static void SaveMusicDelay(int delay)
    {
        CheckPath();
        File.WriteAllText(musicDelayPath, delay.ToString());
    }
    
    public static void SaveSEDelay(int delay)
    {
        CheckPath();
        File.WriteAllText(SEDelayPath, delay.ToString());
    }
    
    public static int LoadMusicDelay()
    {
        CheckPath();
        if (!File.Exists(musicDelayPath))
        {
            return GameController.Delay;
        }

        if (int.TryParse(File.ReadAllText(musicDelayPath), out int delay))
        {
            return delay;
        }
        return GameController.Delay;
    }
    
    public static int LoadSEDelay()
    {
        CheckPath();
        if (!File.Exists(SEDelayPath))
        {
            return CMAudioController.intSEDelay;
        }

        if (int.TryParse(File.ReadAllText(SEDelayPath), out int delay))
        {
            return delay;
        }
        return CMAudioController.intSEDelay;
    }

    private static void CheckPath()
    {
        if (!Directory.Exists(settingsPath))
        {
            Directory.CreateDirectory(settingsPath);
        }
    }
}
