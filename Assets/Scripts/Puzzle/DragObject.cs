//***
// Author: Nate
// Description: DragObject allows an object to be dragged with the mouse
//***

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DragObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField, Tooltip("How fast an object will rotate")] private float rotationSpeed;
    [SerializeField, Tooltip("How fast an object will snap")] private float snapSpeed;
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

    private void Start()
    {
        _piece = GetComponent<PuzzlePiece>();
        _grid = FindObjectOfType<PuzzleGrid>();
        _myTransform = transform;
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleObjectDrag();
        HandleObjectRotation();
        HandleObjectReset();
    }
    
    //handles if an object should be dragged and how it should be dragged
    private void HandleObjectDrag()
    {
        if (!_leftClickHeld) return;
        if (!_canDrag) return;

        //if the object is not touching the grid interpolate the movement. Otherwise just snap it to the grid based on distance.
        transform.position = _piece.IsTouchingGrid ? GetMousePos() : Vector3.Lerp(transform.position, GetMousePos(), Time.deltaTime * snapSpeed);
        
        float smallestDistanceSquared = SnapDistance * SnapDistance;
        //check the grid tiles to see if any point on the object is touching a grid tile collision point
        //if they touch snap to the grid tile.
        //multiply the result by: (Screen.width / BaseScreenWidth) so it will keep movement responsive to screen size
        foreach (PuzzleTile tile in _grid.Tiles)
        {
            foreach (Transform point in _piece.SnapPoints)
            {
                if (Vector3.Distance(tile.transform.position, point.position) < smallestDistanceSquared)
                {
                    Vector3 localPositionForRot = CheckRotation(point);
                    transform.position = 
                        tile.transform.position - new Vector3(localPositionForRot.x, localPositionForRot.y, 0) *
                        (Screen.width / BaseScreenWidth);
                    return;
                }
            }
        }
    }

    //resets and object back to its starting position and rotation
    private void HandleObjectReset()
    {
        if (!_shouldReset) return;

        _myTransform.rotation = Quaternion.RotateTowards(_myTransform.rotation, _piece.StartRotation, Time.deltaTime * rotationSpeed);
        _myTransform.position = Vector3.Lerp(_myTransform.position, _piece.StartPosition, Time.deltaTime * snapSpeed);

        //if the object has reached its starting position and starting rotation exit the method
        if (Vector3.Distance(_myTransform.position, _piece.StartPosition) <= DistanceBuffer &&
            Utility.Approximately(Utility.QuaternionAbs(_myTransform.rotation), Utility.QuaternionAbs(_piece.StartRotation)))
        {
            _animator.Play("Dropped", 0, 0);
            _myTransform.position = _piece.StartPosition;
            _shouldReset = false;
        }
    }

    //rotates an object if it can rotate
    private void HandleObjectRotation()
    {
        if (!_shouldRotate) return;

        _myTransform.rotation = Quaternion.RotateTowards(_myTransform.rotation, _newAngle, Time.deltaTime * rotationSpeed);

        //if object has reached its starting rotation, check for validation and then exit
        if (Utility.Approximately(Utility.QuaternionAbs(_myTransform.rotation), Utility.QuaternionAbs(_newAngle)))
        {
            _myTransform.rotation = _newAngle;
            if (_piece.IsValidated)
            {
                if (!_piece.CheckValidation())
                {
                    StartObjectReset();
                }
            }
            _canDrag = true;
            _shouldRotate = false;
        }
    }

    private static Vector3 GetMousePos()
    {
        return Mouse.current.position.ReadValue();
    }

    //when the mouse is clicked (this can be left, right, middle, or any side buttons on a mouse
    public void OnPointerDown(PointerEventData eventData)
    {
        HandleLeftClick(eventData);
        HandleRightClick(eventData);
    }

    //handles left clicking
    private void HandleLeftClick(PointerEventData data)
    {
        if (data.button != PointerEventData.InputButton.Left) return;
        if (_shouldReset) return;

        _piece.IsValidated = false;
        //if the piece is touching the grid and button is pressed just played picked up idle
        //otherwise play the full opening animation if one exists
        _animator.Play(_piece.IsTouchingGrid ? "PickedUpIdle" : "PickedUp", 0, 0);
        transform.SetAsLastSibling();
        _shouldReset = false;
        _leftClickHeld = true;
    }

    //handles right clicking
    private void HandleRightClick(PointerEventData data)
    {
        if (data.button != PointerEventData.InputButton.Right) return;
        if (_shouldReset) return;
        if (_shouldRotate) return;
        
        _canDrag = false;
        _newAngle = Quaternion.Euler(_myTransform.rotation.eulerAngles + new Vector3(0, 0, 90f));
        _shouldRotate = true;
    }

    //handles when you release a click on the mouse. can be any mouse button
    public void OnPointerUp(PointerEventData eventData)
    {
        //right now only left click release is used. so exit if its not the left mouse button
        if (eventData.button != PointerEventData.InputButton.Left) return;

        //reset object if validation fails && it is touching the grid
        if (!_piece.CheckValidation() && _piece.IsTouchingGrid)
        {
            StartObjectReset();
            _leftClickHeld = false;
            return;
        }
        
        //if the piece is touching the grid play the placed animation otherwise it will be dropped
        _animator.Play(_piece.IsTouchingGrid ? "Placed" : "Dropped", 0, 0);

        _leftClickHeld = false;
    }

    //starts the objects reset phase
    private void StartObjectReset()
    {
        _shouldReset = true;
    }

    //kinda of ugly hardcoded exceptions for rotation local position to ensure colliders stay in the right place
    private Vector3 CheckRotation(Transform point)
    {
        Vector3 currentPosition = point.localPosition;
        Vector3 temp = Mathf.Abs(Mathf.RoundToInt(gameObject.transform.rotation.eulerAngles.z)) switch
        {
            0 => new Vector3(currentPosition.x, currentPosition.y, 0),
            90 => new Vector3(-currentPosition.y, currentPosition.x, 0),
            180 => new Vector3(-currentPosition.x, -currentPosition.y, 0),
            270 => new Vector3(currentPosition.y, -currentPosition.x, 0),
            _ => new Vector3(currentPosition.x, currentPosition.y, 0)
        };
        return temp;
    }
}