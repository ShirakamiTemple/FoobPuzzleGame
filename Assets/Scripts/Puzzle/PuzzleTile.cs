//***
// Author: Nate
// Description: PuzzleTile is the main logic for handling a tile on a grid and storing its data and triggers
//***

using System.Collections;
using FoxHerding.Puzzle.Pieces;
using UnityEngine;
using UnityEngine.UI;

namespace FoxHerding.Puzzle
{
    public class PuzzleTile : MonoBehaviour
    {
        public bool IsOccupied { get; private set; }
        private SnapPointData _currentSnapPoint;
        private const string ColTag = "PuzzlePiece";
        private PuzzlePiece _piece;
        private RawImage _image;
        private Color _color;

        private void Awake()
        {
            _image = GetComponent<RawImage>();
            if (_image != null)
            {
                _color = _image.color;
            }
        }

        //need this to be a coroutine so it will wait 1 frame before executing.
        //if you move a piece to fast sometimes onexit will trigger when onenter triggers and causes a conflict
        private IEnumerator OnTriggerEnter2D(Collider2D col)
        {
            if (!col.gameObject.CompareTag(ColTag)) yield break;
            yield return null;
        
            IsOccupied = true;
            if (_currentSnapPoint == null)
            {
                _currentSnapPoint = col.gameObject.GetComponent<SnapPointData>();
            }
            //check if the parent is in obstacle. If it is it will always be occupied so nothing can be placed there
            _currentSnapPoint.IsOccupying = !transform.parent.CompareTag("Obstacle");
            if (_image != null)
            {
                _image.color = Color.magenta;
            }
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            if (!col.gameObject.CompareTag(ColTag)) return;
        
            if (_currentSnapPoint != col.gameObject.GetComponent<SnapPointData>()) return;
        
            _currentSnapPoint.IsOccupying = false;
            IsOccupied = false;
            _currentSnapPoint = null;
            _image.color = _color;
        }
    }
}