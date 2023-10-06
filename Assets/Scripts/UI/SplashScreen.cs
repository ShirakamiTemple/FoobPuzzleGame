//***
// Author: Nate
// Description: Splashscreen.cs handles the functionality of the splash screen
//***

using System;
using System.Collections;
using FoxHerding.Handlers;
using FoxHerding.Utility;
using UnityEngine;
using UnityEngine.Video;

namespace FoxHerding.UI
{
    public class SplashScreen : MonoBehaviour
    {
        [SerializeField, Tooltip("Reference to intro video player")]
        private VideoPlayer videoPlayer;
        [SerializeField, Tooltip("How long to delay input for continuing to main menu")]
        private float inputDelayTime = 1f;

        private void Awake()
        {
            videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath,"FoobHerdinIntro.mp4");
        }

        private IEnumerator Start()
        {
            videoPlayer.loopPointReached += TransitionToMainMenu;
            yield return Tools.GetWait(inputDelayTime);

            while (!Input.anyKeyDown)
            {
                yield return null;
            }
            TransitionToMainMenu();
        }

        private static void TransitionToMainMenu(VideoPlayer source = null)
        {
            SceneHandler.Instance.SwitchScene("MainMenu");
        }

        private void OnDestroy()
        {
            videoPlayer.loopPointReached -= TransitionToMainMenu;
        }
    }
}