using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : Singleton<DialogueManager>
{
    [Header("Dialogue ����")]
    public int mission = 0;
    public int type = 0;

    [Header("���� ĳ����")]
    public int characterNum;
    [SerializeField] Character[] characters = new Character[3] {null, null, null};


    [Header("CSV ����")]
    [SerializeField] TextAsset d_file;
    [SerializeField] TextAsset a_file; // GameManager�� �ִ°� ���� ����

    [Header("CSV ���")]
    public List<Dictionary<string, object>> lines;
    public Dictionary<string, object> line;
    public int now_line;
    public List<Dictionary<string, object>> answers;

    [Header("Answer ����")]
    public string answerId;

    [Header("������Ʈ")]
    [SerializeField] Button reversebtn;
    [SerializeField] Dialogue dialogueBox;
    [SerializeField] GameObject answer_box;
    [SerializeField] Answer answer_prb;
    [SerializeField] Character character_prb;

    void Awake()
    {
        // CSV���� �б�
        lines = CSVReader.Read("CSVfiles/01_Dialogue/" + d_file.name);

        // ĳ���� �ؽ����̺� (�Ŀ��� ���� ���� ����÷� ����)
        CharacterTable.setTable();

        // ���� ����
        now_line = 0;

        // ��ư ����
        reversebtn.onClick.AddListener(previousDialogue);

        readlines();
    }

    void start()
    {
        //readlines(); �� ����ٰ� �ϸ� �ȵ���?
    }


    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (type == 0 && mission == 0)
                nextDialogue();
        }

    }


    public void nextDialogue()
    {

        if (now_line != lines.Count - 1)
        {
            destoryObjects();
            now_line++;
            readlines();
        }
    }

    void previousDialogue()
    {
        // now_line�� 0�̰� �� ��ȭ�� Ư�� ��ȭ(type != 0)�� �ƴϾ�� ���� ����
        if (now_line > 0 && int.Parse(lines[now_line - 1]["Type"].ToString()) == 0)
        {
            destoryObjects();

            now_line--;
            readlines();

        }
    }

    // ��ȭ ����,����� ����ų� �ʱ�ȭ�ϴ� �͵�
    void destoryObjects()
    {
        // ���� ���� Ÿ���� n�̶�� ó�����ֱ�
        switch (type)
        {
            case 1:
                // �ٸ� Answer�� ã�Ƽ� ���� > ������
                //Destroy(GameObject.FindGameObjectWithTag("Answer"));
                GameObject[] answer_objs = GameObject.FindGameObjectsWithTag("Answer");
                foreach(GameObject answer in answer_objs)
                {
                    Destroy(answer);
                }
                break;

            default:
                break;
        }

        // �ٸ� Character�� ã�Ƽ� ���� > ������
        //Destroy(GameObject.FindGameObjectWithTag("Character"));
        GameObject[] character_objs = GameObject.FindGameObjectsWithTag("Character");
        foreach (GameObject character in character_objs)
        {
            Destroy(character);
        }
        mission = 0;

    }

    void readlines()
    {
        line = lines[now_line];

        type = int.Parse(line["Type"].ToString());
        answerId = line["AnswerId"].ToString();

        // ��� �� �̸� ����
        dialogueBox.line = line["Dialogue"].ToString();
        dialogueBox.c_name = getCharacterName(int.Parse(line["CharacterId"].ToString()));
        dialogueBox.showline();

        // ĳ���� ���� ���� ��
        characterNum = int.Parse(line["CharacterNum"].ToString());
        // �� ���� ���� �ȵ�
        switch (characterNum)
        {
            case 1:
                Character character = Instantiate(character_prb, new Vector3(0, 1), Quaternion.identity);
                character.id = int.Parse(line["MCharacter"].ToString());
                character.c_name = getCharacterName(character.id);
                character.setIllust(int.Parse(line["MCIllust"].ToString()));

                if (characters[0] != null || characters[2] != null)
                {
                    if (character.id == characters[0].id)
                    {
                        character.transform.position = new Vector3(-5, 1);
                        character.moveMiddle();
                    }
                    else if (character.id == characters[2].id)
                    {
                        character.transform.position = new Vector3(5, 1);
                        character.moveMiddle();
                    }

                }

                characters[1] = character;
                characters[0] = null;
                characters[2] = null;
                break;

            case 2:
                Character characterL = Instantiate(character_prb, new Vector3(-5, 1), Quaternion.identity);
                characterL.id = int.Parse(line["LCharacter"].ToString());
                characterL.c_name = getCharacterName(characterL.id);
                characterL.setIllust(int.Parse(line["LCIllust"].ToString()));

                Character characterR = Instantiate(character_prb, new Vector3(5, 1), Quaternion.identity);
                characterR.id = int.Parse(line["RCharacter"].ToString());
                characterR.c_name = getCharacterName(characterR.id);
                characterR.setIllust(int.Parse(line["RCIllust"].ToString()));

                if (characters[1] != null)
                {
                    if (characterL.id == characters[1].id)
                    {
                        characterL.transform.position = new Vector3(0, 1);
                        characterL.moveLeft();
                    }
                    else if (characterR.id == characters[1].id)
                    {
                        characterR.transform.position = new Vector3(0, 1);
                        characterR.moveRight();
                    }
                }

                characters[1] = null;
                characters[0] = characterL;
                characters[2] = characterR;
                break;

            default:
                break;
        }

        // �÷��̾� ��� �䱸 ��
        if (type == 1)
        {
            Instance.mission = 1;
            Instance.getAnswers();
        }
    }

    void createAnswer()
    {
        foreach (Dictionary<string, object> answer in answers)
        {
            answer_prb.content = answer["Content"].ToString();
            answer_prb.S1 = int.Parse(answer["S1"].ToString());
            answer_prb.S2 = int.Parse(answer["S1"].ToString());

            Instantiate(answer_prb, answer_box.transform);
        }

    }

    void getAnswers()
    {
        answers = CSVReader.Read("CSVfiles/01_Dialogue/" + a_file.name).Where(answer => answer["Id"].ToString() == answerId).ToList();
        createAnswer();
    }


    string getCharacterName(int id)
    {
        return CharacterTable.cTable[id].ToString();
    }

}
