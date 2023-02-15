//***
// Author: Nate
// Description: SnapPointData.cs holds information about a pieces cells snap points
//***

using UnityEngine;
using UnityEngine.UI;

public class SnapPointData : MonoBehaviour
{
    public bool IsOccupying { get; set; }
    private RawImage _image;

    private void Awake()
    {
        _image = GetComponentInChildren<RawImage>();
    }

    private void Update()
    {
        if (_image == null) return;

        _image.color = IsOccupying ? Color.blue : Color.red;
    }
}