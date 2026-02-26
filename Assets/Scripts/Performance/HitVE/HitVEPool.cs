using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitVEPool : ObjectPoolTemplate
{
    #region Mono饿汉单例代码

    private static HitVEPool _instance;
    public static HitVEPool Instance => _instance;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void InitializeRuntime()
    {
        GameObject obj = new GameObject("HitVEPool");
        _instance = obj.AddComponent<HitVEPool>();
        DontDestroyOnLoad(obj);
    }

    #endregion
    

    
    
    public new void Awake()
    {
        warmCount = 10;
        poolSize = 30;
        objectPrefab = Resources.Load<GameObject>("Prefabs/HitVE");
        
        base.Awake();
    }


    /// <summary>
    /// 播放特效
    /// </summary>
    /// <param name="position"></param>
    /// <param name="result"></param>
    /// <param name="scale"></param>
    public void PlayHitVE(Vector3 position,JudgmentResult result,float scale = 1)
    {
        HitVE hit = GetObject().GetComponent<HitVE>();
        if (hit)
        {
            hit.PlayEffect(position,result,scale);
        }
    }
}
