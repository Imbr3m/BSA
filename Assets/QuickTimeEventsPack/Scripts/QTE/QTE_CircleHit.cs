using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QTEPack
{
    public class QTE_CircleHit : QuickTimeEvent
    {
        [Header("DBD Style Settings")]
        [Tooltip("How many consecutive hits are needed to pass the QTE?")]
        [SerializeField] private int requiredHitsToWin = 3; 

        [Header("Original Settings")]
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

        // NEW: Variables to track our DBD mechanics
        private int currentDifficulty;
        private int currentSuccessfulHits;
        private float moveDirection; 

        public override void ShowQTE(Vector2 position, float scale, int difficulty)
        {
            base.ShowQTE(position, scale, difficulty);

            // Save difficulty to generate new hit areas later
            currentDifficulty = difficulty;
            
            // Reset our hit tracker and set direction to clockwise (1f)
            currentSuccessfulHits = 0;
            moveDirection = 1f; 

            // Generate the very first hit area
            GenerateNewHitArea();

            done = QTEHitResult.Playing;
            resultText.text = "";
            StartCoroutine(RunQTE(difficulty));
        }

        // NEW: A helper function that moves the target safely without ending the game
        private void GenerateNewHitArea()
        {
            hitAreaImage.fillAmount = HitAreaScaleByDifficulty[currentDifficulty];
            
            // Randomize the new location on the circle
            hitAreaRotationZ = Random.Range(0, 360);
            hitAreaImage.rectTransform.rotation = Quaternion.Euler(0, 0, hitAreaRotationZ);

            hitAreaImage.color = ColorByDifficulty[currentDifficulty];

            var allowedErrorByDifficulty = -350f * HitAreaScaleByDifficulty[currentDifficulty] + 7.5f;
            minCursorRotToWin = hitAreaRotationZ + allowedErrorByDifficulty;
            maxCursorRotToWin = hitAreaRotationZ + 10;
        }

        public IEnumerator RunQTE(int difficulty)
        {
            float speed = CursorSpeedByDifficulty[difficulty];
            cursorRotationZ = 0;

            while (done == QTEHitResult.Playing)
            {
                // NEW: We multiply the speed by our moveDirection so it can spin backwards!
                cursorRotationZ -= FixedSpeed(speed) * moveDirection;

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
                // FIXED: Changed to GetMouseButtonDown so holding the button doesn't cheat the system
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) 
                {
                    var normalizedCursorZ = cursorRotationZ % 360;

                    var checkA = normalizedCursorZ >= System.Math.Min(minCursorRotToWin, maxCursorRotToWin) && normalizedCursorZ <= System.Math.Max(minCursorRotToWin, maxCursorRotToWin);
                    var checkB = (360 + normalizedCursorZ) >= System.Math.Min(minCursorRotToWin, maxCursorRotToWin) && (360 + normalizedCursorZ) <= System.Math.Max(minCursorRotToWin, maxCursorRotToWin);

                    if (checkA || checkB)
                    {
                        // They hit the sweet spot! 
                        currentSuccessfulHits++;
                        
                        if (currentSuccessfulHits >= requiredHitsToWin)
                        {
                            // If they reached the required amount of hits, they win.
                            done = QTEHitResult.Win;
                        }
                        else
                        {
                            // REVERSE AND RELOCATE!
                            moveDirection *= -1f; // Flips between 1 and -1
                            GenerateNewHitArea();
                        }
                    }
                    else
                    {
                        // They missed! Instant fail.
                        done = QTEHitResult.Fail;
                    }
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