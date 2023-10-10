//***
// Author: Nate
// Description: Autoassigns a camera to canvas
//***

using FoxHerding.Handlers;
using UnityEngine;

namespace FoxHerding.Cameras
{
    /// <summary>
    /// AutoAssignCamera
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public class AutoAssignCamera : MonoBehaviour
    {
        [SerializeField, Tooltip("Reference to the camera for this canvas")]
        private FoxCameraType cameraToAssign;
        
        private void Start()
        {
            Canvas canvas = GetComponent<Canvas>();
            canvas.worldCamera = CameraHandler.Instance.GetCamByType(cameraToAssign);
        }
    }
}