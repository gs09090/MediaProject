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

    //UI를 담을 변수들을 생성 (이미지는 배열)
    public Image[] UIhealth;
    public TextMeshProUGUI UIPoint;
    public TextMeshProUGUI UIStage;
    public GameObject UIRestartBtn;

    public GameObject talkSet;
    public TMP_Text Text;
    public GameObject Cursor;
    public AudioSource typingSound; // 타이핑 사운드
    Dictionary<int, string[]> talkData;

    public int countNum = 0; // 대화 문구의 인덱스
    float typingSpeed = 0.1f; // 타자 속도 (초 단위)
    bool isTyping = false; // 타이핑 중인지 여부
    bool isComplete = false; // 문장이 완전히 출력되었는지 여부
    Coroutine typingCoroutine; // 현재 타이핑 코루틴

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
        //bgmSource.loop = true; // BGM을 반복 재생
        //bgmSource.Play();
    }

    void Update()
    {
        UIPoint.text = (totalPoint + stagePoint).ToString();
        if(stageNum == 1)
        {
            UIStage.text = "STAGE " + (stageIndex+1) + "가시밭길";
        }
        else if (stageNum == 2)
        {
            UIStage.text = "STAGE " + (stageIndex+1) + "여우의 습격";
        }
        else if (stageNum == 3)
        {
            UIStage.text = "STAGE " + (stageIndex+1) + "미로의 공간";
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                // 타이핑 중에 스페이스를 누르면 전체 문장 출력
                Text.text = talkData[countNum][0];
                StopCoroutine(typingCoroutine);
                isTyping = false;
                isComplete = true;

                Cursor.SetActive(true);
            }
            else if (isComplete && talkSet.activeSelf)
            {
                // 문장이 완전히 출력된 상태에서 스페이스를 누르면 다음 대화로 이동
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
                    Debug.Log("대화 종료");
                    talkSet.SetActive(false); // 모든 대화가 끝났을 때 talkSet을 비활성화
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
            //스테이지 갯수를 확인하여 다음스테이지 이동 / 종료 구현
            //Change Stage
            if (stageIndex < Stages.Length - 1)
            {
                Stages[stageIndex].SetActive(false);
                stageIndex++;
                //stageInde에 따라 스테이지 활성화/비활성화
                Stages[stageIndex].SetActive(true);
                //PlayerReposition();
                player.transform.position = new Vector3(0, 0, 0);//시작위치
                player.VelocityZero();

                stageNum = stageNum + 1;
            }
            else
            {
                //Game Clear
                Time.timeScale = 0; //완주하게 되면 timeScale = 0으로 시간을 멈춰둠
                                    //Player Control Lock
                                    //Result UI
                Debug.Log("게임 클리어!");
                //Restart Button UI
                //버튼 텍스트는 자식오브젝트이므로 InChildren을 더 붙여야 함
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
            //UI 화면 띄우기 코인 부족
            //그렇게되면 버튼 클릭시 씬전환하는걸로
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
            Debug.Log("죽었습니다");
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
            player.transform.position = new Vector3(0, 0, 0);//시작위치
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
        talkData.Add(0, new string[] { "감히 겁도 없이 내 공간에 들어오다니.." });
        talkData.Add(1, new string[] { "내공 5000점을 모으지 못하면 이 공간을 빠져나가지 못할 것이다!" });
        talkData.Add(2, new string[] { "그래.. 만약 네놈이 5000점을 모은다면.. 내가 네놈의 소원인 빨간 호리병을 주지!" });
        talkData.Add(3, new string[] { "하지만.. 과연 빠져나갈 수 있을까??" });
        talkData.Add(4, new string[] { "우하하하하하!!!!" });

        talkData.Add(5, new string[] { "결국은 내공 5000점을 모으지 못했구만" });
        talkData.Add(6, new string[] { "네놈은 영원히 이곳을 벗어날 수 없을 것 같군!" });
        talkData.Add(7, new string[] { "우하하하하하!!!!" });
        talkData.Add(8, new string[] { "우하하하하하!!!!!!!!" });

        talkData.Add(9, new string[] { "대단해.." });
        talkData.Add(10, new string[] { "내공 5000점을 모으다니!!!" });
        talkData.Add(11, new string[] { "그래.. 약속대로 내가 네놈의 소원인 빨간 호리병을 주지!" });
        talkData.Add(12, new string[] { "아주 놀라워....." });
        talkData.Add(13, new string[] { "우하하하하하!!!!" });
        talkData.Add(14, new string[] { "우하하하하하!!!!!!!!" });
        talkData.Add(15, new string[] { "우하하하하하!!!!!!!!" });
        talkData.Add(16, new string[] { "우하하하하하!!!!!!!!" });
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

        Cursor.SetActive(true);
    }
}
