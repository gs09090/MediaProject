using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    public int questId;
    public int questActionIndex;
    public GameObject[] questObject;
    public bool isMoving = false;
    public bool reThink = false;

    Dictionary<int, QuestData> questList;
    
    void Awake()
    {
        if (DataManager.isFristStart)
            DataManager.isFristStart = false;
        else
        {
            //  두번째 부터 Datamanger에서 값 불러온다.
            questId = DataManager.currentId;
            questActionIndex = DataManager.currentQuestIndex;
        }


        questList = new Dictionary<int, QuestData>();
        GenerateData();
    }

    void GenerateData()
    {
        //int[]에는 해당 퀘스트에 연관된 NPC Id를 입력
        questList.Add(10, new QuestData("마을 사람들과 대화하기", new int[] { 1000, 2000 }));
        questList.Add(20, new QuestData("김장사의 엽전 세냥 찾아주기", new int[] { 5000, 5001, 5002, 2000 }));
        questList.Add(30, new QuestData("비밀의 문 찾기", new int[] { 1000, 6000, 6000 }));
        questList.Add(40, new QuestData("옆마을 이농부 찾아가기", new int[] { 100, 1000, 3000 }));
        questList.Add(50, new QuestData("이농부의 농사를 망치는 두더지 잡기", new int[] { 3000, 7000 }));
        questList.Add(60, new QuestData("냇가 가기", new int[] { 3000, 200, 1000, 4000 }));
        questList.Add(70, new QuestData("가락지 찾기", new int[] { 8000, 4000, 300}));
        questList.Add(80, new QuestData("고향집 가기", new int[] { 1000 }));
        questList.Add(90, new QuestData("퀘스트 올 클리어", new int[] { 0 }));
    }

    public int GetQuestTalkIndex(int id)
    {
        //퀘스트번호 + 퀘스트 대화순서 = 퀘스트 대화 ID
        return questId + questActionIndex;
    }

    public string CheckQuest(int id)
    {
        //Next Talk Target
        if (id == questList[questId].npcId[questActionIndex])
        {
            questActionIndex++;
            DataManager.currentQuestIndex = questActionIndex;
        }

        //Control Quest Object
        ControlObject();

        //Talk Complete & Next Quest
        if (questActionIndex == questList[questId].npcId.Length)
            NextQuest();

        //Quest Name
        return questList[questId].questName;
    }

    public string CheckQuest()
    {
        //Quest Name
        return questList[questId].questName;
    }

    void NextQuest()
    {
        questId += 10;
        questActionIndex = 0;

        DataManager.currentId = questId;
        DataManager.currentQuestIndex = questActionIndex;

    }

    public void ControlObject()
    {
        switch (questId)
        {
            case 10:
                if (questActionIndex == 2)
                {
                    questObject[0].SetActive(true);
                    questObject[1].SetActive(false);
                    questObject[2].SetActive(false);
                }
                break;
            
            case 20:
                if (questActionIndex == 0)
                {
                    questObject[0].SetActive(true);
                    questObject[1].SetActive(false);
                    questObject[2].SetActive(false);
                }
                if (questActionIndex == 1)
                {
                    questObject[0].SetActive(false);
                    questObject[1].SetActive(true);
                }
                if (questActionIndex == 2)
                {
                    questObject[1].SetActive(false);
                    questObject[2].SetActive(true);
                }
                if (questActionIndex == 3)
                {
                    questObject[2].SetActive(false);
                }
                if(questActionIndex == 4)
                    questObject[3].SetActive(true);
                break;
            case 30:
                if (questActionIndex == 0)
                    questObject[3].SetActive(true);
                if (questActionIndex == 2)
                {
                    isMoving = true;
                    questObject[4].SetActive(true);
                }
                if (questActionIndex == 3)
                {
                    reThink = true;
                }
                break;
            case 40:
                if (questActionIndex == 0)
                {
                    questObject[7].SetActive(true);
                    questObject[4].SetActive(false);
                }
                if (questActionIndex == 1)
                {
                    questObject[7].SetActive(false);
                }
                break;
            case 50:
                if (questActionIndex == 1)
                    questObject[5].SetActive(true);
                if (questActionIndex == 2)
                    SceneManager.LoadScene("Stage3");
                break;
            case 60:
                if (questActionIndex == 1)
                    questObject[8].SetActive(true);
                if (questActionIndex == 2)
                    questObject[8].SetActive(false);
                if (questActionIndex == 3)
                    questObject[6].SetActive(true);
                break;
            case 70:
                if (questActionIndex == 0)
                    questObject[6].SetActive(true);
                if (questActionIndex == 1)
                {
                    questObject[6].SetActive(false);
                    SceneManager.LoadScene("Stage4");
                }
                if (questActionIndex == 2)
                {
                    questObject[9].SetActive(true);
                }
                if (questActionIndex == 3)
                {
                    questObject[9].SetActive(false);
                }
                break;
            case 80:
                if (questActionIndex == 1)
                    SceneManager.LoadScene("Stage5");
                break;

        }
    }
}
