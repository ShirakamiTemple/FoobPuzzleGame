//***
// Author: Nate
// Description: Controls sway speed of menu corn lol
//***

using UnityEngine;

namespace FoxHerding.MainMenu
{
    public class CornSwaySpeed : MonoBehaviour
    {
        private const float MIN_SWAY_SPEED = 0.85f;
        private const float MAX_SWAY_SPEED = 1.15f;
        
        private void Awake()
        {
            GetComponent<Animator>().speed = Random.Range(MIN_SWAY_SPEED, MAX_SWAY_SPEED);
        }
    }
}