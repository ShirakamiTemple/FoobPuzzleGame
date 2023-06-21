//***
// Author: Nate
// Description: PuzzlePiece is the main logic for handling a piece on a grid and storing its data
//***

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace FoxHerding.Puzzle.Pieces
{
    public class PuzzlePiece : MonoBehaviour
    {
        public enum PieceType { SINGLE, BIG_I, BIG_L, SQUARE }
        [field: SerializeField]
        public PieceType CurrentPieceType { get; private set; }
        [field: SerializeField]
        public List<Transform> SnapPoints { get; private set; }
        [field: SerializeField]
        public List<SnapPointData> PointData { get; private set; }
        public Vector3 StartPosition { get; private set; }
        public Quaternion StartRotation { get; private set; }
        [field: SerializeField]
        public bool IsValidated { get; set; }
        public bool IsTouchingGrid { get; private set; }
        private Transform _myTransform;
        [SerializeField]
        private Image pieceImage;

        private void Awake()
        {
            _myTransform = transform;
            pieceImage.alphaHitTestMinimumThreshold = 0.5f;
        }

        public void SetStartOrientation()
        {
            StartPosition = _myTransform.position;
            StartRotation = _myTransform.rotation;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.gameObject.CompareTag("PuzzleGrid")) return;

            IsTouchingGrid = true;
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            if (!col.gameObject.CompareTag("PuzzleGrid")) return;

            IsTouchingGrid = false;
        }

        public bool CheckValidation()
        {
            IsValidated = PointData.All(data => data.IsOccupying);
            return IsValidated;
        }
    }
}