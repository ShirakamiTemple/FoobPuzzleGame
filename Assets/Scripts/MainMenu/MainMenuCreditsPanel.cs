using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FoxHerding.MainMenu
{
    public class MainMenuCreditsPanel : MonoBehaviour
    {

        [Header("CreditsData")]
        [SerializeField] List<CreditTemplate> creditsList;

        [Header("Credits UI")]
        [SerializeField] TMP_Text nameText;
        [SerializeField] Image iconImage;
        [SerializeField] TMP_Text noteText;
        [SerializeField] TMP_Text functionText;

        private int currentListPosition = 0;

        void Start()
        {
            if (creditsList.Count != 0) UpdateCreditsUI(0);
        }

        private void UpdateCreditsUI(int listPointer)
        {
            if (creditsList[listPointer] != null)
            {
                nameText.text = creditsList[listPointer].creditName;
                iconImage.sprite = creditsList[listPointer].creditSprite;
                noteText.text = creditsList[listPointer].creditNote;
                functionText.text = creditsList[listPointer].creditFunction;
            }
        }

        public void ShowNextCredit()
        {
            currentListPosition++;

            if(currentListPosition > creditsList.Count - 1)
            {
                currentListPosition = 0;
            }

            UpdateCreditsUI(currentListPosition);
        }

        public void ShowPreviousCredit()
        {
            currentListPosition--;

            if (currentListPosition < 0)
            {
                currentListPosition = creditsList.Count - 1;
            }

            UpdateCreditsUI(currentListPosition);
        }
    }
}
