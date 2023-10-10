//***
// Author: Nate
// Description: EventHelper has public methods for button events and other accessible
//***

using FoxHerding.Handlers;
using FoxHerding.Puzzle;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FoxHerding.Utility
{
    public class EventHelper : MonoBehaviour
    {
        public void SwitchScene(string sceneName)
        {
            SceneHandler.Instance.SwitchScene(sceneName);
        }

        public void Exit()
        {
            Application.Quit();
        }

        public void ChangeCurrentPack(int newPack)
        {
            GameHandler.Instance.CurrentPack = (GameHandler.PuzzlePack)newPack;
        }

        public void DetermineNextLevel()
        {
            FindObjectOfType<PuzzleAnimatorHelper>()?.DetermineNextStage();
        }

        public void PlayButtonHover()
        {
            AudioHandler.Instance.PlaySound(AudioHandler.Instance.SfxClips[1]);
        }

        public void PlayButtonSubmit()
        {
            AudioHandler.Instance.PlaySound(AudioHandler.Instance.SfxClips[2]);
        }

        public void PlayPickupPieceUI()
        {
            UIHandler.Instance.GameUI.PieceDisplayPickupAnimation();
        }

        public void SetSelectCursor()
        {
            UIHandler.Instance.SetSelectedCursor();
        }
        
        public void SetDefaultCursor()
        {
            UIHandler.Instance.SetDefaultCursor();
        }
    }
}