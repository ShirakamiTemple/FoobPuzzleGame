using System.Collections.Generic;
using UnityEngine;

public class PuzzleGrid : MonoBehaviour
{
    [field: SerializeField]
    public List<PuzzlePiece> Pieces  { get; private set; }

    public List<PuzzleTile> Tiles { get; private set; } = new();

    private bool _allPiecesValidated;

    private void Awake()
    {
        Tiles.Clear();

        foreach (Transform child in transform)
        {
            Tiles.Add(child.GetComponent<PuzzleTile>());
        }
        print("Startup");
    }

    private void NextStage()
    {
        if (_allPiecesValidated)
        {
            //update grid
        }
    }

    public void CheckAllValidation()
    {
        bool allValidated = true;
        foreach (PuzzlePiece piece in Pieces)
        {
            allValidated = piece.IsValidated;
            if (!allValidated) break;
        }
        _allPiecesValidated = allValidated;
    }
}
