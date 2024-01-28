using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorClass : MonoBehaviour
{
    [Header("Cursor Normal")]
    [SerializeField] private Texture2D _cursorNormal;
    [SerializeField] private Vector2 _cursorNormalHotSpot;

    [Header("Cursor Interactable")]
    [SerializeField] private Texture2D _cursorInteractable;
    [SerializeField] private Vector2 _cursorInteractableHotSpot;

    [Header("Cursor Valve")]
    [SerializeField] private Texture2D _cursorValve;
    [SerializeField] private Vector2 _cursorValveHotSpot;

    void Start()
    {
        Cursor.SetCursor(_cursorNormal, Vector2.zero, CursorMode.Auto);
    }

    public void OnCursorInteractableEnter()
    {
        Cursor.SetCursor(_cursorInteractable, _cursorInteractableHotSpot, CursorMode.Auto);
    }
    
    public void OnCursorValveEnter()
    {
        Cursor.SetCursor(_cursorValve, _cursorValveHotSpot, CursorMode.Auto);
    }

    public void ReturnCursorToNormal()
    {
        Cursor.SetCursor(_cursorNormal, _cursorNormalHotSpot, CursorMode.Auto);
    }
}
