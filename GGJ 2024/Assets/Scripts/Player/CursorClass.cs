using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    [Header("Target Canvas")]    
    [SerializeField] private GraphicRaycaster graphicRaycaster;
    private EventSystem eventSystem;

    private CursorState cursorState = CursorState.Normal;
    private bool arePipesTurning = false;
    private bool gamePaused = false;

    void Start()
    {
        ReturnCursorToNormal();
        eventSystem = GetComponent<EventSystem>();
    }

    void Update()
    {
        if (eventSystem.IsPointerOverGameObject())
        {
            if (cursorState == CursorState.Frustrated)
                return;

            UpdateCursor();
        }
        else if (cursorState != CursorState.Normal)
        {
            ReturnCursorToNormal();
        }
    }

    private void UpdateCursor()
    {
        PointerEventData eventData = new PointerEventData(eventSystem);
        eventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        graphicRaycaster.Raycast(eventData, results);
        Debug.Log(cursorState);
        if (results.Count > 0)
        {
            UpdateHoverCursor(results[0].gameObject.tag);
        }
    }

    private void UpdateHoverCursor(string currentObject)
    {
        switch (currentObject)
        {
            case "Interactable":
                if (cursorState != CursorState.Interactable) { SetCursorInteractable(); }
                break;
            case "Handwheel":
                if (cursorState != CursorState.Handwheel && !gamePaused) { SetCursorHandwheel(); }
                break;
            default:
                if (cursorState != CursorState.Normal) { ReturnCursorToNormal(); }
                break;

        }


        if (cursorState != CursorState.Interactable && currentObject == "Interactable")
        {
            SetCursorInteractable();
        }
    }


    private void SetCursorInteractable()
    {
        Cursor.SetCursor(_cursorInteractable, _cursorInteractableHotSpot, CursorMode.Auto);
        cursorState = CursorState.Interactable;
    }
    
    public void SetCursorHandwheel()
    {
        Cursor.SetCursor(_cursorValve, _cursorValveHotSpot, CursorMode.Auto);
        cursorState = CursorState.Handwheel;
    }

    public void ReturnCursorToNormal()
    {
        Cursor.SetCursor(_cursorNormal, _cursorNormalHotSpot, CursorMode.Auto);
        cursorState = CursorState.Normal;
    }

    public void SetPipesTurning(bool turning)
    {
        arePipesTurning = turning;
        if (turning)
        {
            Cursor.SetCursor(_cursorFrustrated, _cursorFrustratedHotSpot, CursorMode.Auto);
            cursorState = CursorState.Frustrated;
        }
        else
        {
            ReturnCursorToNormal();
        }
    }

    public void TogglePauseMenu()
    {
        gamePaused = !gamePaused;
        if (gamePaused)
        {
            ReturnCursorToNormal();
        }
        else if (arePipesTurning)
        {
            Cursor.SetCursor(_cursorFrustrated, _cursorFrustratedHotSpot, CursorMode.Auto);
        }
    }

    public enum CursorState
    {
        Normal,
        Interactable,
        Frustrated,
        Handwheel
    }
}
