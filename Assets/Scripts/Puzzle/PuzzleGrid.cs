//***
// Author: Nate
// Description: PuzzleGrid is the main logic for handling the entire grid and its data
//***

using System.Collections.Generic;
using System.Linq;
using FoxHerding.Handlers;
using FoxHerding.Puzzle.Pieces;
using UnityEngine;

namespace FoxHerding.Puzzle
{
    public class PuzzleGrid : MonoBehaviour
    {
        public List<PuzzlePiece> Pieces  { get; set; }
        public List<PuzzleTile> Tiles { get; private set; } = new();
        private bool _allPiecesValidated;

        private void Awake()
        {
            Tiles = GetComponentsInChildren<PuzzleTile>()
                .Where(tile => tile.CompareTag("PuzzleTile") || 
                               tile.CompareTag("Obstacle") && 
                               tile.transform.parent.CompareTag("Obstacle"))
                .ToList();
        }

        public void AttemptToProceedToNextStage()
        {
            if (!CheckAllValidation()) return;
            
            GameHandler.Instance.CurrentLevel += 1;
            SceneHandler.Instance.ReloadCurrentScene();
        }

        //checks if all current pieces have been validated
        private bool CheckAllValidation()
        {
            return Pieces.All(piece => piece.IsValidated);
        }
    }
}