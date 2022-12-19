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
}
