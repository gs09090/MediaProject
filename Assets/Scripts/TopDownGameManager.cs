using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TopDownGameManager : MonoBehaviour
{
    public TalkManager talkManager;
    public QuestManager questManager;
    public Animator talkPanel;
    public Image portraitImg;
    public TypeEffect talk;
    public TextMeshProUGUI questText;
    public GameObject menuSet;
    public GameObject scanObject;
    public GameObject player;
    public bool isAction;
    public int talkIndex;
    public GameObject door;
    bool menuSetting = false;

    public AudioSource bgmSource;

    void Start()
    {
        GameLoad();
        Debug.Log(questManager.CheckQuest());
        questText.text = questManager.CheckQuest();

        PlayBGM();
    }

    void PlayBGM()
    {
        bgmSource.loop = true; // BGM을 반복 재생
        bgmSource.Play();
    }

    void Update()
    {
        //Sub Menu
        if (Input.GetButtonDown("Cancel"))
        {/*
            if (menuSet.activeSelf)
                menuSet.SetActive(true);
            else
                menuSet.SetActive(true);*/
            menuSet.SetActive(!menuSet.activeSelf);
        }    
    }

    public void Action(GameObject scanObj)
    {
        //Get Current Object
        scanObject = scanObj;
        ObjData objData = scanObject.GetComponent<ObjData>();
        Talk(objData.id, objData.isNpc);

        //Visible Talk for Action
        talkPanel.SetBool("isShow", isAction);
    }
           
    void Talk(int id, bool isNpc)
    {
        //Set Talk Data
        int questTalkIndex = 0;
        string talkData = "";

        if (talk.isAnim)
        {
            talk.SetMsg("");
            return;
        }
        else
        {
            questTalkIndex = questManager.GetQuestTalkIndex(id);
            //퀘스트번호 + NPC ID = 퀘스트 대화 데이터 Id
            talkData = talkManager.GetTalk(id + questTalkIndex, talkIndex);
        }

        //End Talk
        if(talkData == null)
        {
            isAction = false;
            talkIndex = 0;
            //Debug.Log(questManager.CheckQuest(id)); 댓츠 노노
            questText.text = questManager.CheckQuest(id);
            //Debug.Log(questText.text);
            return;
        }

        //Continue Talk
        if (isNpc)
        {
            talk.SetMsg(talkData.Split(':')[0]);

            //Show Portrait
            portraitImg.sprite = talkManager.GetPortrait(id, int.Parse(talkData.Split(':')[1]));
            portraitImg.color = new Color(1, 1, 1, 1);
        }
        else
        {
            talk.SetMsg(talkData);

            //Hide Portrait
            portraitImg.color = new Color(1, 1, 1, 0);
        }

        //Next Talk
        isAction = true;
        talkIndex++;
    }

    public void GameSave()
    {
        PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
        PlayerPrefs.SetFloat("QuestId", questManager.questId);
        PlayerPrefs.SetFloat("QuestActionIndex", questManager.questActionIndex);
        PlayerPrefs.Save();

        menuSet.SetActive(false);
    }

    public void GameLoad()
    {
        //최초 게임 실행했을 땐 데이터가 없으므로 예외처리 로직 작성
        if (!PlayerPrefs.HasKey("PlayerX"))
            return; //세이브 한 적이 없다면 로드하지 마!

        float x = PlayerPrefs.GetFloat("PlayerX");
        float y = PlayerPrefs.GetFloat("PlayerY");
        int questId = PlayerPrefs.GetInt("QuestId");
        int questActionIndex = PlayerPrefs.GetInt("QuestActionIndex");

        player.transform.position = new Vector3(x, y, 0);
        questManager.questId = questId;
        questManager.questActionIndex = questActionIndex;
        questManager.ControlObject();
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
