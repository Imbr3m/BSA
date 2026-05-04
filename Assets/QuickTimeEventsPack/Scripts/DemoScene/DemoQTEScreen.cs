using TMPro;
using UnityEngine;

public class DemoQTEScreen : MonoBehaviour
{
    [SerializeField] private QTEDemoRunner qtePushKeyDemo;
    [SerializeField] private QTEDemoRunner qteBarHitDemo;
    [SerializeField] private QTEDemoRunner qteCircleHitDemo;
    [SerializeField] private QTEDemoRunner qteDecisionDemo;
    [SerializeField] private QTEDemoRunner qteSmashDemo;
    [SerializeField] private QTEDemoRunner qteRingDemo;

    [SerializeField] private TMP_Text instructionsText;
    [SerializeField] protected TMP_Text[] ColorReferences;

    private string current = "";

    public void Start()
    {
        instructionsText.text = "";
        HideAll();

        OnSelectQTEDemo("pushKey");
    }

    private void HideAll()
    {
        qtePushKeyDemo.Hide();
        qteBarHitDemo.Hide();
        qteCircleHitDemo.Hide();
        qteDecisionDemo.Hide();
        qteSmashDemo.Hide();
        qteRingDemo.Hide();
    }

    public void OnSelectQTEDemo(string model)
    {
        if (current == model)
            return;

        HideAll();

        QTEDemoRunner currentType = null;
        instructionsText.text = "";

        switch (model)
        {
            case "pushKey":
                currentType = qtePushKeyDemo;
                instructionsText.text = "Push Random Key QTE:\nPush the key or mouse button it tells you before the time runs out.\n\nColor references: ";
                break;
            case "barHit":
                currentType = qteBarHitDemo;
                instructionsText.text = "Hit Area Bar QTE:\nUse Space or Left Mouse Click to stop the green ball when it is going through the highligted area.\nColor references: ";
                break;
            case "circleHit":
                currentType = qteCircleHitDemo;
                instructionsText.text = "Hit Area Circle QTE:\nUse Space or Left Mouse Click to stop the cursor when it is going through the highligted area.\nColor references:";
                break;
            case "smash":
                currentType = qteSmashDemo;
                instructionsText.text = "Smash Button QTE:\nSmash Space key really quick until the colored circle is complete\n\nColor references: ";
                break;
            case "ring":
                currentType = qteRingDemo;
                instructionsText.text = "Ring QTE:\nUse Space or Left Mouse Click to stop the inner ring when it is touching the outer ring.\n\nColor references:";
                break;
            case "decision":
                currentType = qteDecisionDemo;
                instructionsText.text = "Decision QTE:\nChoose an option before the times runs out or it will choose an option randomly.\n\nColor references:";
                break;
        }

        if (currentType != null)
        {
            currentType.Show();

            for (int i = 0; i < ColorReferences.Length; i++)
            {
                ColorReferences[i].color = currentType.QTE.ColorByDifficulty[i];
            }
        }
    }
}
