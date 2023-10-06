using UnityEngine;

namespace FoxHerding.UI
{
    public class SettingsHelper : MonoBehaviour
    {
        [SerializeField]
        private Transform parentObject;
        [SerializeField]
        private float animationDuration = 0.3f;
        [SerializeField]
        private AnimationCurve animationCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        private int _childCount;
        private int _activeChildIndex;
        private float _currentAnimationTime;
        private RectTransform[] _childRectTransforms;

        private void Start()
        {
            _childCount = parentObject.childCount;
            _childRectTransforms = new RectTransform[_childCount];
            for (int i = 0; i < _childCount; i++)
            {
                _childRectTransforms[i] = parentObject.GetChild(i).GetComponent<RectTransform>();
            }
        }

        public void OnSliderValueChanged(float value)
        {
            _activeChildIndex = (int)Mathf.Clamp(value, 0, _childCount - 1);
            _currentAnimationTime = 0f;
        }

        private void Update()
        {
            _currentAnimationTime += Time.deltaTime;
            for (int i = 0; i < _childCount; i++)
            {
                Transform child = parentObject.GetChild(i);
                RectTransform childRectTransform = _childRectTransforms[i];
                bool isActive = i == _activeChildIndex;
                child.gameObject.SetActive(isActive);
                if (!isActive) continue;
                
                float t = Mathf.Clamp01(_currentAnimationTime / animationDuration);
                float progress = animationCurve.Evaluate(t);
                Vector2 targetScale = Vector2.one;
                childRectTransform.localScale = Vector2.Lerp(Vector2.zero, targetScale, progress);
            }
        }
    }
}