using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int health;
    public PlayerMove player;
    [SerializeField] public GameObject[] Stages;
    public int stageNum = 1;

    //UI�� ���� �������� ���� (�̹����� �迭)
    public Image[] UIhealth;
    public TextMeshProUGUI UIPoint;
    public TextMeshProUGUI UIStage;
    public GameObject UIRestartBtn;

    public GameObject talkSet;
    public TMP_Text Text;
    public GameObject Cursor;
    public AudioSource typingSound; // Ÿ���� ����
    Dictionary<int, string[]> talkData;

    public int countNum = 0; // ��ȭ ������ �ε���
    float typingSpeed = 0.1f; // Ÿ�� �ӵ� (�� ����)
    bool isTyping = false; // Ÿ���� ������ ����
    bool isComplete = false; // ������ ������ ��µǾ����� ����
    Coroutine typingCoroutine; // ���� Ÿ���� �ڷ�ƾ

    //public AudioSource bgmSource;

    void Start()
    {
        PlayBGM();
        talkData = new Dictionary<int, string[]>();
        GenerateData();
        ShowText();
    }

    void PlayBGM()
    {
        //bgmSource.loop = true; // BGM�� �ݺ� ���
        //bgmSource.Play();
    }

    void Update()
    {
        UIPoint.text = (totalPoint + stagePoint).ToString();
        if(stageNum == 1)
        {
            UIStage.text = "STAGE " + (stageIndex+1) + "���ù��";
        }
        else if (stageNum == 2)
        {
            UIStage.text = "STAGE " + (stageIndex+1) + "������ ����";
        }
        else if (stageNum == 3)
        {
            UIStage.text = "STAGE " + (stageIndex+1) + "�̷��� ����";
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                // Ÿ���� �߿� �����̽��� ������ ��ü ���� ���
                Text.text = talkData[countNum][0];
                StopCoroutine(typingCoroutine);
                isTyping = false;
                isComplete = true;

                Cursor.SetActive(true);
            }
            else if (isComplete && talkSet.activeSelf)
            {
                // ������ ������ ��µ� ���¿��� �����̽��� ������ ���� ��ȭ�� �̵�
                countNum++;
                if (talkData.ContainsKey(countNum))
                {
                    if (countNum == 5)
                    {
                        talkSet.SetActive(false);
                        return;
                    }
                    else if (countNum == 8)
                    {
                        talkSet.SetActive(false);
                        Invoke("ViewBtn", 1);
                        return;
                    }
                    else if (countNum == 13)
                    {
                        talkSet.SetActive(false);
                        Invoke("Next", 2);
                        return;
                    }
                    else if(countNum == 17)
                        Invoke("Next", 2);
                    ShowText();
                }
                else
                {
                    Debug.Log("��ȭ ����");
                    talkSet.SetActive(false); // ��� ��ȭ�� ������ �� talkSet�� ��Ȱ��ȭ
                }
            }
        }
    }

    void Next()
    {
        SceneManager.LoadScene("Stage2");
    }

    public void NextStage()
    {
        if (stageIndex != 2)
        {
            //�������� ������ Ȯ���Ͽ� ������������ �̵� / ���� ����
            //Change Stage
            if (stageIndex < Stages.Length - 1)
            {
                Stages[stageIndex].SetActive(false);
                stageIndex++;
                //stageInde�� ���� �������� Ȱ��ȭ/��Ȱ��ȭ
                Stages[stageIndex].SetActive(true);
                //PlayerReposition();
                player.transform.position = new Vector3(0, 0, 0);//������ġ
                player.VelocityZero();

                stageNum = stageNum + 1;
            }
            else
            {
                //Game Clear
                Time.timeScale = 0; //�����ϰ� �Ǹ� timeScale = 0���� �ð��� �����
                                    //Player Control Lock
                                    //Result UI
                Debug.Log("���� Ŭ����!");
                //Restart Button UI
                //��ư �ؽ�Ʈ�� �ڽĿ�����Ʈ�̹Ƿ� InChildren�� �� �ٿ��� ��
                TextMeshProUGUI btnText = UIRestartBtn.GetComponentInChildren<TextMeshProUGUI>();
                btnText.text = "Clear!";
                ViewBtn();
            }

            //Calculate Point
            totalPoint += stagePoint;
            stagePoint = 0;
        }
        else
        {
            //UI ȭ�� ���� ���� ����
            //�׷��ԵǸ� ��ư Ŭ���� ����ȯ�ϴ°ɷ�
            if (totalPoint >= 50)
            {
                countNum = 9;
                talkSet.SetActive(true);
                ShowText();
                //SceneManager.LoadScene("Stage2");
            }
            else
            {
                countNum = 5;
                talkSet.SetActive(true);
                ShowText();
                //Restart();
            }
        }
    }

    public void HealthDown()
    {
        if (health > 1)
        {
            health--;
            UIhealth[health].color = new Color(1, 0, 0, 0.4f);
        }
        else
        {
            //All Health UI Off
            UIhealth[0].color = new Color(1, 0, 0, 0.4f);
            //Player Die Effect
            player.OnDie();
            //Result UI
            Debug.Log("�׾����ϴ�");
            //Retry Button UI
            UIRestartBtn.SetActive(true);
        }
    }

    public void HealthUp()
    {
        if (health < 3)
        {
            UIhealth[health].color = new Color(1, 1, 1, 1);
            health++;
        }
    }

    public void PlayerReposition()
    {
        if(health > 1)
        {
            player.transform.position = new Vector3(0, 0, 0);//������ġ
            player.VelocityZero();
        }
        HealthDown();
    }

    void ViewBtn()
    {
        UIRestartBtn.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    void GenerateData()
    {
        talkData.Add(0, new string[] { "���� �̵� ���� �� ������ �����ٴ�.." });
        talkData.Add(1, new string[] { "���� 5000���� ������ ���ϸ� �� ������ ���������� ���� ���̴�!" });
        talkData.Add(2, new string[] { "�׷�.. ���� �׳��� 5000���� �����ٸ�.. ���� �׳��� �ҿ��� ���� ȣ������ ����!" });
        talkData.Add(3, new string[] { "������.. ���� �������� �� ������??" });
        talkData.Add(4, new string[] { "������������!!!!" });

        talkData.Add(5, new string[] { "�ᱹ�� ���� 5000���� ������ ���߱���" });
        talkData.Add(6, new string[] { "�׳��� ������ �̰��� ��� �� ���� �� ����!" });
        talkData.Add(7, new string[] { "������������!!!!" });
        talkData.Add(8, new string[] { "������������!!!!!!!!" });

        talkData.Add(9, new string[] { "�����.." });
        talkData.Add(10, new string[] { "���� 5000���� �����ٴ�!!!" });
        talkData.Add(11, new string[] { "�׷�.. ��Ӵ�� ���� �׳��� �ҿ��� ���� ȣ������ ����!" });
        talkData.Add(12, new string[] { "���� ����....." });
        talkData.Add(13, new string[] { "������������!!!!" });
        talkData.Add(14, new string[] { "������������!!!!!!!!" });
        talkData.Add(15, new string[] { "������������!!!!!!!!" });
        talkData.Add(16, new string[] { "������������!!!!!!!!" });
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

        Cursor.SetActive(true);
    }
}
