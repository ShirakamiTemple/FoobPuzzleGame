using UnityEngine;

[CreateAssetMenu(fileName = "CreditInfo", menuName = "CreditsTemplate/CreditsInfo")]
public class CreditTemplate : ScriptableObject
{
    [field: Header("Person credits info")]
    [field: SerializeField] public string creditName { get; private set; }

    [field: SerializeField] public string creditNote { get; private set; }

    [field: SerializeField] public Sprite creditSprite { get; private set; }

    [field: SerializeField] public string creditFunction { get; private set; }


}
