//***
// Author: Nate
// Description: LevelData is just a scriptable object of all a levels data
//***

using System.Collections.Generic;
using FoxHerding.Puzzle.Pieces;
using UnityEngine;

namespace FoxHerding.Levels
{
    [CreateAssetMenu(fileName = "New Level", menuName = "Levels/Level Data")]
    public class LevelData : ScriptableObject
    {
        [field: SerializeField]
        public GameObject LevelGrid { get; private set; }
        [field: SerializeField]
        public List<PieceSpawnData> AvailablePieces { get; private set; }

        [System.Serializable]
        public class PieceSpawnData
        {
            [field: SerializeField]
            public PuzzlePiece.PieceType CurrentPieceType { get; private set; }
            [field: SerializeField]
            public Vector2 SpawnPosition { get; private set; }
        }
    
    }
}
