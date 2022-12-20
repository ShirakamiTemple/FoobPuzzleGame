//***
// Author: Nate
// Description: DragObject allows an object to be dragged with the mouse
//***

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DragObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float snapSpeed;
    private Vector3 _dragOffset, _mousePos;
    private bool _isDragging, _leftClickHeld, _shouldRotate, _shouldReset;
    private bool _canDrag = true;
    private Quaternion _newAngle, _oldAngle;
    private Transform _myTransform;
    private float _rotationTime;
    [SerializeField] private float snapDistance = 7f;
    
    private PuzzleGrid _grid;
    private PuzzlePiece _piece;

    private const float BaseScreenWidth = 1920f;
    private const float DistanceBuffer = 0.5f;

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
        HandleObjectReset();
    }

    private void HandleObjectReset()
    {
        if (!_shouldReset) return;
        if (_shouldRotate) return;
        
        _myTransform.rotation = Quaternion.Lerp(_oldAngle, _piece.StartRotation, _rotationTime);
        _rotationTime += Time.deltaTime * rotationSpeed;

        _myTransform.position = Vector3.Lerp(_myTransform.position, _piece.StartPosition, Time.deltaTime * snapSpeed);

        if (Vector3.Distance(_myTransform.position, _piece.StartPosition) <= DistanceBuffer &&
            Utility.Approximately(Utility.QuaternionAbs(_myTransform.rotation), Utility.QuaternionAbs(_piece.StartRotation)))
        {
            _myTransform.position = _piece.StartPosition;
            _shouldReset = false;
        }
    }

    private void HandleObjectRotation()
    {
        if (!_shouldRotate) return;
        if (_shouldReset) return;
        
        _myTransform.rotation = Quaternion.Lerp(_oldAngle, _newAngle, _rotationTime);
        _rotationTime += Time.deltaTime * rotationSpeed;
        
        if (Utility.Approximately(Utility.QuaternionAbs(_myTransform.rotation), Utility.QuaternionAbs(_newAngle)))
        {
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
        HandleLeftClick(eventData);
        HandleRightClick(eventData);
    }

    private void HandleLeftClick(PointerEventData data)
    {
        if (data.button != PointerEventData.InputButton.Left) return;
        if (_shouldReset) return;
        
        transform.SetAsLastSibling();
        _shouldReset = false;
        _leftClickHeld = true;
    }

    private void HandleRightClick(PointerEventData data)
    {
        if (data.button != PointerEventData.InputButton.Right) return;
        if (_shouldReset) return;
        
        _rotationTime = 0;
        _canDrag = false;
        _oldAngle = _myTransform.rotation;
        _newAngle = Quaternion.Euler(_myTransform.rotation.eulerAngles + new Vector3(0, 0, 90f));
        _shouldRotate = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        
        _leftClickHeld = false;
        
        //reset object if validation fails
        StartObjectReset();

    }

    private void StartObjectReset()
    {
        _rotationTime = 0;
        _oldAngle = transform.rotation;
        _shouldReset = true;
    }

    private void HandleObjectDrag()
    {
        if (!_leftClickHeld) return;
        if (!_canDrag) return;
        
        transform.position  = GetMousePos();
        float smallestDistanceSquared = snapDistance * snapDistance;
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

    private Vector3 CheckRotation(Transform point)
    {
        Vector3 temp;
        switch (Mathf.Abs(Mathf.RoundToInt(this.gameObject.transform.rotation.eulerAngles.z)))
        {
            case 0:
                temp = new Vector3(point.localPosition.x, point.localPosition.y, 0);
                break;

            case 90:
                temp = new Vector3(-point.localPosition.y, point.localPosition.x, 0);
                break;

            case 180:
                temp = new Vector3(-point.localPosition.x, -point.localPosition.y, 0);
                break;

            case 270:
                temp = new Vector3(point.localPosition.y, -point.localPosition.x, 0);
                break;

            default:
                temp = new Vector3(point.localPosition.x, point.localPosition.y, 0);
                break;
        }
        return temp;
    }
}