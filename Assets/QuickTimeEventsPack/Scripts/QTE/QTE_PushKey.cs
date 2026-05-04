using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace QTEPack
{
    public class QTE_PushKey : QuickTimeEvent
    {
        [SerializeField] private List<QTEEVent>[] KeysByDifficulty;
        [SerializeField] private float[] TimeByDifficulty;
        [SerializeField] private Image fillCircle;
        [SerializeField] private TMP_Text keyText;
        [SerializeField] private TMP_Text resultText;

        private QTEEVent keyToPush;
        private bool done;

        protected override void Initialize()
        {
            base.Initialize();

            KeysByDifficulty = new List<QTEEVent>[5];
            KeysByDifficulty[0] = new List<QTEEVent> { new QTEEvent_Key(KeyCode.A), new QTEEvent_Key(KeyCode.D), new QTEEvent_MouseLeftClick() };
            KeysByDifficulty[1] = new List<QTEEVent> { new QTEEvent_Key(KeyCode.A), new QTEEvent_Key(KeyCode.D), new QTEEvent_MouseLeftClick() };
            KeysByDifficulty[2] = new List<QTEEVent> { new QTEEvent_Key(KeyCode.W), new QTEEvent_Key(KeyCode.A), new QTEEvent_Key(KeyCode.S), new QTEEvent_Key(KeyCode.D), new QTEEvent_MouseLeftClick() };
            KeysByDifficulty[3] = new List<QTEEVent> { new QTEEvent_Key(KeyCode.W), new QTEEvent_Key(KeyCode.A), new QTEEvent_Key(KeyCode.S), new QTEEvent_Key(KeyCode.D), new QTEEvent_MouseLeftClick(), new QTEEvent_MouseRightClick() };
            KeysByDifficulty[4] = new List<QTEEVent> { new QTEEvent_Key(KeyCode.W), new QTEEvent_Key(KeyCode.A), new QTEEvent_Key(KeyCode.S), new QTEEvent_Key(KeyCode.D), new QTEEvent_MouseLeftClick(), new QTEEvent_MouseRightClick() };
        }

        public override void ShowQTE(Vector2 position, float scale, int difficulty)
        {
            base.ShowQTE(position, scale, difficulty);

            keyToPush = KeysByDifficulty[difficulty][Random.Range(0, KeysByDifficulty[difficulty].Count)];
            keyText.text = keyToPush.Text();

            fillCircle.color = ColorByDifficulty[difficulty];
            fillCircle.fillAmount = 0;

            done = false;
            resultText.text = "";
            StartCoroutine(RunQTE(difficulty));
        }

        public IEnumerator RunQTE(int difficulty)
        {
            float timeByDifficulty = TimeByDifficulty[difficulty];

            float elapsedTime = 0;

            while (elapsedTime < timeByDifficulty && !done)
            {
                elapsedTime += Time.fixedDeltaTime;

                fillCircle.fillAmount = elapsedTime / timeByDifficulty;

                yield return new WaitForEndOfFrame();
            }

            if (done)
            {
                resultText.text = "Success!!!";
                OnSuccess.Invoke();
            }
            else
            {
                resultText.text = "Ups...";
                OnFail.Invoke();
            }
        }

        private void Update()
        {
            if (!done)
                done = keyToPush.CheckIfDone();
        }

        public override void Hide()
        {
            base.Hide();
            done = false;
        }
    }
}