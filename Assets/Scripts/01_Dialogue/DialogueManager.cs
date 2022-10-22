using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogueManager : Singleton<DialogueManager>
{
    [Header("�� ����")]
    [SerializeField] int chapter;
    // dialogueUI�� �����ִ��� > �̰� �����־�� ��� ��ȭ�� ���õ� ��� ����
    public bool isEnable = false;
    [SerializeField] bool isDLogOpen = false;

    [Header("Dialogue ����")]
    public int mission = 0;
    [SerializeField] int now_turn;
    [SerializeField] int type = 0;
    public int chosen_line;
    [SerializeField] bool nextEnd = false;

    [Header("��� ���")]
    public bool isLineEnd = false;

    [Header("���� ĳ����")]
    [SerializeField] int speakingC;
    [SerializeField] int characterNum;
    [SerializeField] int[] ids = new int[3] {-1, -1, -1};
    [SerializeField] Character[] characters = new Character[3] {null, null, null};

    [Header("CSV ����")]
    [SerializeField] TextAsset d_file;
    [SerializeField] TextAsset p_file;
    [SerializeField] TextAsset a_file; // GameManager�� �ִ°� ���� ����

    [Header("CSV ���")]
    public List<Dictionary<string, object>> lines;
    public Dictionary<string, object> line;
    public List<Dictionary<string, object>> answers;

    [Header("������Ʈ")]
    [SerializeField] GameObject dialogueUIs;
    [SerializeField] Dialogue dialogueBox;
    [SerializeField] GameObject answer_box;
    [SerializeField] DialogueLog dialogueLog;
    [SerializeField] Background backgroundCanvas;

    [Header("��� ������")]
    [SerializeField] Answer answer_prb;
    [SerializeField] Character character_prb;

    void Awake()
    {
        chapter = GameData.Instance.chapter;

        // ĳ���� �ؽ����̺� (�Ŀ��� ���� ���� ����÷� ����)
        CharacterTable.setTable();

        // ���� ��µǴ� �ϰ� �ش� ����
        now_turn = 0;
        chosen_line = 0;

        // ��� ã��
        backgroundCanvas = GameObject.Find("Background").GetComponent<Background>();
    }


    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) && isEnable && !isDLogOpen)
        {
            if (mission == 0 && isLineEnd)
                nextDialogue();
            else if (!isLineEnd)
                dialogueBox.callStopTyping();
        }

        if (Input.GetKeyDown(KeyCode.L) && isEnable)
        {
            openCloseDialogueLog();
        }

    }

    // ���̾�α� �Ŵ��� ����
    public void setDialogueManager(TextAsset d_file)
    {
        // CSV���� �б�
        this.d_file = d_file;
        lines = CSVReader.Read("CSVfiles/01_Dialogue/" + chapter + "/" + d_file.name);

        openCloseDialogue();

        readlines();
    }

    // ���̾�α�UI Ű�����
    void openCloseDialogue()
    {
        if (!isEnable)
        {
            dialogueUIs.SetActive(true);
            isEnable = true;
        }
        else
        {
            dialogueUIs.SetActive(false);
            isEnable = false;
        }
    }

    // ���� ���� ���� �޼���
    public void nextDialogue()
    {
        if (!nextEnd)
        {
            destroyObjects();
            now_turn++;
            readlines();
        }
    }

    // ��ȭ�� �Ѿ�� ����ų� �ʱ�ȭ�ϴ� �͵�
    void destroyObjects()
    {
        GameObject[] answer_objs = GameObject.FindGameObjectsWithTag("Answer");
        foreach (GameObject answer in answer_objs)
        {
            Destroy(answer);
        }

        // �ٸ� Character�� ã�Ƽ� ���� > ������
        GameObject[] character_objs = GameObject.FindGameObjectsWithTag("Character");
        foreach (GameObject character in character_objs)
        {
            Destroy(character);
        }

        mission = 0;
    }

    // ���� �б�
    void readlines()
    {
        try
        {
            line = lines.Where(turn => turn["Turn"].ToString() == now_turn.ToString()).ToList()[chosen_line]; // �� int.Parse �ȵ�
        }
        catch (ArgumentException e)
        {
            Debug.Log("+++++++csv ����+++++++++");
            destroyObjects();
            openCloseDialogue();
        }

        int.TryParse(line["Type"].ToString(), out type);

        // ��ȭ�� ��Ÿ�� �ܼ�/�ι�
        checkInfos();

        // ĳ���� ���� ���� ��
        if (line["CharacterId"].ToString() != "")
            speakingC = int.Parse(line["CharacterId"].ToString());

        if (line["CharacterNum"].ToString() != "")
            characterNum = int.Parse(line["CharacterNum"].ToString());
        operateCharacter();

        // ��� �� �̸� ����
        dialogueBox.line = line["Dialogue"].ToString();
        dialogueBox.c_name = getCharacterName(speakingC);
        dialogueLog.addLog(dialogueBox.c_name, dialogueBox.line);
        dialogueBox.showline();

        // ��信 ���� ������ ���ӵȴٸ�
        int lineoffset;
        if (int.TryParse(line["LineOffset"].ToString(), out lineoffset))
            chosen_line = lineoffset;

        // ��� �ִٸ� ����
        int BGid;
        if (int.TryParse(line["Background"].ToString(), out BGid))
            backgroundCanvas.setBackground(chapter, BGid);

        // �̼�
        doMission(type);
    }

    void checkInfos()
    {
        if (line["PrivisoId"].ToString() != "")
        {
            string privisoId = line["PrivisoId"].ToString();
            GameData.Instance.addPriviso(privisoId);
        }

        if(line["FigureId"].ToString() != "")
        {
            string figureId = line["FigureId"].ToString();
            GameData.Instance.addFigures(figureId);
        }
    }

    void doMission(int type)
    {
        switch (type)
        {
            case 1:
                Instance.getAnswers();
                break;

            default:
                break;
        }
    }

    void operateCharacter()
    {
        // �� ���� ���� �ȵ�
        switch (characterNum)
        {
            case 1:
                Character character = Instantiate(character_prb, new Vector3(0, 1), Quaternion.identity);
                character.id = int.Parse(line["MCharacter"].ToString());
                character.c_name = getCharacterName(character.id);
                character.illust_num = getCharacterIllustNum(character.id);
                character.now_illust = int.Parse(line["MCIllust"].ToString());

                ids[1] = character.id;

                if (characters[0] != null || characters[2] != null)
                {
                    if (character.id == characters[0].id)
                    {
                        character.transform.position = new Vector3(-4, 1);
                        character.moveMiddle();
                    }
                    else if (character.id == characters[2].id)
                    {
                        character.transform.position = new Vector3(4, 1);
                        character.moveMiddle();
                    }

                }

                characters[1] = character;
                characters[0] = null;
                characters[2] = null;
                break;

            case 2:
                Character characterL = Instantiate(character_prb, new Vector3(-4, 1), Quaternion.identity);
                characterL.id = int.Parse(line["LCharacter"].ToString());
                characterL.c_name = getCharacterName(characterL.id);
                characterL.illust_num = getCharacterIllustNum(characterL.id);
                characterL.now_illust = int.Parse(line["LCIllust"].ToString());

                Character characterR = Instantiate(character_prb, new Vector3(4, 1), Quaternion.identity);
                characterR.id = int.Parse(line["RCharacter"].ToString());
                characterR.c_name = getCharacterName(characterR.id);
                characterR.illust_num = getCharacterIllustNum(characterR.id);
                characterR.now_illust = int.Parse(line["RCIllust"].ToString());

                // �����ϸ� ���� ���߱�
                if (characterL.id != speakingC)
                {
                    characterL.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.grey;
                }
                else if (characterR.id != speakingC)
                {
                    characterR.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.grey;
                }

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
    }

    void createAnswer()
    {
        foreach (Dictionary<string, object> answer in answers)
        {
            answer_prb.content = answer["Content"].ToString();
            answer_prb.S1 = int.Parse(answer["S1"].ToString());
            answer_prb.S2 = int.Parse(answer["S1"].ToString());

            int answer_offset;
            answer_prb.offset = int.TryParse(answer["Nextline"].ToString(), out answer_offset)? answer_offset:0;

            Instantiate(answer_prb, answer_box.transform);
        }

    }

    void getAnswers()
    {
        if (a_file != null)
        {
            string answerId = line["AnswerId"].ToString();
            Instance.mission = 1;

            answers = CSVReader.Read("CSVfiles/01_Dialogue/" + chapter + "/" + a_file.name).Where(answer => answer["Id"].ToString() == answerId).ToList();
            createAnswer();
        }
    }

    string getCharacterName(int id)
    {
        CharacterData element = CharacterTable.cTable[id] as CharacterData;
        return element.name;
    }

    int getCharacterIllustNum(int id)
    {
        CharacterData element = CharacterTable.cTable[id] as CharacterData;
        return element.illustNum;
    }

    void openCloseDialogueLog()
    {
        if (!isDLogOpen)
        {
            dialogueLog.gameObject.SetActive(true);
            isDLogOpen = true;
        }
        else
        {
            dialogueLog.gameObject.SetActive(false);
            isDLogOpen = false;
        }
    }
}
