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
    private RectTransform _myRect;

    private void Start()
    {
        _myTransform = transform;
        _myRect = GetComponent<RectTransform>();
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
        
        _myRect.SetAsLastSibling();
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
        // if (IsMovementLocked) return;
        // if (!_leftClickHeld) return;
        //
        // if (!_canDrag)
        // {
        //     if (Mathf.RoundToInt(Mathf.Abs(Vector3.Distance(GetMousePos(), MovementLockStart))) >= _movementLockBuffer)
        //     {
        //         print("distance op");
        //         _canDrag = true;
        //     }
        // }
    }
    
    private void HandleObjectDrag()
    {
        if (!_leftClickHeld) return;
        if (!_canDrag) return;
        
        Vector3 mousePos = GetMousePos();
        //transform.position = Vector3.Lerp(transform.position, mousePos, dragSpeed);
        transform.position = mousePos;
        transform.position = new Vector2(Mathf.RoundToInt(transform.position.x / _cellSize) * _cellSize,
            Mathf.RoundToInt(transform.position.y / _cellSize) * _cellSize);
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