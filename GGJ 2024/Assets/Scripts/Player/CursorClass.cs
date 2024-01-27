using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorClass : MonoBehaviour
{
    [SerializeField] private Texture2D _cursor;
    void Start()
    {
        Cursor.SetCursor(_cursor, Vector2.zero, CursorMode.Auto);
    }
}
