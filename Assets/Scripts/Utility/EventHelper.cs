//***
// Author: Nate
// Description: EventHelper has public methods for button events and other accessible
//***

using UnityEngine;

public class EventHelper : MonoBehaviour
{
    public void SwitchScene(string sceneName)
    {
        SceneHandler.Instance.SwitchScene(sceneName);
    }
}
