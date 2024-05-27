using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;
    Dictionary<int, Sprite> portraitData;

    public Sprite[] portraitArr;

    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        portraitData = new Dictionary<int, Sprite>();
        GenerateData();
    }

    void GenerateData()
    {
        //Talk Data
        //NPC1:2000, NPC2:3000, NPC3:4000
        talkData.Add(1000, new string[] { "서방님!:0", "아직도 출발 안하셨나요?:1", "서두르세요!:0" });
        talkData.Add(2000, new string[] { "앗 선비님!:1", "혹시 마을에서.. 아 별거 아닙니다 허허:1" });
        talkData.Add(3000, new string[] { "저는 장사꾼입니다:1", "밑지는 장사는 하지 않아요:2" });
        talkData.Add(4000, new string[] { "그저 시냇물 소리를 들으며 산책 중이랍니다..:1", "어머니가 보고 싶은 날이네요:3" });

        talkData.Add(100, new string[] { "빨간 호리병이다!" });
        talkData.Add(200, new string[] { "노란 호리병이다!" });
        talkData.Add(300, new string[] { "파란 호리병이다!" });

        talkData.Add(6000, new string[] { "멍멍" });

        //Quest Talk
        talkData.Add(10 + 1000, new string[] { "서방님!:0",
                                               "빨간 호리병의 행방은 옆집 김장사가 알고 있어요.:1",
                                               "어서 그를 찾아가 보세요!:0" });
        talkData.Add(11 + 2000, new string[] { "어쩐 일로 저를 찾아오셨는지요?:0",
                                               "...빨간 호리병이요?:1",
                                               "그러면 그 전에 저를 도와주시겠어요?:0" ,
                                               "제가 마을에서 엽전 세냥을 잃어버려서.. 찾아주시겠어요?:3" });

        talkData.Add(20 + 1000, new string[] { "그가 뭐라던가요?:1",
                                               "엽전이요??:3",
                                               "저도 도와드릴게요!.:3"});

        talkData.Add(20 + 2000, new string[] { "엽전은요?:1",
                                               "아직인가요?:3",
                                               "저는 얼마든지 기다릴 수 있습죠 허허.:1"});

        talkData.Add(20 + 5000, new string[] { "근처에서 첫번째 엽전을 찾았다.", });
        talkData.Add(21 + 5001, new string[] { "근처에서 두번째 엽전을 찾았다.", });
        talkData.Add(22 + 5002, new string[] { "근처에서 세번째 엽전을 찾았다.", "어서 김장사에게 가져다 주자!" });

        talkData.Add(23 + 2000, new string[] { "엇, 찾아줘서 고마워요.:2",
                                               "엽전을 찾아주셨으니 약속대로 빨간 호리병의 행방을 알려드릴게요.:0",
                                               "빨간 호리병은 마을에 숨겨져 있는 비밀의 문을 열고 들어가면 찾을 수 있어요.:1",
                                               "..비밀의 문이 어디있냐고요?:3",
                                               "소문에 의하면 바둑이가 안다고..:2",
                                               "바둑이가 누구나고요? 마을에 떠돌이 개 한마리 있잖아요! 그 개가 바둑이에요.:0"});

        talkData.Add(30 + 1000, new string[] { "바둑이..?:1",
                                               "그냥 개가 아니라 사람 말을 할 줄 아는 개라고 들었어요.:2",
                                               "계속 말을 걸어봐요!:0"});
        talkData.Add(31 + 6000, new string[] { "아까부터 왜 자꾸 쫓아와?",
                                               "비밀의 문을 열어줄테니까 따라와."});
        talkData.Add(32 + 6000, new string[] { "여기야",
                                               "이 땅을 밟으면 미지의 공간으로 이동하게 될거야.",
                                               "그곳에서 빨간 호리병을 찾도록 해.",
                                               "그곳에 갇히게 될 수도 있어. 그건 알고 가는거야?",
                                               "후...",
                                               "명심해. 기회는 단 한번 뿐이야."});

        talkData.Add(40 + 100, new string[] { "빨간 호리병이다!" });
        talkData.Add(41 + 1000, new string[] { "서방님! 해내셨군요!!!:3",
                                               "빨간 호리병은 거센 화염을 만들어낼 수 있어요:2", 
                                               "서방님이 비밀의 문을 열고 들어갔었을 때, 저는 노란 호리병의 행방을 알아냈어요:1",
                                               "옆동네 이농부가 노란 호리병을 가지고 있다고 해요:3",
                                               "제가 이미 찾아가 봤지만.. 쉽지 않았어요...:2"});
        talkData.Add(42 + 3000, new string[] { "아 선비님. 오랜만입니다.:3",
                                               "이미 부인분께 말씀 전했지만 저는 노란 호리병을 줄 생각이 없습니다.:2",
                                               "하지만 선비님께서 제 오랜 골칫거리를 해결해주신다면..:1",
                                               "자꾸만 이곳저곳에서 두더지가 나타나는 바람에 농사를 망치기 일쑤입니다:3",
                                               "두더지들을 내쫒아주신다면 기꺼이 호리병을 드리지요!:2"});

        talkData.Add(50 + 3000, new string[] { "두더지는 밭에 가면 볼 수 있을거에요.:2",
                                               "10마리 정도의 두더지가 밭 이곳저곳을 헤집고 있을 거에요.:3",
                                               "굉장히 빠른 녀석이니 유념하세요!:1"});
        talkData.Add(51 + 7000, new string[] { "왜 자꾸 그래",
                                               "좀 살자 나도",
                                               "자꾸 나를 이렇게 괴롭힐 심신이지?",
                                               "그렇다면 널 데리고 가겠어!!"});

        talkData.Add(60 + 3000, new string[] { "선비님.. 정말 대단하시네요!:2",
                                               "두더지를 모두 사라졌어요!!!:1",
                                               "감사합니다!:3",
                                               "여기 노란 호리병을 드릴게요.:0",});
        talkData.Add(61 + 200, new string[] { "노란 호리병이다!" });
        talkData.Add(62 + 1000, new string[] { "서방님! 괜찮으세요?:2",
                                               "갑자기 땅속으로 꺼져서 과부되는 줄 알고 얼마나 놀랐던지..:3",
                                               "그런데 손에 그건... 노란 호리병!?:3",
                                               "정말 대단해요!!!:1",
                                               "노란 호리병은 거대한 가시덤불을 만들어낼 수 있어요:2",
                                               "우선 냇가에 가셔서 목도 축이고, 세수도 하고 오세요!:1"});
        talkData.Add(63 + 4000, new string[] { "저기..:0",
                                               "초면에 실례합니다만 부탁이 있어서...:1",
                                               "제가 이 냇가에서 귀한 가락지를 잃어버렸습니다....:3",
                                               "돌아가신 어머님께서 남겨주신 소중한 유품입니다:3",
                                               "부디 선비님께서 제 가락지를 찾아주세요..:1"});
        talkData.Add(70 + 4000, new string[] { "정말 죄송한 부탁인 걸 알지만..:0",
                                               "그럼에도 불구하고...:1",
                                               "염치없지만 도와주세요....:3"});
        talkData.Add(70 + 8000, new string[] { "가락지를 찾았다!", 
                                               "그런데.. 물길이 나를 어딘가로 끌어당긴다..!!",
                                               "으아아!!!"});
        talkData.Add(71 + 4000, new string[] { "어머 내 가락지!:1",
                                               "정말정말 감사합니다!:3",
                                               "보답으로 이 파란 호리병을 선비님께 드릴게요:0",
                                               "아버님이 혼수로 주신 신비한 물건이에요:3",
                                               "제 가락지를 이 냇가에 던진 이의 것인데 그가 가지고 있느니 선비님이 가지고 있는게 훨 쓸모있을 거 같네요.:2",
                                               "다시 한번 감사드립니다.. 정말:3"});
        talkData.Add(72 + 300, new string[] { "파란 호리병이다!", "모든 호리병을 다 모았다!!!" });
        talkData.Add(80 + 1000, new string[] { "드디어 모든 호리병을 모았네요.:1",
                                               "서방님.. 고향집에 가실 마음의 준비 되셨나요?:2",
                                               "부디 몸 건강히.. 다녀오세요:0"});


        //Portrait Data
        //0:Idle, 1:Talk, 2:Smile, 3:Angry 
        portraitData.Add(1000 + 0, portraitArr[0]);
        portraitData.Add(1000 + 1, portraitArr[1]);
        portraitData.Add(1000 + 2, portraitArr[2]);
        portraitData.Add(1000 + 3, portraitArr[3]);
        portraitData.Add(2000 + 0, portraitArr[4]);
        portraitData.Add(2000 + 1, portraitArr[5]);
        portraitData.Add(2000 + 2, portraitArr[6]);
        portraitData.Add(2000 + 3, portraitArr[7]);
        portraitData.Add(3000 + 0, portraitArr[8]);
        portraitData.Add(3000 + 1, portraitArr[9]);
        portraitData.Add(3000 + 2, portraitArr[10]);
        portraitData.Add(3000 + 3, portraitArr[11]);
        portraitData.Add(4000 + 0, portraitArr[12]);
        portraitData.Add(4000 + 1, portraitArr[13]);
        portraitData.Add(4000 + 2, portraitArr[14]);
        portraitData.Add(4000 + 3, portraitArr[15]);
    }

    public string GetTalk(int id, int talkIndex)
    {
        if (!talkData.ContainsKey(id))
        {

            if (!talkData.ContainsKey(id - id % 10))
            {
                //퀘스트 맨 처음 대사마저 없을 때
                //기본 대사를 가지고 온다
                return GetTalk(id - id % 100, talkIndex); //Get First Talk
            }
            else
            {
                //해당 퀘스트 진행 순서 대사가 없을 때
                //퀘스트 맨 처음 대사를 가지고 온다
                //id로 대화 Get -> talkIndex로 대화의 한 문장을 Get

                return GetTalk(id - id % 10, talkIndex); //Get First Quest Talk
            }
        }
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }

    public Sprite GetPortrait(int id, int portaitIndex)
    {
        return portraitData[id + portaitIndex];
    }
}
