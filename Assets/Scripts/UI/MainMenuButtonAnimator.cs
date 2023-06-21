
using UnityEngine;
using UnityEngine.UI;

namespace FoxHerding.UI
{
    public class MainMenuButtonAnimator : MonoBehaviour
    {
        private Material _defaultMaterial;
        private MaskableGraphic _buttonImage;
        private static readonly int Hue = Shader.PropertyToID("_Hue");
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
            _defaultMaterial = _buttonImage.material;
            _buttonImage.material = new Material(_defaultMaterial);
            _originalScale = _buttonRectTransform.localScale;
        }
        
        public void OnButtonHoverEnter()
        {
            _buttonImage.material.SetFloat(Hue, hoverColor);
            _buttonRectTransform.localScale = _originalScale * hoverScale;
        }
        
        public void OnButtonHoverExit()
        {
            _buttonImage.material.SetFloat(Hue, _defaultMaterial.GetFloat(Hue));
            _buttonRectTransform.localScale = _originalScale;
        }
    }
}