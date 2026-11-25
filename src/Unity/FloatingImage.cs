using UnityEngine;

public class FloatingImage : MonoBehaviour
{
    public float moveDistance = 100f;   // 위아래 이동량(px)
    public float duration = 2f;         // 왕복 시간

    private RectTransform rect;
    private Vector2 startPos;
    private float timer;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        startPos = rect.anchoredPosition;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // 0~1 값 반복
        float t = Mathf.PingPong(timer / duration, 1f);

        // 끝에서 느려지는 곡선 — SmoothStep
        float eased = Mathf.SmoothStep(0f, 1f, t);

        float offset = Mathf.Lerp(-moveDistance, moveDistance, eased);
        rect.anchoredPosition = startPos + new Vector2(0, offset);
    }
}
