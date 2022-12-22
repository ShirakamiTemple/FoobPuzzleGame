using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DragObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float speed;
    private Vector3 _dragOffset;
    private Camera _cam;
    private Vector3 _mousePos;
    private bool _isDragging, _shouldRotate;

    private Quaternion _newAngle;
    private Quaternion _oldAngle;
    private Transform _myTransform;
    private float _rotationTime;
    
    private void Start()
    {
        _cam = Camera.main;
        _myTransform = transform;
    }

    private void Update()
    {
        HandleObjectDrag();
        //HandleObjectRotation();
    }

    private void FixedUpdate()
    {
        HandleObjectRotation();
    }

    private void HandleObjectDrag()
    {
        if (!_isDragging) return;
        
        Vector3 mousePos = GetMousePos();
        transform.position = Vector3.Lerp(transform.position, mousePos , speed * Time.deltaTime);
    }

    private void HandleObjectRotation()
    {
        if (!_shouldRotate) return;
        
        _myTransform.rotation = Quaternion.Slerp(_oldAngle, _newAngle, _rotationTime);
        _rotationTime += Time.deltaTime * speed;
        
        if (Utility.Approximately(_oldAngle, _newAngle))
        {
            _shouldRotate = false;
        }
    }

    private Vector3 GetMousePos()
    {
        // uncomment this stuff and return _mousePos if you want to use sprites instead of canvas images
        // _mousePos = Mouse.current.position.ReadValue();
        //
        // //Moving a 2d object, so the z needs to be opposite of the camera
        // _mousePos.z = -_cam.transform.position.z;    
        //
        // _mousePos = _cam.ScreenToWorldPoint(_mousePos);

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
        _isDragging = true;
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
        _isDragging = false;
    }
}