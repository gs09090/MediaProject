using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public TMP_Text text;
    public float blinkSpeed = 1f; // �����̴� �ӵ�
    public AudioSource bgmSource; // AudioSource ������Ʈ

    void Start()
    {
        // BGM ���
        PlayBGM();

        // �ؽ�Ʈ ������ �ڷ�ƾ ����
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
        bgmSource.loop = true; // BGM�� �ݺ� ���
        bgmSource.Play();
    }

    IEnumerator Blink()
    {
        while (true)
        {
            // �ؽ�Ʈ�� ���� ������ ��������
            Color color = text.color;

            // ���� ���� 0���� 1���� �ݺ�
            for (float t = 0; t <= 1; t += Time.deltaTime * blinkSpeed)
            {
                color.a = Mathf.Lerp(0, 1, t);
                text.color = color;
                yield return null;
            }

            // ���� ���� 1���� 0���� �ݺ�
            for (float t = 0; t <= 1; t += Time.deltaTime * blinkSpeed)
            {
                color.a = Mathf.Lerp(1, 0, t);
                text.color = color;
                yield return null;
            }
        }
    }
}
