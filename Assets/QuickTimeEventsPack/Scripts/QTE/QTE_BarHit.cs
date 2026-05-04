using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QTEPack
{
    public class QTE_BarHit : QuickTimeEvent
    {
        [SerializeField] RectTransform baseArea;
        [SerializeField] RectTransform hitArea;
        [SerializeField] Image hitAreaImage;
        [SerializeField] RectTransform cursor;
        [SerializeField] private float[] HitAreaScaleByDifficulty;
        [SerializeField] private float[] CursorSpeedByDifficulty;
        [SerializeField] private TMP_Text resultText;

        private float minCursorPosition;
        private float maxCursorPosition;
        private float hitAreaLimitMin;
        private float hitAreaLimitMax;
        private QTEHitResult done;

        public override void ShowQTE(Vector2 position, float scale, int difficulty)
        {
            base.ShowQTE(position, scale, difficulty);

            float hitAreaWidth = baseArea.rect.width * HitAreaScaleByDifficulty[difficulty];
            var hitAreaHeight = hitArea.rect.height;
            hitArea.sizeDelta = new Vector2(hitAreaWidth, hitAreaHeight);
            var minHitAreaPosition = -CalculateCorner(baseArea) + CalculateCorner(hitArea) + 1;
            var maxHitAreaPosition = CalculateCorner(baseArea) - CalculateCorner(hitArea) - 1;
            hitArea.localPosition = new Vector3(Random.Range(minHitAreaPosition, maxHitAreaPosition + 1), 0, 0);
            hitAreaLimitMin = hitArea.localPosition.x - (hitArea.rect.width / 2);
            hitAreaLimitMax = hitArea.localPosition.x + (hitArea.rect.width / 2);

            hitAreaImage.color = ColorByDifficulty[difficulty];

            minCursorPosition = -CalculateCorner(baseArea) + CalculateCorner(cursor) + 1;
            maxCursorPosition = CalculateCorner(baseArea) - CalculateCorner(cursor) - 1;

            done = QTEHitResult.Playing;
            resultText.text = "";
            StartCoroutine(RunQTE(difficulty));
        }

        public IEnumerator RunQTE(int difficulty)
        {
            int direction = 1;
            float cursorX = cursor.localPosition.x;
            float speed = CursorSpeedByDifficulty[difficulty];

            while (done == QTEHitResult.Playing)
            {
                if (direction == 1)
                {
                    if (cursorX + FixedSpeed(speed) >= maxCursorPosition)
                    {
                        cursorX = maxCursorPosition;
                        direction = -1;
                    }
                    else
                    {
                        cursorX = cursorX + FixedSpeed(speed);
                    }
                }
                else //direction == -1
                {
                    if (cursorX - FixedSpeed(speed) <= minCursorPosition)
                    {
                        cursorX = minCursorPosition;
                        direction = 1;
                    }
                    else
                    {
                        cursorX = cursorX - FixedSpeed(speed);
                    }
                }

                cursor.localPosition = new Vector3(cursorX, 0, 0);

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

        private float FixedSpeed(float speed)
        {
            return speed * Time.fixedDeltaTime;
        }

        private void Update()
        {
            if (done == QTEHitResult.Playing)
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButton(0))
                {
                    if (cursor.localPosition.x <= hitAreaLimitMax && cursor.localPosition.x >= hitAreaLimitMin)
                        done = QTEHitResult.Win;
                    else
                        done = QTEHitResult.Fail;
                }
            }
        }

        private float CalculateCorner(RectTransform rectTransform)
        {
            return rectTransform.rect.width / 2;
        }

        public override void Hide()
        {
            base.Hide();
            done = QTEHitResult.Playing;
        }
    }
}