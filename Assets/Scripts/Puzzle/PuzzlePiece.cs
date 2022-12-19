//***
// Author: Nate
// Description: PuzzlePiece is the main logic for handling a piece on a grid
//***

using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    [field: SerializeField]
    public List<Transform> SnapPoints { get; private set; }
    [field: SerializeField]
    public DragObject Draggable { get; private set; }
    public Vector3 StartPosition { get; private set; }
    public Quaternion StartRotation { get; private set; }
    public bool IsValidated { get; set; }

    private void Awake()
    {
        StartPosition = transform.position;
        StartRotation = transform.rotation;
    }
}
