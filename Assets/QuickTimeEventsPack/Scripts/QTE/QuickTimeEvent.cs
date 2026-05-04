using UnityEngine;
using UnityEngine.Events;

namespace QTEPack
{
    enum QTEHitResult
    {
        Playing, Win, Fail
    }

    public class QuickTimeEvent : MonoBehaviour
    {
        [SerializeField] private Transform gameArea;
        [SerializeField] public Color[] ColorByDifficulty;

        public UnityEvent OnFail;
        public UnityEvent OnSuccess;

        private bool initialized = false;

        public virtual void Hide()
        {
            StopAllCoroutines();
            gameObject.SetActive(false);
        }

        protected virtual void Initialize()
        {
            initialized = true;
        }

        public virtual void ShowQTE(Vector2 position, float scale, int difficulty)
        {
            if (!initialized)
                Initialize();

            gameObject.SetActive(true);

            gameArea.localPosition = position;
            gameArea.localScale = new Vector3(scale, scale, scale);
        }
    }
}