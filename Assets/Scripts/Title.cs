using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public TMP_Text text;
    public float blinkSpeed = 1f; // 깜빡이는 속도
    public AudioSource bgmSource; // AudioSource 컴포넌트

    void Start()
    {
        // BGM 재생
        PlayBGM();

        // 텍스트 깜빡임 코루틴 시작
        StartCoroutine(Blink());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Tutorial");
        }
    }

    void PlayBGM()
    {
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
