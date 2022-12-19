//***
// Author: Nate
// Description: DragObject allows an object to be dragged with the mouse
//***

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DragObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler, IPointerExitHandler
{
    [SerializeField] private float dragSpeed;
    [SerializeField] private float rotationSpeed;
    private Vector3 _dragOffset, _mousePos;
    private bool _isDragging, _leftClickHeld, _shouldRotate;
    private bool _canDrag = true;
    private Quaternion _newAngle, _oldAngle;
    private Transform _myTransform;
    private float _rotationTime;

    private float _cellSize = 150f;

    private float _snapDistance = 10f;

    private PuzzleGrid _grid;
    private PuzzlePiece _piece;

    private void Start()
    {
        _piece = GetComponent<PuzzlePiece>();
        _grid = FindObjectOfType<PuzzleGrid>();
        _myTransform = transform;
    }

    private void Update()
    {
        HandleObjectDrag();
        HandleObjectRotation();
    }

    private void HandleObjectRotation()
    {
        if (!_shouldRotate) return;
        
        _myTransform.rotation = Quaternion.Lerp(_oldAngle, _newAngle, _rotationTime);
        _rotationTime += Time.deltaTime * rotationSpeed;
        
        if (Utility.Approximately(_oldAngle, _newAngle))
        {
            _shouldRotate = false;
        }
    }

    public Vector3 GetMousePos()
    {
        return Mouse.current.position.ReadValue();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        HandleLeftClick(eventData);
        HandleRightClick(eventData);
    }

    private void HandleLeftClick(PointerEventData data)
    {
        if (data.button != PointerEventData.InputButton.Left) return;
        
        transform.SetAsLastSibling();
        _leftClickHeld = true;
    }

    private void HandleRightClick(PointerEventData data)
    {
        if (data.button != PointerEventData.InputButton.Right) return;
        _rotationTime = 0;
        _oldAngle = _myTransform.rotation;
        _newAngle = Quaternion.Euler(_myTransform.rotation.eulerAngles + new Vector3(0, 0, 90f));
        _shouldRotate = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        
        _leftClickHeld = false;
    }

    public void OnPointerMove(PointerEventData eventData)
    {

    }
    
    private void HandleObjectDrag()
    {
        if (!_leftClickHeld) return;
        if (!_canDrag) return;
        
        transform.position  = GetMousePos();
        float smallestDistanceSquared = _snapDistance * _snapDistance;
        foreach (PuzzleTile tile in _grid.Tiles)
        {
            foreach (Transform point in _piece.SnapPoints)
            {
                if (Vector3.Distance(tile.transform.position, point.position) < smallestDistanceSquared)
                {
                    //_canDrag = false;
                    //print("Point " + point.name + " is close to " + tile.name);
                    transform.position = tile.transform.position - point.localPosition;
                    //smallestDistanceSquared = Vector3.Distance(tile.transform.position, point.position);
                    return;
                }
            }
        }
    }

    public void SetDrag(bool drag)
    {
        _canDrag = drag;
    }

    public bool CanDrag()
    {
        return _canDrag;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // if (IsMovementLocked) return;
        // if (!_leftClickHeld) return;
        //
        // SetDrag(true);
    }
}