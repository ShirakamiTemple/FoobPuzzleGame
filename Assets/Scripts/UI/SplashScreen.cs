using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    [SerializeField, Tooltip("How long to delay input for continuing to main menu")]
    private float inputDelayTime = 1f;

    private IEnumerator Start()
    {
        yield return Utility.GetWait(inputDelayTime);

        while (!Input.anyKeyDown)
        {
            yield return null;
        }
        SceneManager.LoadScene("MainMenu");
    }
}