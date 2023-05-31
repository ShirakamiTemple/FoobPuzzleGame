//***
// Author: Nate
// Description: UIHandler.cs handles most of the game UI and UI references?
//***

using FoxHerding.Generics;
using FoxHerding.UI;

namespace FoxHerding.Handlers
{
    public class UIHandler : Handler<UIHandler>
    {
        public InGameUI GameUI { get; set; }
    }
}
