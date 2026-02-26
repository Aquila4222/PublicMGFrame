using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputSystem
{
    public static KeyCode EscapeKey = KeyCode.Escape;

    public static bool  Esc => Input.GetKey(EscapeKey);
    public static bool EscDown => Input.GetKeyDown(EscapeKey);
    public static bool EscUp => Input.GetKeyUp(EscapeKey);

    public static KeyCode[] HitKey = new[] { KeyCode.D, KeyCode.F, KeyCode.J, KeyCode.K };

    public static bool Hit(int index)
    {
        return index is >= 0 and < Datas.NoteTrackCount && Input.GetKey(HitKey[index]);
    }
    public static bool HitDown(int index)
    {
        return index is >= 0 and < Datas.NoteTrackCount && Input.GetKeyDown(HitKey[index]);
    }
    public static bool HitUp(int index)
    {
        return index is >= 0 and < Datas.NoteTrackCount && Input.GetKeyUp(HitKey[index]);
    }
}
