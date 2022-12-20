using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
        _color = _image.color;
    }

    //need this to be a coroutine so it will wait 1 frame before executing.
    //if you move a piece to fast sometimes onexit will trigger when onenter triggers and causes a confliction
    private IEnumerator OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag(ColTag)) yield break;
        
        yield return null;
        
        IsOccupied = true;
        if (_currentSnapPoint == null)
        {
            _currentSnapPoint = col.gameObject.GetComponent<SnapPointData>();
        }
        _currentSnapPoint.IsOccupying = true;
        _image.color = Color.magenta;
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag(ColTag)) return;
        if (_currentSnapPoint == null)
        {
            _currentSnapPoint = col.gameObject.GetComponent<SnapPointData>();
        }

        if (_currentSnapPoint != col.gameObject.GetComponent<SnapPointData>()) return;
        
        _currentSnapPoint.IsOccupying = false;
        IsOccupied = false;
        _currentSnapPoint = null;
        _image.color = _color;
    }
}
