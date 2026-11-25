from flask import Flask, request, jsonify
import requests
import os

app = Flask(__name__)

# 여기에 제미나이 API 키 넣기 (연습용)
GEMINI_API_KEY = os.environ.get("GEMINI_API_KEY", "")
MODEL_NAME = "gemini-2.5-flash"
GEMINI_URL = f"https://generativelanguage.googleapis.com/v1beta/models/{MODEL_NAME}:generateContent"


def build_prompt(data):
    last_result = "성공" if data.get("lastSuccess") else "실패"

    prompt = f"""
당신은 ADHD를 가진 우주선 파일럿 Nova를 옆에서 돕는 AI 동료 Luna입니다.
게임은 Nova가 번쩍이는 타겟 신호를 보고 빠르게 반응하는 미니게임입니다.
아래는 Nova의 최근 플레이 상태입니다.

- 현재 라운드 번호: {data.get("roundIndex")}
- 최근 성공률: {data.get("successRate"):.2f}
- 평균 반응시간: {data.get("avgReactionTime"):.3f} 초
- 현재 난이도(타겟 유지 시간): {data.get("targetLifeTime"):.3f} 초
- 연속 성공 횟수: {data.get("streakSuccess")}
- 연속 실패 횟수: {data.get("streakFail")}
- 방금 라운드 결과: {last_result}

이 정보를 바탕으로, Nova의 "현재 집중 상태"를 아주 짧게 해석하고,
Luna의 말투로 따뜻하게 한 문단으로 코멘트해주세요.

규칙:
- 한국어로 답변합니다.
- 2~3문장 이내로 짧게 말합니다.
- 평가나 혼내는 표현은 사용하지 않습니다.
- 대신 "지금 어떤 상태인지"를 인정해 주고, "다음 라운드에서 시도해볼 작은 팁"을 하나 알려줍니다.
- 캐릭터 이름(Nova, Luna)을 자연스럽게 섞어주세요.
- 이모티콘은 최대 1개까지만 사용합니다.
"""
    return prompt


def call_gemini(prompt: str) -> str:
    headers = {"Content-Type": "application/json"}
    params = {"key": GEMINI_API_KEY}
    payload = {
        "contents": [
            {"parts": [{"text": prompt}]}
        ]
    }
    resp = requests.post(GEMINI_URL, headers=headers, params=params, json=payload)
    resp.raise_for_status()
    resp_json = resp.json()
    text = resp_json["candidates"][0]["content"]["parts"][0]["text"]
    return text


@app.route("/feedback", methods=["POST"])
def feedback():
    data = request.get_json()

    print("=== Unity에서 받은 데이터 ===")
    print(data)

    try:
        prompt = build_prompt(data)
        message = call_gemini(prompt)
    except Exception as e:
        print("Gemini 호출 중 에러:", e)
        message = "지금은 루나가 신호를 받지 못했어요. 잠시 후 다시 시도해볼까요?"

    return jsonify({"message": message})


if __name__ == "__main__":
    app.run(host="0.0.0.0", port=5000, debug=True)
