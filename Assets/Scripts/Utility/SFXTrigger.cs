//***
// Author: Nate
// Description: SFXTrigger.cs is used to trigger sound effects through events primarily.
//***

using UnityEngine;

public class SFXTrigger : MonoBehaviour
{
    // called from object event component or animation events
    public void PlaySFX(AudioClip clip)
    {
        AudioHandler.Instance.PlaySound(clip);
    }
}
