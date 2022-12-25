using UnityEngine;
using UnityEngine.UI;

public class SnapPointData : MonoBehaviour
{
    [field: SerializeField]
    public bool IsOccupying { get; set; }
    private PuzzleTile _currentTile;
    private RawImage _image;

    private void Awake()
    {
        _image = GetComponentInChildren<RawImage>();
    }

    private void Update()
    {
        if (_image == null) return;

        if (IsOccupying)
        {
            _image.color = Color.blue;
            return;
        }
        _image.color = Color.red;
    }
}
