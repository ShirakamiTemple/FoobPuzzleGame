//***
// Author: Nate
// Description: DontDestroy keeps objects and children from destroying on new scene load
//***

using UnityEngine;

namespace FoxHerding.Utility
{
    public class DontDestroy : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}