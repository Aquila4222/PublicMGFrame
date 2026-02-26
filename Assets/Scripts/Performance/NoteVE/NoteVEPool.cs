using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteVEPool : ObjectPoolTemplate
{
    #region Mono饿汉单例代码

    private static NoteVEPool _instance;
    public static NoteVEPool Instance => _instance;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void InitializeRuntime()
    {
        GameObject obj = new GameObject("NoteVEPool");
        _instance = obj.AddComponent<NoteVEPool>();
        DontDestroyOnLoad(obj);
    }

    #endregion

    public new void Awake()
    {
        warmCount = 100;
        poolSize = 500;
        objectPrefab = Resources.Load<GameObject>("Prefabs/NoteVE");
        
        base.Awake();
    }

    /// <summary>
    /// 生成视觉Note
    /// </summary>
    /// <param name="trackIndex"></param>
    /// <param name="noteType"></param>
    /// <param name="timePoint"></param>
    /// <param name="holdingTime"></param>
    /// <returns></returns>
    public NoteVE PlaceNoteVE(NoteInfo noteInfo)
    {
        NoteVE noteVE = GetObject().GetComponent<NoteVE>();
        noteVE.Initialize(noteInfo);
        return noteVE;
    }
    
}
