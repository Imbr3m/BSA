using QTEPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEDemoRunner : MonoBehaviour
{
    [SerializeField] public QuickTimeEvent QTE;

    public void Show()
    {
        QTE.OnFail.AddListener(OnFinishQTE);
        QTE.OnSuccess.AddListener(OnFinishQTE);

        QTE.ShowQTE(new Vector2(200f, 200f), 1, 2);

    }

    public void Hide()
    {
        QTE.OnFail.RemoveAllListeners();
        QTE.OnSuccess.RemoveAllListeners();

        QTE.Hide();
        StopAllCoroutines();
    }

    private void OnFinishQTE()
    {
        StartCoroutine(WaitASecondAndGoAgain());
    }

    private IEnumerator WaitASecondAndGoAgain()
    {
        yield return new WaitForSeconds(1);
        Test();
    }

    protected virtual void Test()
    {
        QTE.ShowQTE(new Vector2(Random.Range(-500f, 501f), Random.Range(-500f, 501f)), Random.Range(1f, 3f), Random.Range(0, 5));
    }
}
