using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMVisualNoteGenerator : Singleton<CMVisualNoteGenerator>
{
    private GameObject _notePrefab;

    private GameObject NotePrefab
    {
        get
        {
            if (_notePrefab == null)
                _notePrefab = Resources.Load<GameObject>("CMPrefabs/NotePrefab");
            return _notePrefab;
        }
        set
        {
            _notePrefab = value;
        }
    }
    
    /// <summary>
    /// 输入一个noteInfo，生成一个Note视觉效果的克隆体，并给与其noteInfo的索引
    /// </summary>
    /// <param name="noteInfo"></param>
    public GameObject GenerateNote(NoteInfo noteInfo)
    {
                GameObject noteObj = Object.Instantiate(NotePrefab);
                CMVisualNote noteScript = noteObj.GetComponent<CMVisualNote>();
                noteScript.SetInfo(noteInfo);
                noteScript.UpdateVisualState();
                return noteObj;
    }
}
