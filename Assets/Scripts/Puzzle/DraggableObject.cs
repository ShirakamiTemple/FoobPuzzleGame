//***
// Author: Nate
// Description: DragObject allows an object to be dragged with the mouse
//***

using FoxHerding.Handlers;
using FoxHerding.Puzzle.Pieces;
using FoxHerding.Utility;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace FoxHerding.Puzzle
{
    public class DraggableObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField, Tooltip("How fast an object will rotate")]
        private float rotationSpeed;
        [SerializeField, Tooltip("How fast an object will snap")]
        private float snapSpeed;
        private bool _leftClickHeld, _shouldRotate, _shouldReset;
        private bool _canDrag = true;
        private Quaternion _newAngle;
        private Transform _myTransform;
        private PuzzleGrid _grid;
        private PuzzlePiece _piece;
        private const float SnapThreshold = 0.5f;
        private Animator _animator;

        private void Awake()
        {
            _piece = GetComponent<PuzzlePiece>();
            _grid = FindObjectOfType<PuzzleGrid>();
            _animator = GetComponentInChildren<Animator>();
            _myTransform = transform;
        }

        private void Update()
        {
            HandleObjectInteractions();
        }

        private void HandleObjectInteractions()
        {
            if (_shouldReset)
            {
                ResetObject();
            }
            else if (_shouldRotate)
            {
                RotateObject();
            }
            else if (_leftClickHeld && _canDrag)
            {
                DragObject();
            }
        }

        private void DragObject()
        {
            _myTransform.position = GetMousePos();
        }

        private void ResetObject()
        {
            _myTransform.rotation = Quaternion.RotateTowards(_myTransform.rotation, _piece.StartRotation, Time.deltaTime * rotationSpeed);
            _myTransform.position = Vector3.Lerp(_myTransform.position, CameraHandler.Instance.FoxScreenToWorldPoint(_piece.StartPosition), snapSpeed * Time.deltaTime);
            _animator.Play("Dropped", 0, 0);
            float distance = Vector3.Distance( _myTransform.position, CameraHandler.Instance.FoxScreenToWorldPoint(_piece.StartPosition));
            if (distance > 0.01f) return;

                _shouldReset = false;
            }

        private void RotateObject()
        {
            _myTransform.rotation = Quaternion.RotateTowards(_myTransform.rotation, _newAngle, Time.deltaTime * rotationSpeed);
            if (Tools.Approximately(Tools.QuaternionAbs(_myTransform.rotation), Tools.QuaternionAbs(_newAngle)))
            {
                _myTransform.rotation = _newAngle;
                if (_piece.IsValidated && !_piece.CheckValidation())
                {
                    _shouldReset = true;
                }
                _canDrag = true;
                _shouldRotate = false;
            }
        }
        
        private void SnapToTile()
        {
            foreach (Transform snapPoint in _piece.SnapPoints)
            {
                foreach (PuzzleTile tile in _grid.Tiles)
                {
                    RectTransform tileSnapPoint = tile.GetComponentInChildren<RectTransform>(); // Assuming the snap point is the first child RectTransform of the tile
                    // Calculate the distance between the snap point and the tile snap point
                    float distance = Vector2.Distance(snapPoint.position, tileSnapPoint.position);
                    if (distance < SnapThreshold)
                    {
                        // Snap the piece to the tile snap point
                        _piece.transform.position += tileSnapPoint.position - snapPoint.position;
                        break; // Exit the loop once a snap occurs
                    }
                }
            }
        }

        private static Vector3 GetMousePos()
        {
            // Get the mouse position in screen space
            Vector3 mouseScreenPosition = Mouse.current.position.ReadValue();
            return CameraHandler.Instance.FoxScreenToWorldPoint(mouseScreenPosition);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            HandleClick(eventData);
        }

        private void HandleClick(PointerEventData data)
        {
            if (_shouldReset) return;

            switch (data.button)
            {
                case PointerEventData.InputButton.Left:
                    _piece.IsValidated = false;
                    _animator.Play(_piece.IsTouchingGrid ? "PickedUpIdle" : "PickedUp", 0, 0);
                    UIHandler.Instance.GameUI.PlaySukonbuReaction("Pickup");
                    transform.SetAsLastSibling();
                    _shouldReset = false;
                    _leftClickHeld = true;
                    break;
                case PointerEventData.InputButton.Right when !_shouldRotate:
                    _canDrag = false;
                    _newAngle = Quaternion.Euler(_myTransform.rotation.eulerAngles + new Vector3(0, 0, 90f));
                    _shouldRotate = true;
                    break;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left) return;
    
            UIHandler.Instance.GameUI.PlaySukonbuReaction("Idle");
            _animator.Play(_piece.IsTouchingGrid ? "Placed" : "Dropped", 0, 0);
            if (!_piece.CheckValidation() && _piece.IsTouchingGrid)
            {
                _shouldReset = true;
                _leftClickHeld = false;
                return;
            }
            if (_piece.IsTouchingGrid)
            {
                SnapToTile();
            }
            _leftClickHeld = false;
            if (_piece.CheckValidation())
            {
                _grid.AttemptToProceedToNextStage();
            }
        }

        // CheckRotation checks the current rotation of the game object and calculates the correct local position of a given
        // snap point based on that rotation. It returns
        // the adjusted local position for use in the position calculation for snapping to a grid tile.
        private Vector3 CheckRotation(Transform point)
        {
            Vector3 currentPosition = point.localPosition;
            int angle = Mathf.RoundToInt(gameObject.transform.rotation.eulerAngles.z) % 360;
            float sin = Mathf.Sin(angle * Mathf.Deg2Rad);
            float cos = Mathf.Cos(angle * Mathf.Deg2Rad);
            Vector3 newEuler = new(
                currentPosition.x * cos - currentPosition.y * sin,
                currentPosition.x * sin + currentPosition.y * cos,
                0);
            return newEuler;
        }
    }
}