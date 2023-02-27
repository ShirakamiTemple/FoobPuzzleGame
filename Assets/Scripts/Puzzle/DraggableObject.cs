//***
// Author: Nate
// Description: DragObject allows an object to be dragged with the mouse
//***

using FoxHerding.Puzzle.Pieces;
using FoxHerding.UI;
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
        private const float SnapDistance = 6.5f;
        private const float BaseScreenWidth = 1920f;
        private const float DistanceBuffer = 0.5f;
        private Animator _animator;
        [SerializeField]
        private ParticleSystem pickupParticles;
        [SerializeField]
        private Camera particleCamera;
        [SerializeField]
        private RawImage particleImage;

        private void Awake()
        {
            _piece = GetComponent<PuzzlePiece>();
            _grid = FindObjectOfType<PuzzleGrid>();
            _animator = GetComponentInChildren<Animator>();
            _myTransform = transform;
            particleCamera.targetTexture = (RenderTexture)particleImage.texture;
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
            _myTransform.position = _piece.IsTouchingGrid ? GetMousePos() : Vector3.Lerp(transform.position, GetMousePos(), Time.deltaTime * snapSpeed);
            const float smallestDistanceSquared = SnapDistance * SnapDistance;
            foreach (PuzzleTile tile in _grid.Tiles)
            {
                foreach (Transform point in _piece.SnapPoints)
                {
                    if (Vector3.Distance(tile.transform.position, point.position) < smallestDistanceSquared)
                    {
                        Vector3 localPositionForRot = CheckRotation(point);
                        _myTransform.position =
                            tile.transform.position - new Vector3(localPositionForRot.x, localPositionForRot.y, 0) *
                            (Screen.width / BaseScreenWidth);
                        return;
                    }
                }
            }
        }

        private void ResetObject()
        {
            _myTransform.rotation = Quaternion.RotateTowards(_myTransform.rotation, _piece.StartRotation, Time.deltaTime * rotationSpeed);
            _myTransform.position = Vector3.Lerp(_myTransform.position, _piece.StartPosition, Time.deltaTime * snapSpeed);
            if (Vector3.Distance(_myTransform.position, _piece.StartPosition) <= DistanceBuffer &&
                Tools.Approximately(Tools.QuaternionAbs(_myTransform.rotation), Tools.QuaternionAbs(_piece.StartRotation)))
            {
                _animator.Play("Dropped", 0, 0);
                _myTransform.position = _piece.StartPosition;
                _shouldReset = false;
            }
        }

        private void RotateObject()
        {
            _myTransform.rotation = Quaternion.RotateTowards(_myTransform.rotation, _newAngle, Time.deltaTime * rotationSpeed);
            if (Tools.Approximately(Tools.QuaternionAbs(_myTransform.rotation), Tools.QuaternionAbs(_newAngle)))
            {
                _myTransform.rotation = _newAngle;
                if (_piece.IsValidated && !_piece.CheckValidation())
                {
                    StartObjectReset();
                }
                _canDrag = true;
                _shouldRotate = false;
            }
        }

        private static Vector3 GetMousePos()
        {
            return Mouse.current.position.ReadValue();
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
                    FoobabyAnimator.PlayPickedUpFoob();
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

            FoobabyAnimator.PlayNormalFoob();
            if (!_piece.CheckValidation() && _piece.IsTouchingGrid)
            {
                StartObjectReset();
                _leftClickHeld = false;
                return;
            }
            if (_piece.IsTouchingGrid)
            {
                pickupParticles.gameObject.SetActive(true);
                pickupParticles.Play();
            }
            _animator.Play(_piece.IsTouchingGrid ? "Placed" : "Dropped", 0, 0);
            _leftClickHeld = false;
        }

        //starts the objects reset phase
        private void StartObjectReset()
        {
            _shouldReset = true;
        }

        // CheckRotation checks the current rotation of the game object and calculates the correct local position of a given
        //snap point based on that rotation. It uses a switch statement to handle the different rotation angles and returns
        //the adjusted local position for use in the position calculation for snapping to a grid tile.
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