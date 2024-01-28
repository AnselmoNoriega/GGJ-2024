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

    [Header("Cursor Frustrated")]
    [SerializeField] private Texture2D _cursorFrustrated;
    [SerializeField] private Vector2 _cursorFrustratedHotSpot;

    private bool isPipesTurning = false;
    private bool gamePaused = false;

    void Start()
    {
        Cursor.SetCursor(_cursorNormal, Vector2.zero, CursorMode.Auto);
    }

    public void OnCursorInteractableEnter()
    {
        if (gamePaused == true || isPipesTurning == false)
        {
            Cursor.SetCursor(_cursorInteractable, _cursorInteractableHotSpot, CursorMode.Auto);
        }
    }
    
    public void OnCursorValveEnter()
    {
        if (gamePaused == true || isPipesTurning == false)
        {
            Cursor.SetCursor(_cursorValve, _cursorValveHotSpot, CursorMode.Auto);
        }
    }

    public void CursorFrustratedWhilePipesTurning()
    {
        Cursor.SetCursor(_cursorFrustrated, _cursorFrustratedHotSpot, CursorMode.Auto);
    }

    public void ReturnCursorToNormal()
    {
        if (gamePaused == true || isPipesTurning == false)
        {
            Cursor.SetCursor(_cursorNormal, _cursorNormalHotSpot, CursorMode.Auto);
        }
    }

    public void SetPipesTurningToTrue()
    {
        isPipesTurning = true;
    }

    public void SetPipesTurningToFalse()
    {
        isPipesTurning = false;
    }

    public void SetGamePaused()
    {
        if (gamePaused == false)
        {
            gamePaused = true;
        }
        if (gamePaused == true)
        {
            gamePaused = false;
            if (isPipesTurning == true)
            {
                CursorFrustratedWhilePipesTurning();
            }
            else
            {
                ReturnCursorToNormal();
            }
        }
    }
}
