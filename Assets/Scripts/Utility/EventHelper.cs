//***
// Author: Nate
// Description: EventHelper has public methods for button events and other accessible
//***

using FoxHerding.Handlers;
using UnityEngine;

namespace FoxHerding.Utility
{
    public class EventHelper : MonoBehaviour
    {
        public void SwitchScene(string sceneName)
        {
            SceneHandler.Instance.SwitchScene(sceneName);
        }

        public void ChangeCurrentPack(int newPack)
        {
            GameHandler.Instance.CurrentPack = (GameHandler.PuzzlePack)newPack;
        }
    }
}