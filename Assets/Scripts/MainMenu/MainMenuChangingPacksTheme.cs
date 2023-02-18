//***
// Author: DroneMaker
// Modified by: Nate
// Description: changes the UI on pack selector to match the selected theme
//***

using System.Collections;
using System.Collections.Generic;
using FoxHerding.Handlers;
using UnityEngine;
using UnityEngine.UI;

namespace FoxHerding.MainMenu
{
    public class MainMenuChangingPacksTheme : MonoBehaviour
    {
        [SerializeField] 
        private List<Sprite> themeSprites;
        private Image _themeImage;
        private int _currentThemeIndex;
        [SerializeField] 
        private Scrollbar scrollbar;
        private float _currentStep;
        private float _lastStep;
        private Animator _packAnimator;

        private void Awake()
        {
            _themeImage = GetComponent<Image>();
            _packAnimator = GetComponent<Animator>();
            scrollbar.onValueChanged.AddListener(SetCurrentPack);
            StartCoroutine(SetScrollbarDefaultValues());
        }

        private void OnDestroy()
        {
            if (scrollbar == null) return;
            scrollbar.onValueChanged.RemoveListener(SetCurrentPack);
        }

        private void SetCurrentPack(float newPack)
        {
            //this is just to limit the onValueChanged event to be only executed once
            _currentStep = Mathf.Round(scrollbar.value * 10.0f);
            if (Mathf.Approximately(_currentStep, _lastStep)) return;
        
            int totalPacks = GameHandler.Instance.AvailablePieces.AllPossiblePacks.Count - 1;
            float currentValue = newPack * totalPacks;
            GameHandler.Instance.CurrentPack = (GameHandler.PuzzlePack)currentValue;
            SetThemeImage((int)currentValue);
            _lastStep = _currentStep;
            SaveLoadHandler.Instance.SaveData();
        }

        public void SetThemeImage(int listPosition)
        {
            _currentThemeIndex = listPosition;
            _themeImage.sprite = themeSprites[listPosition];
            _packAnimator.Play("PackChange", 0, 0);
        }

        public int GetCurrentThemeIndex()
        {
            return _currentThemeIndex;
        }

        private IEnumerator SetScrollbarDefaultValues()
        {
            yield return new WaitForEndOfFrame();
            int selectedPack = (int)GameHandler.Instance.CurrentPack;
            int totalPacks = GameHandler.Instance.AvailablePieces.AllPossiblePacks.Count - 1;
            scrollbar.value = (float)selectedPack / totalPacks;
        }
    }
}