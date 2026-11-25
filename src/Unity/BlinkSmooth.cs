using UnityEngine;
using UnityEngine.UI;

public class BlinkSmooth : MonoBehaviour
{
    public float speed;

    private Text textUI;

    void Start()
    {
        textUI = GetComponent<Text>();
    }

    void Update()
    {
        float alpha = Mathf.PingPong(Time.time * speed, 1f);

        Color c = textUI.color;
        c.a = alpha;
        textUI.color = c;
    }
}
