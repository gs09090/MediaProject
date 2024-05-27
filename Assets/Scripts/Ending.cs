using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ending : MonoBehaviour
{
    public GameObject talkSet;
    public TMP_Text Text;
    public GameObject Cursor;
    public AudioSource typingSound; // 타이핑 사운드
    Dictionary<int, string[]> talkData;

    int countNum = 0; // 대화 문구의 인덱스
    float typingSpeed = 0.1f; // 타자 속도 (초 단위)
    bool isTyping = false; // 타이핑 중인지 여부
    bool isComplete = false; // 문장이 완전히 출력되었는지 여부

    Coroutine typingCoroutine; // 현재 타이핑 코루틴

    string previousScene; // 이전 스테이지 이름

    public AudioSource bgmSource;
    public TMP_Text text;
    public float blinkSpeed = 1f; // 깜빡이는 속도

    void Start()
    {
        //이전 스테이지 변수
        previousScene = PlayerPrefs.GetString("PreviousScene", "No previous scene");

        talkData = new Dictionary<int, string[]>();
        GenerateData();

        // 첫 대화 시작
        //ShowText();
        Invoke("TextStart", 3);
    }

    void TextStart()
    {
        talkSet.SetActive(true);
        ShowText();
        //countNum++;
    }

    void GenerateData()
    {
        talkData.Add(0, new string[] { "..." });
        if(previousScene == "Stage6")
            talkData.Add(1, new string[] { "빨간 호리병을 이용했더니 여우누이는 불에 타 죽었다." });
        else if(previousScene == "Stage7")
            talkData.Add(1, new string[] { "노란 호리병을 이용했더니 여우누이는 가시에 찔려 죽었다." });
        else if(previousScene == "Stage8")
        {
            talkData.Add(1, new string[] { "파란 호리병을 이용했더니 여우누이는 물에 빠져 죽었다." });
            print("?");
        }
            //talkData.Add(1, new string[] { "파란 호리병을 이용했더니 여우누이는 물에 빠져 죽었다." });
        else
            talkData.Add(1, new string[] { "호리병의 요술로 여우누이가 죽었다." });
        talkData.Add(2, new string[] { "여우누이가 죽고 폐가가 된 집을 정리했다." });
        talkData.Add(3, new string[] { "추억이 많았던 집..." });
        talkData.Add(4, new string[] { "이제는 기억 속에 묻고, 나의 부인과 함께 오래도록 행복하고 싶다." });
    }

    void ShowText()
    {
        // 현재 타이핑 코루틴이 있다면 중지
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        isTyping = true;
        isComplete = false;

        // 커서 비활성화
        Cursor.SetActive(false);

        // 현재 대화 데이터 확인
        string textToShow = talkData[countNum][0];
        Text.text = "";

        // 타이핑 효과
        for (int i = 0; i < textToShow.Length; i++)
        {
            Text.text += textToShow[i];
            if (!char.IsWhiteSpace(textToShow[i])) // 현재 문자가 공백이 아닐 때만 소리 재생
            {
                typingSound.Play();
            }
            yield return new WaitForSeconds(typingSpeed);

            // 스페이스가 눌리면 타이핑 효과 중단하고 전체 문장 출력
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Text.text = textToShow;
                break;
            }
        }

        isTyping = false;
        isComplete = true;

        // 커서 활성화
        Cursor.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                // 타이핑 중에 스페이스를 누르면 전체 문장 출력
                Text.text = talkData[countNum][0];
                StopCoroutine(typingCoroutine);
                isTyping = false;
                isComplete = true;

                // 커서 활성화
                Cursor.SetActive(true);
            }
            else if (isComplete)
            {
                // 문장이 완전히 출력된 상태에서 스페이스를 누르면 다음 대화로 이동
                countNum++;
                if (talkData.ContainsKey(countNum))
                {
                    ShowText();
                }
                else
                {
                    Debug.Log("대화 종료");
                    talkSet.SetActive(false); // 모든 대화가 끝났을 때 talkSet을 비활성화
                    StartCoroutine(Blink());
                    text.text = "이야기 끝";
                }
            }
        }
    }

    void PlayBGM()
    {
        //text.SetActive(true);
        //text.text = "이야기 끝";
        bgmSource.loop = true; // BGM을 반복 재생
        bgmSource.Play();
    }

    IEnumerator Blink()
    {
        while (true)
        {
            // 텍스트의 현재 색상을 가져오기
            Color color = text.color;

            // 알파 값을 0에서 1까지 반복
            for (float t = 0; t <= 1; t += Time.deltaTime * blinkSpeed)
            {
                color.a = Mathf.Lerp(0, 1, t);
                text.color = color;
                yield return null;
            }

            // 알파 값을 1에서 0까지 반복
            for (float t = 0; t <= 1; t += Time.deltaTime * blinkSpeed)
            {
                color.a = Mathf.Lerp(1, 0, t);
                text.color = color;
                yield return null;
            }
        }
    }
}
