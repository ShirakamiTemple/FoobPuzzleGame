//***
// Author: Nate
// Description: PuzzlePiece is the main logic for handling a piece on a grid
//***

using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    [SerializeField] private float snapSpeed;
    [SerializeField] private RectTransform myRect;
    [SerializeField] private List<BoxCollider2D> snapPoints = new();
    [SerializeField] private DragObject dragObject;
    private bool _hasCollided, _shouldMove;
    private Vector3 _targetPosition;

    private void Update()
    {
        // if (!_shouldMove) return;
        //
        // if (transform.position.Equals(_targetPosition))
        // {
        //     dragObject.IsMovementLocked = false;
        //     _shouldMove = false;
        //     _hasCollided = false;
        // }
        //
        // transform.position = Vector3.MoveTowards(transform.position, _targetPosition , snapSpeed * Time.deltaTime);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if (_hasCollided) return;
        //
        // dragObject.MovementLockStart = dragObject.GetMousePos();
        // dragObject.IsMovementLocked = true;
        // dragObject.SetDrag(false);
        // Vector2 boundsCenter = collision.collider.bounds.center;
        // Vector2 colliderOffset = collision.otherCollider.offset;
        // _targetPosition = new Vector3(boundsCenter.x - colliderOffset.x, boundsCenter.y - colliderOffset.y, 0);
        // _hasCollided = true;
        // _shouldMove = true;
    }
}
