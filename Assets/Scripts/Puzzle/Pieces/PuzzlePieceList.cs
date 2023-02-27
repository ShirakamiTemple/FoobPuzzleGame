using System.Collections.Generic;
using FoxHerding.Handlers;
using UnityEngine;

namespace FoxHerding.Puzzle.Pieces
{
    [CreateAssetMenu(fileName = "Piece List", menuName = "Pieces/Piece Data")]
    public class PuzzlePieceList : ScriptableObject
    {
        [field: SerializeField]
        public List<PuzzlePieceData> AllPossiblePacks { get; private set; }
    
        [System.Serializable]
        public class PuzzlePieceData
        {
            [field: SerializeField]
            public GameHandler.PuzzlePack CurrentPack { get; private set; }
            [field: SerializeField]
            public List<PuzzlePiece> PackPieces { get; private set; }
        }
    }
}
