using UnityEngine;
using UnityEngine.UI;

public class ButtonFadeIn : MonoBehaviour
{
    public float duration = 1f;   // 페이드인 시간

    private Graphic[] graphics;   // 버튼 안의 모든 Graphic(Text, Image 등)
    private float timer = 0f;

    void Start()
    {
        // Button + 자식들의 모든 Graphic 가져오기
        graphics = GetComponentsInChildren<Graphic>(true);

        // 전부 투명하게 설정
        foreach (var g in graphics)
        {
            Color c = g.color;
            c.a = 0f;
            g.color = c;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        float alpha = Mathf.Clamp01(timer / duration);

        foreach (var g in graphics)
        {
            Color c = g.color;
            c.a = alpha;
            g.color = c;
        }

        if (alpha >= 1f)
            enabled = false; // 완료되면 Update 종료
    }
}
