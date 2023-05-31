//***
// Author: Nate
// Description: GameHandler.cs handles most of the game information such as pieces, levels and packs
//***

using System.Collections.Generic;
using FoxHerding.Generics;
using FoxHerding.Levels;
using FoxHerding.Puzzle.Pieces;
using UnityEngine;

namespace FoxHerding.Handlers
{
    public class GameHandler : Handler<GameHandler>
    {
        [field: SerializeField]
        public List<LevelData> AvailableLevels { get; private set; }
        [field: SerializeField]
        public PuzzlePieceList AvailablePieces { get; private set; }
        public enum PuzzlePack { DAN_PACK, BORIZ_PACK, KINOKO_PACK }
        public int CurrentLevel { get; set; }
        public PuzzlePack CurrentPack { get; set; }
        public delegate void OnPackChange();
        public OnPackChange PackChanged;
        [SerializeField]
        private bool skipLoad;

        protected override void Awake()
        {
            base.Awake();
            SaveLoadHandler.Instance.Load += AssignGameValues;
            SaveLoadHandler.Instance.Save += SaveGameValues;
        }

        private void AssignGameValues()
        {
            if (skipLoad) return;
            
            CurrentLevel = PlayerPrefs.GetInt("CurrentLevel");
            ChangePack(PlayerPrefs.GetInt("CurrentPack"));
        }

        private void SaveGameValues()
        {
            PlayerPrefs.SetInt("CurrentLevel", CurrentLevel);
            PlayerPrefs.SetInt("CurrentPack", (int)CurrentPack);
        }

        private void OnDestroy()
        {
            SaveLoadHandler.Instance.Load -= AssignGameValues;
            SaveLoadHandler.Instance.Save -= SaveGameValues;
        }
    
        /// <summary>
        /// ChangePack accepts an int and converted to the PuzzlePack enum. Updates the currently used pack.
        /// </summary>
        /// <param name="newPack"></param>
        public void ChangePack(int newPack)
        {
            CurrentPack = (PuzzlePack)newPack;
            PackChanged?.Invoke();
        }
    }
}
