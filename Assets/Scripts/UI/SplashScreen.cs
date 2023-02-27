using System.Collections;
using FoxHerding.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FoxHerding.UI
{
    public class SplashScreen : MonoBehaviour
    {
        [SerializeField, Tooltip("How long to delay input for continuing to main menu")]
        private float inputDelayTime = 1f;

        private IEnumerator Start()
        {
            yield return Tools.GetWait(inputDelayTime);

            while (!Input.anyKeyDown)
            {
                yield return null;
            }
            SceneManager.LoadScene("MainMenu");
        }
    }
}