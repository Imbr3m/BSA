using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QTEPack
{
    public class QTE_CircleHit : QuickTimeEvent
    {
        [SerializeField] private float[] HitAreaScaleByDifficulty;
        [SerializeField] private float[] CursorSpeedByDifficulty;
        [SerializeField] private Image hitAreaImage;
        [SerializeField] private TMP_Text resultText;
        [SerializeField] private RectTransform cursor;

        private float hitAreaRotationZ;
        private float cursorRotationZ;
        private float minCursorRotToWin;
        private float maxCursorRotToWin;
        private QTEHitResult done;

        public override void ShowQTE(Vector2 position, float scale, int difficulty)
        {
            base.ShowQTE(position, scale, difficulty);

            hitAreaImage.fillAmount = HitAreaScaleByDifficulty[difficulty];
            hitAreaRotationZ = Random.Range(0, 360);
            hitAreaImage.rectTransform.rotation = Quaternion.Euler(0, 0, hitAreaRotationZ);

            hitAreaImage.color = ColorByDifficulty[difficulty];

            var allowedErrorByDifficulty = -350f * HitAreaScaleByDifficulty[difficulty] + 7.5f;
            minCursorRotToWin = hitAreaRotationZ + allowedErrorByDifficulty;
            maxCursorRotToWin = hitAreaRotationZ + 10;

            done = QTEHitResult.Playing;
            resultText.text = "";
            StartCoroutine(RunQTE(difficulty));
        }

        public IEnumerator RunQTE(int difficulty)
        {
            float speed = CursorSpeedByDifficulty[difficulty];
            cursorRotationZ = 0;

            while (done == QTEHitResult.Playing)
            {
                cursorRotationZ -= FixedSpeed(speed);

                cursor.rotation = Quaternion.Euler(0, 0, cursorRotationZ);

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
                    var normalizedCursorZ = cursorRotationZ % 360;

                    var checkA = normalizedCursorZ >= System.Math.Min(minCursorRotToWin, maxCursorRotToWin) && normalizedCursorZ <= System.Math.Max(minCursorRotToWin, maxCursorRotToWin);
                    var checkB = (360 + normalizedCursorZ) >= System.Math.Min(minCursorRotToWin, maxCursorRotToWin) && (360 + normalizedCursorZ) <= System.Math.Max(minCursorRotToWin, maxCursorRotToWin);

                    if (checkA || checkB)
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