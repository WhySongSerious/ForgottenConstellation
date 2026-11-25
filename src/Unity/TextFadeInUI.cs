using UnityEngine;
using UnityEngine.UI;

public class TextFadeInUI : MonoBehaviour
{
    public float duration = 1f;

    private Text text;
    private float timer = 0f;

    void Start()
    {
        text = GetComponent<Text>();
        Color c = text.color;
        c.a = 0f;
        text.color = c;
    }

    void Update()
    {
        timer += Time.deltaTime;

        float alpha = Mathf.Clamp01(timer / duration);

        Color c = text.color;
        c.a = alpha;
        text.color = c;

        if (alpha >= 1f)
            enabled = false;
    }
}