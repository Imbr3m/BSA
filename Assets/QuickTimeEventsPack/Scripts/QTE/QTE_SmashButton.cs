using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QTEPack
{
    public class QTE_SmashButton : QuickTimeEvent
    {
        [Header("QTE Settings")]
        [SerializeField] private int[] clicksToCompleteByDifficulty;
        [SerializeField] private float[] BackSpeedByDifficulty;
        [SerializeField] private float timeToLoose; //in seconds. If -1, infinite time 
        [SerializeField] private Image fillCircle;
        [SerializeField] private TMP_Text resultText;

        [Header("Food Animation")]
        [SerializeField] private Transform foodSprite; // Drag your food GameObject here!
        [SerializeField] private float bounceHeight = 0.5f; // How high it jumps. (Use bigger numbers like 20f if it's on a Canvas)
        [SerializeField] private float bounceSpeed = 0.15f; // How fast the bounce takes to finish

        private bool done;
        private int clicksCount;
        private Vector3 originalFoodPos;
        private Coroutine bounceRoutine;

        public override void ShowQTE(Vector2 position, float scale, int difficulty)
        {
            base.ShowQTE(position, scale, difficulty);

            fillCircle.color = ColorByDifficulty[difficulty];
            fillCircle.fillAmount = 0;

            done = false;
            clicksCount = 0;
            resultText.text = "";

            // Lock in the starting position the moment the QTE appears
            if (foodSprite != null)
            {
                originalFoodPos = foodSprite.localPosition;
            }

            StartCoroutine(RunQTE(difficulty));
        }

        public IEnumerator RunQTE(int difficulty)
        {
            StartCoroutine(DeathTimer());
            StartCoroutine(BackSpeedTriggerer(BackSpeedByDifficulty[difficulty]));

            while (!done)
            {
                fillCircle.fillAmount = (float)clicksCount / (float)clicksToCompleteByDifficulty[difficulty];
                done = clicksCount >= clicksToCompleteByDifficulty[difficulty];

                yield return new WaitForEndOfFrame();
            }

            StopAllCoroutines();

            if (clicksCount >= clicksToCompleteByDifficulty[difficulty])
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

        private IEnumerator DeathTimer()
        {
            if (timeToLoose > 0)
            {
                yield return new WaitForSeconds(timeToLoose);
                done = true;
            }
        }

        private IEnumerator BackSpeedTriggerer(float speed)
        {
            yield return new WaitForSeconds(speed);
            while (!done)
            {
                clicksCount = Math.Max(0, clicksCount - 1);
                yield return new WaitForSeconds(speed);
            }
        }

        private void Update()
        {
            if (!done)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    clicksCount++;

                    // Trigger the bounce animation every time they press space!
                    if (foodSprite != null)
                    {
                        // Stop the current bounce if they spam the button before it finishes
                        if (bounceRoutine != null) StopCoroutine(bounceRoutine);
                        bounceRoutine = StartCoroutine(FoodBounce());
                    }
                }
            }
        }

        private IEnumerator FoodBounce()
        {
            float timer = 0f;
            while (timer < bounceSpeed)
            {
                timer += Time.deltaTime;
                
                // This creates a perfect, smooth arc that always returns to 0
                float heightCurve = Mathf.Sin((timer / bounceSpeed) * Mathf.PI);
                
                foodSprite.localPosition = originalFoodPos + new Vector3(0, bounceHeight * heightCurve, 0);
                yield return null;
            }
            
            // Lock it perfectly back into place at the end
            foodSprite.localPosition = originalFoodPos;
        }

        public override void Hide()
        {
            base.Hide();
            // Reset position fully when the QTE ends
            if (foodSprite != null)
            {
                foodSprite.localPosition = originalFoodPos;
            }
        }
    }
}