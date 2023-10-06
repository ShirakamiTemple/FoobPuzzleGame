using TMPro;
using UnityEngine;

namespace FoxHerding.Utility
{
    public class VersionController : MonoBehaviour
    {
        private TextMeshProUGUI _textMesh;

        private void Awake()
        {
            _textMesh = GetComponentInChildren<TextMeshProUGUI>();
            _textMesh.text = "Alpha Version: " + Application.version;
        }
    }
}