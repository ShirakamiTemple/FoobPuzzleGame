//***
// Author: Nate
// Description: PuzzlePiece is the main logic for handling a piece on a grid
//***

using System;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    [field: SerializeField]
    public List<Transform> SnapPoints { get; private set; }
    [field: SerializeField]
    public List<SnapPointData> PointData { get; private set; }
    [field: SerializeField]
    public DragObject Draggable { get; private set; }
    public Vector3 StartPosition { get; private set; }
    public Quaternion StartRotation { get; private set; }
    [field: SerializeField]
    public bool IsValidated { get; set; }

    private readonly GUIStyle _style = new();

    private string _styleString;

    private void Awake()
    {
        StartPosition = transform.position;
        StartRotation = transform.rotation;

        _style.fontSize = 30;
        print("Startup");
    }

    public bool CheckValidation()
    {
        bool temp = false;
        foreach (SnapPointData data in PointData)
        {
            temp = data.IsOccupying;
            if (!temp)
            {
                break;
            }
        }
        IsValidated = temp;
        return IsValidated;
    }
}
