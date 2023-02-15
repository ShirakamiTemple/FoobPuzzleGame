//***
// Author: Nate
// Description: MainMenuNavigation controls navigating and animations on the main menu
//***

using System.Collections.Generic;
using UnityEngine;

public class MainMenuNavigation : MonoBehaviour
{
    [SerializeField] 
    private List<Vector2> positionViews = new();
    private Vector2 _positionToMove;
    [SerializeField] 
    private AnimationCurve movementCurve;
    [SerializeField] 
    private RectTransform objectToMove;
    private Vector3 _startPoint;
    private float _lerpTime;
    private bool _canLerp;

    private void Awake()
    {
        _positionToMove = objectToMove.anchoredPosition;
    }

    /// <summary>
    /// onClickEvent for changing main menu view. Uses enum int in order on the main menu
    /// title = 0, packs = 1, options = 2, credits = 3, levels = 5
    /// </summary>
    /// <param name="option"></param>
    public void OnClickChangeView(int option)
    {
        _lerpTime = 0;
        _startPoint = _positionToMove;
        _positionToMove = positionViews[option];
        _canLerp = true;
    }

    private void Update()
    {
        if (!_canLerp) return;

        if (_positionToMove == objectToMove.anchoredPosition)
        {
            _canLerp = false;
            return;
        }
        _lerpTime += Time.deltaTime;
        objectToMove.anchoredPosition = Vector2.Lerp(_startPoint, _positionToMove,  movementCurve.Evaluate(_lerpTime));
    }
}
