//***
// Author: Nate
// Description: SplashScreenText.cs creates a wave and fade in for the splash screen text.
// This can actually just be used on any text component. Idk why I named it this tbh
//***

using System.Collections;
using FoxHerding.Utility;
using TMPro;
using UnityEngine;

namespace FoxHerding.UI
{
    public class SplashScreenText : MonoBehaviour
    {
        private readonly AnimationCurve _vertexCurve = new(new Keyframe(0, 0), new Keyframe(0.25f, 2.0f), 
            new Keyframe(0.5f, 0), new Keyframe(0.75f, 2.0f), new Keyframe(1, 0f));
        [SerializeField]
        private float curveScale = 1.0f;
        private TMP_Text _waveText;
        [SerializeField] 
        private float waveSpeed;

        private void Awake()
        {
            _waveText = GetComponent<TMP_Text>();
            _waveText.alpha = 0;
            StartCoroutine(AnimateWave());
        }
    
        private void ResetGeometry()
        {
            TMP_TextInfo textInfo = _waveText.textInfo;
            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                Vector3[] newVertexPositions = textInfo.meshInfo[i].vertices;
                // Upload the mesh with the revised information
                UpdateMesh(newVertexPositions, 0);
            }
            _waveText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            _waveText.ForceMeshUpdate(); // Generate the mesh and populate the textInfo with data we can use and manipulate.
        }

        private void UpdateMesh(Vector3[] vertex, int index)
        {
            _waveText.mesh.vertices = vertex;
            _waveText.mesh.uv = _waveText.textInfo.meshInfo[index].uvs0;
            _waveText.mesh.uv2 = _waveText.textInfo.meshInfo[index].uvs2;
            _waveText.mesh.colors32 = _waveText.textInfo.meshInfo[index].colors32;
        }

        private IEnumerator AnimateWave()
        {
            _vertexCurve.preWrapMode = WrapMode.Loop;
            _vertexCurve.postWrapMode = WrapMode.Loop;
            int loopCount = 0;
            while (true)
            {
                ResetGeometry();
                TMP_TextInfo textInfo = _waveText.textInfo;
                int characterCount = textInfo.characterCount;
                Vector3[] newVertexPositions = textInfo.meshInfo[0].vertices;

                for (int i = 0; i < characterCount; i++)
                {
                    if (!textInfo.characterInfo[i].isVisible) continue;
                
                    int vertexIndex = textInfo.characterInfo[i].vertexIndex;
                    float offsetY = _vertexCurve.Evaluate((float)i / characterCount + loopCount / 50f) * curveScale;
                    newVertexPositions[vertexIndex + 0].y += offsetY;
                    newVertexPositions[vertexIndex + 1].y += offsetY;
                    newVertexPositions[vertexIndex + 2].y += offsetY;
                    newVertexPositions[vertexIndex + 3].y += offsetY;
                }
                loopCount += 1;
                for (int i = 0; i < textInfo.meshInfo.Length; i++)
                {
                    textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                    _waveText.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
                }
                yield return Tools.GetWait(waveSpeed);
                if (_waveText.alpha < 1)
                {
                    _waveText.alpha += waveSpeed * 1.5f;
                }
            }
            // ReSharper disable once IteratorNeverReturns
        }
    }
}