using FoxHerding.Handlers;
using UnityEngine;

namespace FoxHerding.Cameras
{
    public class HandleDoF : MonoBehaviour
    {
        private const float Amount = 3;
        private const float DefaultAmount = 10;
        private void OnEnable()
        {
            CameraHandler.Instance.SetDepthOfField(Amount);
        }

        private void OnDisable()
        {
            CameraHandler.Instance.SetDepthOfField(DefaultAmount);
        }
    }
}
