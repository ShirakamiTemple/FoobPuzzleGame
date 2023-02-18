//***
// Author: Nate
// Description: PuzzleGrid is the main logic for handling the entire grid and its data
//***

using System.Collections.Generic;
using System.Linq;
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
                .Where(x => x.CompareTag("PuzzleTile") || 
                            x.CompareTag("Obstacle") && 
                            x.transform.parent.CompareTag("Obstacle"))
                .ToList();
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
            _allPiecesValidated = Pieces.All(x => x.IsValidated);
        }
    }
}