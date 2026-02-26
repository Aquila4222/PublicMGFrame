using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSEPool : ObjectPoolTemplate
{
    #region Mono饿汉单例代码

    private static HitSEPool _instance;
    public static HitSEPool Instance => _instance;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void InitializeRuntime()
    {
        GameObject obj = new GameObject("HitSEPool");
        _instance = obj.AddComponent<HitSEPool>();
        DontDestroyOnLoad(obj);
    }

    #endregion
    
    public new void Awake()
    {
        warmCount = 20;
        poolSize = 500;
        objectPrefab = Resources.Load<GameObject>("Prefabs/HitSE");
        
        base.Awake();
    }

    public float Volume;

    public void PlayHitSE()
    {
        HitSE hit = GetObject().GetComponent<HitSE>();
        if (hit)
        {
            hit.PlaySE(Volume);
        }
    }
    
}
