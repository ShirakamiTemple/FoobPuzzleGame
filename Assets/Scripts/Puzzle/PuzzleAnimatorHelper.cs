using FoxHerding.Handlers;
using UnityEngine;

namespace FoxHerding.Puzzle
{
    public class PuzzleAnimatorHelper : MonoBehaviour
    {
        [SerializeField] 
        private Transform piecesTransform;
        [SerializeField]
        private Transform insidePanelTransform;
        private const int PiecesChildIndex = 3;
        private PuzzleGrid _grid;
        private bool _replayLevel;

        public void FinishLevel()
        {
            if (_grid == null)
            {
                _grid = FindObjectOfType<PuzzleGrid>();
            }
            UIHandler.Instance.GameUI.ShowPuzzleWinNotice();
        }

        public void ReplayLevel()
        {
            _replayLevel = true;
            UIHandler.Instance.GameUI.HidePuzzleWinNotice();
        }

        public void NextLevel()
        {
            _replayLevel = false;
            UIHandler.Instance.GameUI.HidePuzzleWinNotice();
        }

        public void DetermineNextStage()
        {
            if (_grid == null)
            {
                _grid = FindObjectOfType<PuzzleGrid>();
            }
            if (_replayLevel)
            {
                _grid.ReplayStage();
                return;
            }
            _grid.ProceedToNextStage();
        }

        public void SetPiecesParent()
        {
            piecesTransform.SetParent(insidePanelTransform);
        }

        public void UnsetPiecesParent()
        {
            piecesTransform.SetParent(transform);
            piecesTransform.SetSiblingIndex(PiecesChildIndex);
        }
    }
}
