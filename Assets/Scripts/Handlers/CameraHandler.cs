//***
// Author: Nate
// Description: CameraHandler.cs handles camera logic and camera references
//***

using FoxHerding.Generics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace FoxHerding.Handlers
{
    public enum FoxCameraType { DoF, Main }
    /// <summary>
    /// CameraHandler
    /// </summary>
    public class CameraHandler : Handler<CameraHandler>
    {
        [field: SerializeField]
        public Camera MainCamRef { get; private set; }
        [field: SerializeField]
        public Camera DoFCamRef { get; private set; }
        [SerializeField] 
        private float dofSpeed = 0.2f;
        [SerializeField] 
        private float targetFocusDistance = 10f;
        [SerializeField]
        private Volume volume;
        private DepthOfField _depthOfField;
        private float _initialFocusDistance;
        private float _currentFocusDistance;
        private float _transitionTimer;

        protected override void Awake()
        {
            volume.profile.TryGet(out _depthOfField);
            _initialFocusDistance = _depthOfField.focusDistance.value;
            _currentFocusDistance = _initialFocusDistance;
        }

        private void Update()
        {
            if (_transitionTimer < dofSpeed)
            {
                _transitionTimer += Time.deltaTime;
                float t = Mathf.Clamp01(_transitionTimer / dofSpeed);

                // Smoothly transition the focus distance
                _currentFocusDistance = Mathf.Lerp(_initialFocusDistance, targetFocusDistance, t);
                _depthOfField.focusDistance.value = _currentFocusDistance;
            }
        }

        public void ResetDepthOfField()
        {
            print("reset");
            _initialFocusDistance = _currentFocusDistance; // Set the initial focus distance to the current value
            _transitionTimer = 0f; // Reset the transition timer
        }

        public void SetDepthOfField(float focusDistance)
        {
            _initialFocusDistance = _currentFocusDistance; // Set the initial focus distance to the current value
            targetFocusDistance = focusDistance; // Set the target focus distance
            _transitionTimer = 0f; // Reset the transition timer
        }
        
        public Vector3 FoxScreenToWorldPoint(Vector3 screenPosition)
        {
            Vector3 worldPosition = MainCamRef.ScreenToWorldPoint(screenPosition);
            worldPosition.z = 10f; // Set the z-coordinate to 10f
            return worldPosition;
        }

        public Camera GetCamByType(FoxCameraType camType)
        {
            switch (camType)
            {
                case FoxCameraType.Main:
                    return MainCamRef;
                case FoxCameraType.DoF:
                    return DoFCamRef;
            }
            return MainCamRef;
        }
    }
}