//***
// Author: Nate
// Description: UIHandler.cs handles most of the game UI and UI references?
//***

using FoxHerding.Generics;
using FoxHerding.UI;
using UnityEngine;

namespace FoxHerding.Handlers
{
    public class UIHandler : Handler<UIHandler>
    {
        public InGameUI GameUI { get; set; }
        [SerializeField]
        private Texture2D defaultCursor;
        [SerializeField]
        private Texture2D selectableCursor;
        [SerializeField]
        private Animator optionsAnimator;

        public void SetDefaultCursor()
        {
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
        }

        public void SetSelectedCursor()
        {
            Cursor.SetCursor(selectableCursor, Vector2.zero, CursorMode.Auto);
        }

        public void ShowOptions()
        {
            optionsAnimator.Play("OptionsShow", 0, 0);
        }
    }
}