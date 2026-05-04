using QTEPack;
using UnityEngine;

public class DecisionQTEDemo : QTEDemoRunner
{
    protected override void Test()
    {
        if (Random.Range(0, 2) == 0)
            (QTE as QTE_Decision).ShowQTE(Vector2.zero, 1, Random.Range(0, 5), new string[] { "Run", "Hide" });
        else
            (QTE as QTE_Decision).ShowQTE(Vector2.zero, 1, Random.Range(0, 5), new string[] { "Run", "Hide", "Fight" });
    }
}
