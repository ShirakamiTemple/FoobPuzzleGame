//***
// Author: Nate
// Description: DontDestroy keeps objects and children from destroying on new scene load
//***

using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}