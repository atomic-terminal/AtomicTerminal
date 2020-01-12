using UnityEngine;
using System.Collections;

public class CursorManager : MonoBehaviour {

    public static CursorManager _instance;

    public Texture2D cursor_normal;
    public Texture2D cursor_attack;
    public Texture2D cursor_lockTarget;
    public Texture2D cursor_pick;

    private Vector2 hotspot = Vector2.zero;
    private CursorMode mode = CursorMode.Auto;

    void Start() {
        _instance = this;
        SetNormal();
    }

    public void SetNormal() {
        Cursor.SetCursor(cursor_normal, hotspot, mode);
    }
    public void SetAttack() {
        Cursor.SetCursor(cursor_attack, hotspot, mode);
    }
    public void SetLockTarget() {
        Cursor.SetCursor(cursor_lockTarget, hotspot, mode);
    }
}
