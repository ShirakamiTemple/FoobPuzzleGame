
using System.Collections.Generic;
using FoxHerding.Data.Levels;
using FoxHerding.Handlers;
using FoxHerding.Puzzle.Pieces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FoxHerding.UI
{
    public class InGameUI : MonoBehaviour
    {
        [SerializeField, Tooltip("Reference to puzzle win animator")]
        private Animator puzzleWinAnimator;
        [SerializeField, Tooltip("Reference to newPiece animator")]
        private Animator newPieceNoticeAnimator;
        [SerializeField, Tooltip("Reference to the newPieceDisplay Animator")]
        private Animator newPieceDisplayAnimator;
        [SerializeField, Tooltip("Reference to the newPieceDisplay text name field")]
        private TextMeshProUGUI newPieceName;
        [SerializeField, Tooltip("Reference to the newPieceDisplay text description field")]
        private TextMeshProUGUI newPieceDescription;
        [SerializeField, Tooltip("Reference to tile holder parent transform")]
        private Transform tileHolderParent;
        [SerializeField, Tooltip("Reference to level UI text")]
        private TextMeshProUGUI currentLevel;
        [SerializeField, Tooltip("reference to the main game animator")] 
        private Animator gameAnimator;
        [SerializeField, Tooltip("Reference to sukonbu reaction animator")] 
        private Animator sukonbuAnimator;
        [SerializeField, Tooltip("Reference to the current level animator")]
        private Animator currentLevelAnimator;
        [SerializeField, Tooltip("Reference to the settings slider animator")]
        private Animator settingsSlider;
        private bool settingsSliderIsVisible;
        [SerializeField, Tooltip("Reference to nejima animator")]
        private Animator nejimaAnimator;
        private static readonly int SettingsHovered = Animator.StringToHash("settingsHovered");

        private void Awake()
        {
            UIHandler.Instance.GameUI = this;
            SceneHandler.Instance.SceneLoaded += PlayGameStartAnimation;
            UpdateLevelUI();
        }

        private void OnDestroy()
        {
            SceneHandler.Instance.SceneLoaded -= PlayGameStartAnimation;
        }
        
        public void PlaySukonbuReaction(string animationName)
        {
            sukonbuAnimator.Play(animationName, 0, 0);
        }

        public void ShowPuzzleWinNotice()
        {
            puzzleWinAnimator.Play("LevelClearShow", 0, 0);
        }

        public void HidePuzzleWinNotice()
        {
            puzzleWinAnimator.Play("LevelClearHide", 0, 0);
        }

        private void PlayGameStartAnimation(string sceneName)
        {
            gameAnimator.Play("Map_Start_Game", 0, 0);
        }

        public void PlayGameEndAnimation()
        {
            gameAnimator.Play("Map_End_Game", 0, 0);
            HideCurrentLevel();
        }

        public void ShowNewPieceNotice()
        {
            newPieceNoticeAnimator.Play("NewPieceShow", 0, 0);
        }

        public void HideNewPieceNotice()
        {
            newPieceNoticeAnimator.Play("NewPieceHide", 0, 0);
        }

        public void PieceDisplayPickupAnimation()
        {
            newPieceDisplayAnimator.Play("PickedUp", 0, 0);
        }

        public void ShowCurrentLevel()
        {
            currentLevelAnimator.Play("Show", 0, 0);
        }

        private void HideCurrentLevel()
        {
            currentLevelAnimator.Play("Hide", 0, 0);
        }

        public void ToggleSettingsSlider()
        {
            if (settingsSliderIsVisible)
            {
                HideSettingsSlider();
            }
            else
            {
                ShowSettingsSlider();
            }
            settingsSliderIsVisible = !settingsSliderIsVisible;
            nejimaAnimator.Play("Press", 0, 0);
        }

        public void ToggleSettingsHover(bool hovered)
        {
            nejimaAnimator.SetBool(SettingsHovered, hovered);
        }
        
        private void ShowSettingsSlider()
        {
            settingsSlider.Play("Show");
            ToggleSettingsSliderButtons(true);
        }

        private void HideSettingsSlider()
        {
            settingsSlider.Play("Hide");
            ToggleSettingsSliderButtons(false);
        }

        private void ToggleSettingsSliderButtons(bool toggle)
        {
            foreach (Transform child in settingsSlider.transform.GetChild(0).transform)
            {
                Button childButton = child.GetComponent<Button>();
                if (childButton == null) continue;
                print(toggle);
                childButton.interactable = toggle;
            }
        }

        public void UpdateNewPieceData(PuzzlePieceList.PuzzlePieceData pieceData, LevelData levelData)
        {
            if (!levelData.IntroduceNewPiece) return;

            foreach (PuzzlePiece packPiece in pieceData.PackPieces)
            {
                if (packPiece.CurrentPieceType == levelData.PieceToIntroduce.PieceType)
                {
                    Animator pieceAnim = packPiece.gameObject.GetComponentInChildren<Animator>();
                    newPieceDisplayAnimator.runtimeAnimatorController = pieceAnim?.runtimeAnimatorController;
                    newPieceName.text = levelData.PieceToIntroduce.PieceDisplayName;
                    newPieceDescription.text = levelData.PieceToIntroduce.PieceDescription;
                    if (levelData.PieceToIntroduce.TileHolderPrefab == null) break;
                    GameObject tileUsage = Instantiate(levelData.PieceToIntroduce.TileHolderPrefab, tileHolderParent);
                    tileUsage.name = "TileHolder";
                }
            }
            ShowNewPieceNotice();
        }

        public void UpdateLevelUI()
        {
            int currentLevelInt = GameHandler.Instance.CurrentLevel + 1;
            currentLevel.text = currentLevelInt.ToString();
        }
        
        public bool IsIsomorphic(string t, string s)
        {
            // Check if either string is null or if the strings have different lengths; if so, they cannot be isomorphic.
            if (t == null || s == null || t.Length != s.Length) return false;
            
            //Dictionary to map the characters to each other
            // Hashset to track characters that have been used.
            Dictionary<char, char> map = new Dictionary<char, char>();
            HashSet<char> usedCharacters = new HashSet<char>();

            // Loop through each character in the first string.
            for (int i = 0; i < t.Length; i++) 
            {
                // Characters to track
                char characterInFirstString = t[i];
                char characterInSecondString = s[i];
                // If the current character in the first string has already been mapped to a character in the second string,
                // check if the character in the second string matches the current character. If not, the strings
                // are not isomorphic.
                if (map.TryGetValue(characterInFirstString, out char value))
                {
                    if (value != characterInSecondString) return false;
                }
                else
                {
                    // If the current character in the first string has not been tracked yet, check if the 
                    // character in the second string has already been used as a mapping for another character in the first 
                    // string. If so, the strings are not isomorphic. Otherwise, add the current map to the dictionary 
                    // and the current character to the set of used characters.
                    if (usedCharacters.Contains(characterInSecondString)) return false;
                            
                    map[characterInFirstString] = characterInSecondString;
                    usedCharacters.Add(characterInSecondString);
                }
            }
            // If the method makes it through the loop without returning false, the strings are isomorphic.
            return true;
        }
    }
}