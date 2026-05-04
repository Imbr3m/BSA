using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QTEPack
{
    public class QTE_Decision : QuickTimeEvent
    {
        [SerializeField] private float[] TimeByDifficulty;
        [SerializeField] private TMP_Text[] optionsTexts;
        [SerializeField] private Image timeFillArea;
        [SerializeField] private TMP_Text resultText;

        private string[] availableOptions;
        private string selectedOption = "";

        protected override void Initialize()
        {
            base.Initialize();
        }

        public void ShowQTE(Vector2 position, float scale, int difficulty, string[] options)
        {
            base.ShowQTE(position, scale, difficulty);

            availableOptions = options;

            timeFillArea.color = ColorByDifficulty[difficulty];

            optionsTexts[0].text = availableOptions[0];
            optionsTexts[1].text = availableOptions[1];
            if (availableOptions.Length > 2)
            {
                optionsTexts[2].transform.parent.gameObject.SetActive(true);
                optionsTexts[2].text = availableOptions[2];
            }
            else
            {
                optionsTexts[2].transform.parent.gameObject.SetActive(false);
            }

            selectedOption = "";
            resultText.text = "";
            StartCoroutine(RunQTE(difficulty));
        }

        public IEnumerator RunQTE(int difficulty)
        {
            float timeByDifficulty = TimeByDifficulty[difficulty];

            float elapsedTime = 0;

            while (elapsedTime < timeByDifficulty && selectedOption == "")
            {
                elapsedTime += Time.fixedDeltaTime;

                timeFillArea.fillAmount = elapsedTime / timeByDifficulty;

                yield return new WaitForEndOfFrame();
            }

            if (selectedOption == "")
                SelectRandomOption();

            resultText.text = "Your choice: " + selectedOption;

            OnSuccess.Invoke();
        }

        private void SelectRandomOption()
        {
            selectedOption = availableOptions[UnityEngine.Random.Range(0, availableOptions.Length)] + " (Randomly selected)";
        }

        public void OnOptionClick(int selectedOptionId)
        {
            selectedOption = availableOptions[selectedOptionId];
        }
    }
}