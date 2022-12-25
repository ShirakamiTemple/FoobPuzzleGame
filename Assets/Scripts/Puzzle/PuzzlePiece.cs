//***
// Author: Nate
// Description: PuzzlePiece is the main logic for handling a piece on a grid and storing its data
//***

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
    public bool IsValidated { get; set; }

    public bool IsTouchingGrid { get; private set; }

    private void Awake()
    {
        StartPosition = transform.position;
        StartRotation = transform.rotation;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag($"PuzzleGrid")) return;
        
        IsTouchingGrid = true;
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag($"PuzzleGrid")) return;

        IsTouchingGrid = false;
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
