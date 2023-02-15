//***
// Author: Nate
// Description: FoobabyAnimator.cs just controls foobaby
//***

using UnityEngine;

public class FoobabyAnimator : MonoBehaviour
{
    private static Animator _foobabyAnimator;

    private void Awake()
    {
        _foobabyAnimator = GetComponent<Animator>();
    }

    public static void PlayPickedUpFoob()
    {
        _foobabyAnimator.Play("Disabled", 0, 0);
    }

    public static void PlayNormalFoob()
    {
        _foobabyAnimator.Play("Normal", 0, 0);
    }
}
