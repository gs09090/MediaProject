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
        talkData.Add(1000, new string[] { "�����!:0", "������ ��� ���ϼ̳���?:1", "���θ�����!:0" });
        talkData.Add(2000, new string[] { "�� �����!:1", "Ȥ�� ��������.. �� ���� �ƴմϴ� ����:1" });
        talkData.Add(3000, new string[] { "���� �����Դϴ�:1", "������ ���� ���� �ʾƿ�:2" });
        talkData.Add(4000, new string[] { "���� �ó��� �Ҹ��� ������ ��å ���̶��ϴ�..:1", "��Ӵϰ� ���� ���� ���̳׿�:3" });

        talkData.Add(100, new string[] { "���� ȣ�����̴�!" });
        talkData.Add(200, new string[] { "��� ȣ�����̴�!" });
        talkData.Add(300, new string[] { "�Ķ� ȣ�����̴�!" });

        talkData.Add(6000, new string[] { "�۸�" });

        //Quest Talk
        talkData.Add(10 + 1000, new string[] { "�����!:0",
                                               "���� ȣ������ ����� ���� ����簡 �˰� �־��.:1",
                                               "� �׸� ã�ư� ������!:0" });
        talkData.Add(11 + 2000, new string[] { "��¾ �Ϸ� ���� ã�ƿ��̴�����?:0",
                                               "...���� ȣ�����̿�?:1",
                                               "�׷��� �� ���� ���� �����ֽðھ��?:0" ,
                                               "���� �������� ���� ������ �Ҿ������.. ã���ֽðھ��?:3" });

        talkData.Add(20 + 1000, new string[] { "�װ� ���������?:1",
                                               "�����̿�??:3",
                                               "���� ���͵帱�Կ�!.:3"});

        talkData.Add(20 + 2000, new string[] { "��������?:1",
                                               "�����ΰ���?:3",
                                               "���� �󸶵��� ��ٸ� �� �ֽ��� ����.:1"});

        talkData.Add(20 + 5000, new string[] { "��ó���� ù��° ������ ã�Ҵ�.", });
        talkData.Add(21 + 5001, new string[] { "��ó���� �ι�° ������ ã�Ҵ�.", });
        talkData.Add(22 + 5002, new string[] { "��ó���� ����° ������ ã�Ҵ�.", "� ����翡�� ������ ����!" });

        talkData.Add(23 + 2000, new string[] { "��, ã���༭ ������.:2",
                                               "������ ã���ּ����� ��Ӵ�� ���� ȣ������ ����� �˷��帱�Կ�.:0",
                                               "���� ȣ������ ������ ������ �ִ� ����� ���� ���� ���� ã�� �� �־��.:1",
                                               "..����� ���� ����ֳİ��?:3",
                                               "�ҹ��� ���ϸ� �ٵ��̰� �ȴٰ�..:2",
                                               "�ٵ��̰� ���������? ������ ������ �� �Ѹ��� ���ݾƿ�! �� ���� �ٵ��̿���.:0"});

        talkData.Add(30 + 1000, new string[] { "�ٵ���..?:1",
                                               "�׳� ���� �ƴ϶� ��� ���� �� �� �ƴ� ����� ������.:2",
                                               "��� ���� �ɾ����!:0"});
        talkData.Add(31 + 6000, new string[] { "�Ʊ���� �� �ڲ� �Ѿƿ�?",
                                               "����� ���� �������״ϱ� �����."});
        talkData.Add(32 + 6000, new string[] { "�����",
                                               "�� ���� ������ ������ �������� �̵��ϰ� �ɰž�.",
                                               "�װ����� ���� ȣ������ ã���� ��.",
                                               "�װ��� ������ �� ���� �־�. �װ� �˰� ���°ž�?",
                                               "��...",
                                               "�����. ��ȸ�� �� �ѹ� ���̾�."});

        talkData.Add(40 + 100, new string[] { "���� ȣ�����̴�!" });
        talkData.Add(41 + 1000, new string[] { "�����! �س��̱���!!!:3",
                                               "���� ȣ������ �ż� ȭ���� ���� �� �־��:2", 
                                               "������� ����� ���� ���� ������ ��, ���� ��� ȣ������ ����� �˾Ƴ¾��:1",
                                               "������ �̳�ΰ� ��� ȣ������ ������ �ִٰ� �ؿ�:3",
                                               "���� �̹� ã�ư� ������.. ���� �ʾҾ��...:2"});
        talkData.Add(42 + 3000, new string[] { "�� �����. �������Դϴ�.:3",
                                               "�̹� ���κв� ���� �������� ���� ��� ȣ������ �� ������ �����ϴ�.:2",
                                               "������ ����Բ��� �� ���� ��ĩ�Ÿ��� �ذ����ֽŴٸ�..:1",
                                               "�ڲٸ� �̰��������� �δ����� ��Ÿ���� �ٶ��� ��縦 ��ġ�� �Ͼ��Դϴ�:3",
                                               "�δ������� ���i���ֽŴٸ� �Ⲩ�� ȣ������ �帮����!:2"});

        talkData.Add(50 + 3000, new string[] { "�δ����� �翡 ���� �� �� �����ſ���.:2",
                                               "10���� ������ �δ����� �� �̰������� ������ ���� �ſ���.:3",
                                               "������ ���� �༮�̴� �����ϼ���!:1"});
        talkData.Add(51 + 7000, new string[] { "�� �ڲ� �׷�",
                                               "�� ���� ����",
                                               "�ڲ� ���� �̷��� ������ �ɽ�����?",
                                               "�׷��ٸ� �� ������ ���ھ�!!"});

        talkData.Add(60 + 3000, new string[] { "�����.. ���� ����Ͻó׿�!:2",
                                               "�δ����� ��� ��������!!!:1",
                                               "�����մϴ�!:3",
                                               "���� ��� ȣ������ �帱�Կ�.:0",});
        talkData.Add(61 + 200, new string[] { "��� ȣ�����̴�!" });
        talkData.Add(62 + 1000, new string[] { "�����! ����������?:2",
                                               "���ڱ� �������� ������ ���εǴ� �� �˰� �󸶳� �������..:3",
                                               "�׷��� �տ� �װ�... ��� ȣ����!?:3",
                                               "���� ����ؿ�!!!:1",
                                               "��� ȣ������ �Ŵ��� ���ô����� ���� �� �־��:2",
                                               "�켱 ������ ���ż� �� ���̰�, ������ �ϰ� ������!:1"});
        talkData.Add(63 + 4000, new string[] { "����..:0",
                                               "�ʸ鿡 �Ƿ��մϴٸ� ��Ź�� �־...:1",
                                               "���� �� �������� ���� �������� �Ҿ���Ƚ��ϴ�....:3",
                                               "���ư��� ��ӴԲ��� �����ֽ� ������ ��ǰ�Դϴ�:3",
                                               "�ε� ����Բ��� �� �������� ã���ּ���..:1"});
        talkData.Add(70 + 4000, new string[] { "���� �˼��� ��Ź�� �� ������..:0",
                                               "�׷����� �ұ��ϰ�...:1",
                                               "��ġ������ �����ּ���....:3"});
        talkData.Add(70 + 8000, new string[] { "�������� ã�Ҵ�!", 
                                               "�׷���.. ������ ���� ��򰡷� �������..!!",
                                               "���ƾ�!!!"});
        talkData.Add(71 + 4000, new string[] { "��� �� ������!:1",
                                               "�������� �����մϴ�!:3",
                                               "�������� �� �Ķ� ȣ������ ����Բ� �帱�Կ�:0",
                                               "�ƹ����� ȥ���� �ֽ� �ź��� �����̿���:3",
                                               "�� �������� �� ������ ���� ���� ���ε� �װ� ������ �ִ��� ������� ������ �ִ°� �� �������� �� ���׿�.:2",
                                               "�ٽ� �ѹ� ����帳�ϴ�.. ����:3"});
        talkData.Add(72 + 300, new string[] { "�Ķ� ȣ�����̴�!", "��� ȣ������ �� ��Ҵ�!!!" });
        talkData.Add(80 + 1000, new string[] { "���� ��� ȣ������ ��ҳ׿�.:1",
                                               "�����.. �������� ���� ������ �غ� �Ǽ̳���?:2",
                                               "�ε� �� �ǰ���.. �ٳ������:0"});


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
                //����Ʈ �� ó�� ��縶�� ���� ��
                //�⺻ ��縦 ������ �´�
                return GetTalk(id - id % 100, talkIndex); //Get First Talk
            }
            else
            {
                //�ش� ����Ʈ ���� ���� ��簡 ���� ��
                //����Ʈ �� ó�� ��縦 ������ �´�
                //id�� ��ȭ Get -> talkIndex�� ��ȭ�� �� ������ Get

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
