using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public class StoryPage
{
    [TextArea(3, 6)]
    public string text;   // 이 페이지에서 보여줄 긴 문장
}

public class StoryManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI storyText;    // 화면에 보이는 텍스트

    [Header("Typing Settings")]
    public float typingSpeed = 0.03f;    // 글자 하나 찍히는 속도 (초)

    [Header("Story Pages")]
    public StoryPage[] pages;            // 인트로 페이지들

    int currentIndex = 0;                // 현재 페이지 인덱스
    bool isTyping = false;               // 지금 타이핑 중인지
    string currentFullText = "";         // 이 페이지의 전체 문장
    Coroutine typingCoroutine;

    void Start()
    {
        ShowPage(currentIndex);
    }

    void Update()
    {
        // 마우스 왼쪽 버튼 or 스페이스 입력
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            OnClick();
        }
    }

    void OnClick()
    {
        if (isTyping)
        {
            // 타이핑 중이면 → 바로 전체 문장 보여주고 타이핑 종료
            StopCoroutine(typingCoroutine);
            storyText.text = currentFullText;
            isTyping = false;
        }
        else
        {
            // 타이핑이 끝난 상태에서 → 다음 페이지로
            currentIndex++;

            if (currentIndex < pages.Length)
            {
                ShowPage(currentIndex);
            }
            else
            {
                // 전체 스토리 끝난 이후 처리
                // 예: 메인 씬으로 이동
                SceneManager.LoadScene("GameSelect");
                Debug.Log("Story finished.");
            }
        }
    }

    void ShowPage(int index)
    {
        if (index < 0 || index >= pages.Length) return;

        currentFullText = pages[index].text;
        storyText.text = "";
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        isTyping = true;

        foreach (char c in currentFullText)
        {
            storyText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
}
