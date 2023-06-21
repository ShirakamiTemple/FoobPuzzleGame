//***
// Author: Nate
// Description: PuzzleGenerator.cs generates the grid and pieces required for the current level
//***

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
        [SerializeField]
        private List<RectTransform> spawnZones;

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

                    GameObject newPiece = Instantiate(packPiece.gameObject, pieceParentTransform);
                    PuzzlePiece puzzlePiece = newPiece.GetComponent<PuzzlePiece>();
                    RectTransform pieceTransform = newPiece.GetComponent<RectTransform>();
                    newPiece.name = packPiece.gameObject.name;
                    //pieceTransform.anchoredPosition = levelPiece.SpawnPosition;
                    PlaceObjectInRandomZone(pieceTransform);
                    puzzlePiece.SetStartOrientation();
                    _createdPieces.Add(puzzlePiece);
                }
            }
            _newGrid.GetComponent<PuzzleGrid>().Pieces = _createdPieces;
        }
        
        private void PlaceObjectInRandomZone(RectTransform objectToSpawn)
        {
            Rect objectRect = objectToSpawn.rect;
            // Get a random spawn zone
            int randomIndex = Random.Range(0, spawnZones.Count);
            RectTransform spawnZone = spawnZones[randomIndex];
            Rect spawnZoneRect = GetWorldRect(spawnZone);

            // Calculate the available space within the spawn zone
            float minX = spawnZoneRect.xMin + objectRect.width / 2;
            float maxX = spawnZoneRect.xMax - objectRect.width / 2;
            float minY = spawnZoneRect.yMin + objectRect.height / 2;
            float maxY = spawnZoneRect.yMax - objectRect.height / 2;

            // Clamp the random position within the available space
            float randomX = Mathf.Clamp(Random.Range(minX, maxX), minX, maxX);
            float randomY = Mathf.Clamp(Random.Range(minY, maxY), minY, maxY);
            Vector2 randomPosition = new Vector2(randomX, randomY);

            // Set the position of the object's RectTransform
            objectToSpawn.position = randomPosition;
        }

        private static Rect GetWorldRect(RectTransform rectTransform)
        {
            Vector2 rectTransformPosition = rectTransform.position;
            Vector2 rectTransformSize = rectTransform.rect.size;
            Rect rect = new Rect(rectTransformPosition.x - rectTransformSize.x / 2,
                rectTransformPosition.y - rectTransformSize.y / 2,
                rectTransformSize.x, rectTransformSize.y);
            return rect;
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