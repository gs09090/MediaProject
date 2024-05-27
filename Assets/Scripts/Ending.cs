using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ending : MonoBehaviour
{
    public GameObject talkSet;
    public TMP_Text Text;
    public GameObject Cursor;
    public AudioSource typingSound; // Ÿ���� ����
    Dictionary<int, string[]> talkData;

    int countNum = 0; // ��ȭ ������ �ε���
    float typingSpeed = 0.1f; // Ÿ�� �ӵ� (�� ����)
    bool isTyping = false; // Ÿ���� ������ ����
    bool isComplete = false; // ������ ������ ��µǾ����� ����

    Coroutine typingCoroutine; // ���� Ÿ���� �ڷ�ƾ

    string previousScene; // ���� �������� �̸�

    public AudioSource bgmSource;
    public TMP_Text text;
    public float blinkSpeed = 1f; // �����̴� �ӵ�

    void Start()
    {
        //���� �������� ����
        previousScene = PlayerPrefs.GetString("PreviousScene", "No previous scene");

        talkData = new Dictionary<int, string[]>();
        GenerateData();

        // ù ��ȭ ����
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
            talkData.Add(1, new string[] { "���� ȣ������ �̿��ߴ��� ���촩�̴� �ҿ� Ÿ �׾���." });
        else if(previousScene == "Stage7")
            talkData.Add(1, new string[] { "��� ȣ������ �̿��ߴ��� ���촩�̴� ���ÿ� ��� �׾���." });
        else if(previousScene == "Stage8")
        {
            talkData.Add(1, new string[] { "�Ķ� ȣ������ �̿��ߴ��� ���촩�̴� ���� ���� �׾���." });
            print("?");
        }
            //talkData.Add(1, new string[] { "�Ķ� ȣ������ �̿��ߴ��� ���촩�̴� ���� ���� �׾���." });
        else
            talkData.Add(1, new string[] { "ȣ������ ����� ���촩�̰� �׾���." });
        talkData.Add(2, new string[] { "���촩�̰� �װ� �󰡰� �� ���� �����ߴ�." });
        talkData.Add(3, new string[] { "�߾��� ���Ҵ� ��..." });
        talkData.Add(4, new string[] { "������ ��� �ӿ� ����, ���� ���ΰ� �Բ� �������� �ູ�ϰ� �ʹ�." });
    }

    void ShowText()
    {
        // ���� Ÿ���� �ڷ�ƾ�� �ִٸ� ����
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

        // Ŀ�� ��Ȱ��ȭ
        Cursor.SetActive(false);

        // ���� ��ȭ ������ Ȯ��
        string textToShow = talkData[countNum][0];
        Text.text = "";

        // Ÿ���� ȿ��
        for (int i = 0; i < textToShow.Length; i++)
        {
            Text.text += textToShow[i];
            if (!char.IsWhiteSpace(textToShow[i])) // ���� ���ڰ� ������ �ƴ� ���� �Ҹ� ���
            {
                typingSound.Play();
            }
            yield return new WaitForSeconds(typingSpeed);

            // �����̽��� ������ Ÿ���� ȿ�� �ߴ��ϰ� ��ü ���� ���
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Text.text = textToShow;
                break;
            }
        }

        isTyping = false;
        isComplete = true;

        // Ŀ�� Ȱ��ȭ
        Cursor.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                // Ÿ���� �߿� �����̽��� ������ ��ü ���� ���
                Text.text = talkData[countNum][0];
                StopCoroutine(typingCoroutine);
                isTyping = false;
                isComplete = true;

                // Ŀ�� Ȱ��ȭ
                Cursor.SetActive(true);
            }
            else if (isComplete)
            {
                // ������ ������ ��µ� ���¿��� �����̽��� ������ ���� ��ȭ�� �̵�
                countNum++;
                if (talkData.ContainsKey(countNum))
                {
                    ShowText();
                }
                else
                {
                    Debug.Log("��ȭ ����");
                    talkSet.SetActive(false); // ��� ��ȭ�� ������ �� talkSet�� ��Ȱ��ȭ
                    StartCoroutine(Blink());
                    text.text = "�̾߱� ��";
                }
            }
        }
    }

    void PlayBGM()
    {
        //text.SetActive(true);
        //text.text = "�̾߱� ��";
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
