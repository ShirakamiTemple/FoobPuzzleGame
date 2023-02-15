//***
// Author: Nate
// Description: PuzzleGenerator.cs generates the grid and pieces required for the current level
//***

using System.Collections.Generic;
using UnityEngine;

public class PuzzleGenerator : MonoBehaviour
{
    [SerializeField, Tooltip("Reference to the parent of where you want the pieces to be instantiated")]
    private Transform pieceParentTransform;
    [SerializeField, Tooltip("Reference to the parent of where you want the grid to be instantiated")] 
    private Transform gridParentTransform;
    private PuzzlePieceList.PuzzlePieceData _currentPieceData;
    private GameHandler _gameHandler;
    private readonly List<PuzzlePiece> _createdPieces = new();
    private GameObject _newGrid;

    private void Awake()
    {
        _gameHandler = GameHandler.Instance;
        PlaceGrid();
        GeneratorPieces();
    }

    private void GeneratorPieces()
    {
        _createdPieces.Clear();
        LevelData levelData = _gameHandler.AvailableLevels[_gameHandler.CurrentLevel];
        _currentPieceData = _gameHandler.AvailablePieces.AllPossiblePacks[(int)_gameHandler.CurrentPack];
        foreach (LevelData.PieceSpawnData levelPiece in levelData.AvailablePieces)
        {
            foreach (PuzzlePiece packPiece in _currentPieceData.PackPieces)
            {
                if (packPiece.CurrentPieceType != levelPiece.CurrentPieceType) continue;

                print("spawn: " + packPiece.gameObject.name + " at position: " + levelPiece.SpawnPosition);
                GameObject newPiece = Instantiate(packPiece.gameObject, pieceParentTransform);
                PuzzlePiece puzzlePiece = newPiece.GetComponent<PuzzlePiece>();
                RectTransform pieceTransform = newPiece.GetComponent<RectTransform>();
                newPiece.name = packPiece.gameObject.name;
                pieceTransform.anchoredPosition = levelPiece.SpawnPosition;
                puzzlePiece.SetStartOrientation();
                _createdPieces.Add(puzzlePiece);
            }
        }
        _newGrid.GetComponent<PuzzleGrid>().Pieces = _createdPieces;
    }

    private void PlaceGrid()
    {
        _newGrid = Instantiate(_gameHandler.AvailableLevels[_gameHandler.CurrentLevel].LevelGrid, gridParentTransform);
    }
}