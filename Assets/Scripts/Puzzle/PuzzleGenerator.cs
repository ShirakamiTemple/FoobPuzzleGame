//***
// Author: Nate
// Description: PuzzleGenerator.cs generates the grid and pieces required for the current level
//***

using System;
using System.Collections.Generic;
using FoxHerding.Handlers;
using FoxHerding.Levels;
using FoxHerding.Puzzle.Pieces;
using UnityEngine;

namespace FoxHerding.Puzzle
{
    public class PuzzleGenerator : MonoBehaviour
    {
        [SerializeField, Tooltip("Reference to the parent of where you want the pieces to be instantiated")]
        private Transform pieceParentTransform;
        [SerializeField, Tooltip("Reference to the parent of where you want the grid to be instantiated")] 
        private Transform gridParentTransform;
        private PuzzlePieceList.PuzzlePieceData _currentPieceData;
        private LevelData _levelData;
        private GameHandler _gameHandler;
        private readonly List<PuzzlePiece> _createdPieces = new();
        private GameObject _newGrid;

        private void Awake()
        {
            _gameHandler = GameHandler.Instance;
            PlaceGrid();
            CenterActiveTiles();
            GeneratePieces();
            SceneHandler.Instance.SceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneHandler.Instance.SceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(string sceneName)
        {
            UIHandler.Instance.GameUI.UpdateNewPieceData(_currentPieceData, _levelData);
        }

        private void GeneratePieces()
        {
            _createdPieces.Clear();
            _levelData = _gameHandler.AvailableLevels[_gameHandler.CurrentLevel];
            _currentPieceData = _gameHandler.AvailablePieces.AllPossiblePacks[(int)_gameHandler.CurrentPack];
            foreach (LevelData.PieceSpawnData levelPiece in _levelData.AvailablePieces)
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
            _newGrid.name = "Level " + _gameHandler.CurrentLevel + 1 + " grid";
        }

        private void CenterActiveTiles()
        {
            if (_newGrid == null) return;
            
            int activeTileCount = 0;
            Vector3 totalPosition = Vector3.zero;
            foreach (Transform child in _newGrid.transform)
            {
                if (!child.gameObject.activeSelf) continue;
                
                activeTileCount++;
                totalPosition += child.position;
            }
            if (activeTileCount <= 0) return;
            
            // center the grid based on active tiles
            Vector3 centerPosition = totalPosition / activeTileCount;
            _newGrid.transform.position = centerPosition;
            RectTransform gridRect = _newGrid.GetComponent<RectTransform>();
            Vector2 anchoredPosition = gridRect.anchoredPosition;
            anchoredPosition = new Vector2(-anchoredPosition.x, -anchoredPosition.y);
            gridRect.anchoredPosition = anchoredPosition;
        }
    }
}