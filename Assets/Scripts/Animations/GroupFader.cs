using UnityEngine;
using TMPro; 

[ExecuteAlways] 
public class GroupFader : MonoBehaviour
{
    [Range(0f, 1f)] 
    [Tooltip("Slide this to fade everything inside this object!")]
    public float masterOpacity = 1f;

    private SpriteRenderer[] sprites;
    private TMP_Text[] texts;

    void Update()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>();
        texts = GetComponentsInChildren<TMP_Text>();

        foreach (SpriteRenderer sr in sprites) 
        {
            Color c = sr.color;
            c.a = masterOpacity;
            sr.color = c;
        }

        foreach (TMP_Text txt in texts) 
        {
            Color c = txt.color;
            c.a = masterOpacity;
            txt.color = c;
        }
    }
}