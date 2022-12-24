using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuChangingPacksTheme : MonoBehaviour
{
    [SerializeField] List<Sprite> themeSprites;
    private Image themeImage;

    private int currentThemeIndex;

    private void Start()
    {
        themeImage = GetComponent<Image>();
    }

    public void SetThemeImage(int listPosition)
    {
        currentThemeIndex = listPosition;
        themeImage.sprite = themeSprites[listPosition];
    }

    public int GetCurrentThemeIndex()
    {
        return currentThemeIndex;
    }

}
