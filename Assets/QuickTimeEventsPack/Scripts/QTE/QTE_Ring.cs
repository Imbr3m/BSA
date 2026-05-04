using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QTEPack
{
    public class QTE_Ring : QuickTimeEvent
    {
        [SerializeField] private float[] RingSpeedByDifficulty;
        [SerializeField] private Image ringImage;
        [SerializeField] private TMP_Text resultText;
        private QTEHitResult done;
        private float currentRingScale;

        public override void ShowQTE(Vector2 position, float scale, int difficulty)
        {
            base.ShowQTE(position, scale, difficulty);

            ringImage.color = ColorByDifficulty[difficulty];

            done = QTEHitResult.Playing;
            resultText.text = "";
            StartCoroutine(RunQTE(difficulty));
        }

        public IEnumerator RunQTE(int difficulty)
        {
            float speed = RingSpeedByDifficulty[difficulty];

            currentRingScale = 0;
            float ringAlpha = 1;

            while (done == QTEHitResult.Playing)
            {
                currentRingScale += FixedSpeed(speed);
                if (currentRingScale > 1.1f)
                {
                    ringAlpha = .5f - (currentRingScale - 1f);
                    if (currentRingScale >= 1.5f)
                    {
                        currentRingScale = 0;
                        ringAlpha = 1;
                    }
                }

                var currentColor = ringImage.color;
                currentColor.a = ringAlpha;
                ringImage.color = currentColor;

                ringImage.transform.localScale = new Vector3(currentRingScale, currentRingScale, currentRingScale);

                yield return new WaitForEndOfFrame();
            }

            if (done == QTEHitResult.Win)
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
            if (done == QTEHitResult.Playing)
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButton(0))
                {
                    if (currentRingScale >= 0.9f && currentRingScale <= 1.1f)
                    {
                        done = QTEHitResult.Win;
                    }
                    else
                        done = QTEHitResult.Fail;
                }
            }
        }

        private float FixedSpeed(float speed)
        {
            return speed * Time.fixedDeltaTime;
        }
        public override void Hide()
        {
            base.Hide();
            done = QTEHitResult.Playing;
        }
    }
}