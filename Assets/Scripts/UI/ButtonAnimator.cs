
using UnityEngine;
using UnityEngine.UI;

namespace FoxHerding.UI
{
    public class ButtonAnimator : MonoBehaviour
    {
        [SerializeField]
        private bool disableButton;
        private Material _defaultMaterial;
        private MaskableGraphic _buttonImage;
        private static readonly int Hue = Shader.PropertyToID("_Hue");
        private static readonly int Alpha = Shader.PropertyToID("_Alpha");
        [SerializeField, Range(0, 360)]
        private float hoverColor;
        private Vector3 _originalScale;
        [SerializeField]
        private float hoverScale;
        private RectTransform _buttonRectTransform;

        private void Awake()
        {
            _buttonImage = GetComponent<MaskableGraphic>();
            _buttonRectTransform = GetComponent<RectTransform>();
            _originalScale = _buttonRectTransform.localScale;

            if (_buttonImage == null) return;
            
            _defaultMaterial = _buttonImage.material;
            _buttonImage.material = new Material(_defaultMaterial);
            _buttonImage.material.SetFloat(Alpha, disableButton ? 0.3f : 1f);
        }
        
        public void OnButtonHoverEnter()
        {
            _buttonRectTransform.localScale = _originalScale * hoverScale;

            if (_buttonImage == null) return;
            
            _buttonImage.material.SetFloat(Hue, hoverColor);
        }
        
        public void OnButtonHoverExit()
        {
            _buttonRectTransform.localScale = _originalScale;
            
            if (_buttonImage == null) return;

            if (_defaultMaterial.HasProperty(Hue))
            {
                _buttonImage.material.SetFloat(Hue, _defaultMaterial.GetFloat(Hue));
            }
        }
    }
}