using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class FeedbackClient : MonoBehaviour
{
    [Header("서버 주소 (Flask 서버)")]
    public string serverUrl = "http://127.0.0.1:5000/feedback";

    [System.Serializable]
    private class FeedbackWrapper
    {
        public string message;
    }

    void Start()
    {
        Debug.Log(" FeedbackClient.Start() 실행됨 — serverUrl = " + serverUrl);
    }

    public IEnumerator SendSnapshotAndGetFeedback(FocusSnapshot snapshot, System.Action<string> onResult)
    {
        // 스냅샷 -> JSON 변환
        string json = JsonUtility.ToJson(snapshot);
        byte[] body = System.Text.Encoding.UTF8.GetBytes(json);

        Debug.Log(" 서버로 피드백 요청 전송: " + serverUrl);
        Debug.Log(" 보낸 JSON: " + json);

        using (UnityWebRequest www = new UnityWebRequest(serverUrl, "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(body);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(" Feedback error: " + www.error);
                onResult?.Invoke(null);
            }
            else
            {
                Debug.Log(" 서버 응답 RAW: " + www.downloadHandler.text);

                FeedbackWrapper wrapper = JsonUtility.FromJson<FeedbackWrapper>(www.downloadHandler.text);
                onResult?.Invoke(wrapper.message);
            }
        }
    }
}
