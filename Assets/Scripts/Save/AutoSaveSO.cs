using System;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// 标记这个类的所有实例都会自动保存
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class AutoSaveSOAttribute : Attribute
{
}

public class AutoSaveSO : MonoBehaviour
{
    #region 单例

    private static AutoSaveSO _instance;
    public static AutoSaveSO Instance => _instance;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeRuntime()
    {
        GameObject obj = new GameObject("AutoSave");
        _instance = obj.AddComponent<AutoSaveSO>();
        DontDestroyOnLoad(obj);
    }

    #endregion

    private string saveFolderPath;
    
    // 缓存：哪些类型需要自动保存
    private HashSet<System.Type> autoSaveTypes = new HashSet<System.Type>();

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        saveFolderPath = Path.Combine(Application.persistentDataPath, "AutoSave");
        
        if (!Directory.Exists(saveFolderPath))
        {
            Directory.CreateDirectory(saveFolderPath);
        }
        
        Debug.Log($"📁 存档路径: {saveFolderPath}");
        
        // 扫描所有程序集，找到带[AutoSaveSO]标签的类
        ScanForAutoSaveTypes();
    }
    
    void Start()
    {
        // 启动时自动加载所有符合条件的SO
        LoadAllAutoSaveSOs();
    }

    /// <summary>
    /// 扫描所有带[AutoSaveSO]标签的类
    /// </summary>
    private void ScanForAutoSaveTypes()
    {
        var assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
        
        foreach (var assembly in assemblies)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsSubclassOf(typeof(ScriptableObject)) && 
                    type.GetCustomAttributes(typeof(AutoSaveSOAttribute), true).Length > 0)
                {
                    autoSaveTypes.Add(type);
                    Debug.Log($"🔍 发现自动保存类型: {type.Name}");
                }
            }
        }
    }

    /// <summary>
    /// 加载所有带标签的SO实例
    /// </summary>
    public void LoadAllAutoSaveSOs()
    {
        var allSOs = Resources.FindObjectsOfTypeAll<ScriptableObject>();
        
        foreach (var so in allSOs)
        {
            if (autoSaveTypes.Contains(so.GetType()))
            {
                LoadSO(so);
            }
        }
    }

    /// <summary>
    /// 保存所有带标签的SO实例
    /// </summary>
    public void SaveAllAutoSaveSOs()
    {
        var allSOs = Resources.FindObjectsOfTypeAll<ScriptableObject>();
        
        foreach (var so in allSOs)
        {
            if (autoSaveTypes.Contains(so.GetType()))
            {
                SaveSO(so);
            }
        }
    }

    /// <summary>
    /// 保存单个ScriptableObject
    /// </summary>
    public void SaveSO(ScriptableObject so)
    {
        if (so == null) return;

        string filePath = GetFilePath(so);
        string json = JsonUtility.ToJson(so, true);
        File.WriteAllText(filePath, json);
        
        Debug.Log($"💾 保存成功: {so.name} ({so.GetType().Name})");
    }

    /// <summary>
    /// 加载单个ScriptableObject
    /// </summary>
    public void LoadSO(ScriptableObject so)
    {
        if (so == null) return;

        string filePath = GetFilePath(so);
        
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            if (json == string.Empty) return;
            JsonUtility.FromJsonOverwrite(json, so);
            Debug.Log($"📂 加载成功: {so.name}");
        }
    }

    /// <summary>
    /// 获取文件路径
    /// </summary>
    private string GetFilePath(ScriptableObject so)
    {
        string safeName = string.Join("_", so.name.Split(Path.GetInvalidFileNameChars()));
        string fileName = $"{so.GetType().Name}_{safeName}.json";
        return Path.Combine(saveFolderPath, fileName);
    }

    /// <summary>
    /// 手动触发保存
    /// </summary>
    public void SaveNow()
    {
        SaveAllAutoSaveSOs();
    }

    void OnApplicationQuit()
    {
        SaveAllAutoSaveSOs();
    }

    void OnDestroy()
    {
        SaveAllAutoSaveSOs();
    }
}