//***
// Author: Nate
// Description: LevelData is just a scriptable object of all a levels data.
//***

using System.Collections.Generic;
using FoxHerding.Attributes;
using FoxHerding.Puzzle.Pieces;
using UnityEngine;

namespace FoxHerding.Levels
{
    [CreateAssetMenu(fileName = "New Level", menuName = "Levels/Level Data")]
    public class LevelData : ScriptableObject
    {
        // The properties in these classes aren't setter getters with backing fields due to needing a custom editor
        // to draw the properties.
        [SerializeField] 
        public GameObject LevelGrid;
        [SerializeField] 
        public List<PieceSpawnData> AvailablePieces;
        [SerializeField] 
        public bool IntroduceNewPiece;
        [SerializeField] 
        public PieceUIData PieceToIntroduce;

        [System.Serializable]
        public class PieceSpawnData
        {
            [SerializeField]
            public PuzzlePiece.PieceType CurrentPieceType;
            [SerializeField] 
            public Vector2 SpawnPosition;
            [SerializeField, FactorOf90] 
            public float SpawnRotation;
        }

        [System.Serializable]
        public class PieceUIData
        {
            [SerializeField]
            public PuzzlePiece.PieceType PieceType;
            [SerializeField]
            public string PieceDisplayName;
            [SerializeField, TextArea]
            public string PieceDescription;
            [SerializeField] 
            public GameObject TileHolderPrefab;
        }
    }
}