using System.Collections.Generic;
using UnityEngine;

public class PuzzleGrid : MonoBehaviour
{
    [field: SerializeField]
    public List<PuzzlePiece> Pieces  { get; private set; }
    [field: SerializeField]
    public List<PuzzleTile> Tiles { get; private set; }
}
