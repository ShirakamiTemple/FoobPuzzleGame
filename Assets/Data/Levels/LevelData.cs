//***
// Author: Nate
// Description: LevelData is just a scriptable object of all a levels data.
//***

using System.Collections.Generic;
using FoxHerding.Puzzle.Pieces;
using UnityEngine;

namespace FoxHerding.Data.Levels
{
    [CreateAssetMenu(fileName = "New Level", menuName = "Levels/Level Data")]
    public class LevelData : ScriptableObject
    {
        // The properties in these classes aren't setter getters with backing fields due to needing a custom editor
        // to draw the properties.
        public GameObject LevelGrid;
        public List<PieceSpawnData> AvailablePieces;
        public bool IntroduceNewPiece;
        public PieceUIData PieceToIntroduce;

        [System.Serializable]
        public class PieceSpawnData
        {
            public PuzzlePiece.PieceType CurrentPieceType;
            public bool FlipPiece;
        }

        [System.Serializable]
        public class PieceUIData
        {
            public PuzzlePiece.PieceType PieceType;
            public string PieceDisplayName;
            [TextArea]
            public string PieceDescription;
            public GameObject TileHolderPrefab;
        }
    }
}