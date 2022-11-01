using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;

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

    [Header("��� ���")]
    public bool isLineEnd = false;

    [Header("���� ĳ����")]
    [SerializeField] string speakingC;
    [SerializeField] int characterNum;
    [SerializeField] string[] ids = new string[3] {"", "", ""};
    [SerializeField] int[] illusts = new int[3] { -1, -1, -1 };
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
    [SerializeField] DialogueLog dialogueLog;
    [SerializeField] Background backgroundCanvas;

    [Header("��� ������")]
    [SerializeField] Answer answer_prb;

    void Awake()
    {
        chapter = GameData.Instance.chapter;

        // ĳ���� �ؽ����̺� (�Ŀ��� ���� ���� ����÷� ����)
        CharacterTable.setTable();

        // ���� ��µǴ� �ϰ� �ش� ����
        now_turn = 0;
        chosen_line = 0;

        // ��� ã��
        backgroundCanvas = GameObject.Find("BackgroundCanvas").GetComponentInChildren<Background>();

        // ���̾�α� ���϶� �ٷ� ���� ��
        if (d_file != null)
            resetDialogueManager(d_file);
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
    public void resetDialogueManager(TextAsset d_file)
    {
        now_turn = 0;

        // CSV���� �б�
        this.d_file = d_file;
        lines = CSVReader.Read("CSVfiles/01_Dialogue/" + chapter + "/" + d_file.name);

        // ĳ���� �ʱ�ȭ
        speakingC = null;
        characterNum = 0;
        ids = new string[3] { "", "", "" };
        illusts = new int[3] { -1, -1, -1 };
        characters = new Character[3] { null, null, null };

        openCloseDialogue();

        readlines();
    }

    // ���� ���� ���� �޼���
    public void nextDialogue()
    {
        destroyObjects();
        now_turn++;
        readlines();
    }

    // ��ȭ�� �Ѿ�� ����ų� �ʱ�ȭ�ϴ� �͵�
    void destroyObjects()
    {
        GameObject[] answer_objs = GameObject.FindGameObjectsWithTag("Answer");
        foreach (GameObject answer in answer_objs)
        {
            answer.SetActive(false);
            ObjectPool.Instance.AnswerQueue.Enqueue(answer.gameObject);
        }
        
        GameObject[] character_objs = GameObject.FindGameObjectsWithTag("Character");
        foreach (GameObject character in character_objs)
        {
            character.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
            character.SetActive(false);
            ObjectPool.Instance.CharacterQueue.Enqueue(character);
        }

        mission = 0;
    }

    // ���� �б�
    void readlines()
    {
        try
        {
            line = lines.Where(turn => turn["Turn"].ToString() == now_turn.ToString()).ToList()[chosen_line]; // �� int.Parse �ȵ�
            //Debug.Log("[" + now_turn + "]" + line["Dialogue"].ToString());

            int.TryParse(line["Type"].ToString(), out type);

            // ��ȭ�� ��Ÿ�� �ܼ�/�ι�
            checkInfos();

            // ĳ���� ���� ���� ��
            if (line["CharacterId"].ToString() != "")
                speakingC = line["CharacterId"].ToString();

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
        catch (ArgumentException e)
        {
            Debug.Log("+++++++csv ����+++++++++");
            destroyObjects();
            openCloseDialogue();
        }
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

            case 2:
                mission = 2;
                GameObject.Find("Info").GetComponent<TextMeshProUGUI>().text = "D�� ���� ��� ������ Ȯ���ϼ���.";
                if (Input.GetKeyDown(KeyCode.D))
                {
                    mission = 0;
                }
                GameObject.Find("Info").GetComponent<TextMeshProUGUI>().text = "";
                break;

            case 3:
                mission = 3;
                GameObject.Find("Info").GetComponent<TextMeshProUGUI>().text = "D�� ���� ��� ������ Ȯ���ϼ���.";
                // databaseManager.rayouts[2].isEnable
                if (true)
                {
                    mission = 0;
                    GameObject.Find("Info").GetComponent<TextMeshProUGUI>().text = "";
                }
                break;

            default:
                break;
        }
    }

    void operateCharacter()
    {
        switch (characterNum)
        {
            case 1:

                if (line["MCharacter"].ToString() != "")
                    ids[1] = line["MCharacter"].ToString();
                if (line["MCIllust"].ToString() != "")
                    illusts[1] = int.Parse(line["MCIllust"].ToString());
                
                Character m = ObjectPool.Instance.CharacterQueue.Dequeue().GetComponent<Character>();
                m.setCharacter(ids[1], getCharacterName(ids[1]), getCharacterIllustNum(ids[1]), illusts[1], new Vector3(0, 1));
                m.gameObject.SetActive(true);

                if (characters[0] != null || characters[2] != null)
                {
                    if (m.id == characters[0].id)
                    {
                        m.transform.position = new Vector3(-4, 1);
                        m.moveMiddle();
                    }
                    else if (m.id == characters[2].id)
                    {
                        m.transform.position = new Vector3(4, 1);
                        m.moveMiddle();
                    }

                }

                characters[1] = m;
                characters[0] = null;
                characters[2] = null;
                break;

            case 2:
                if (line["LCharacter"].ToString() != "")
                    ids[0] = line["LCharacter"].ToString();
                if (line["LCIllust"].ToString() != "")
                    illusts[0] = int.Parse(line["LCIllust"].ToString());

                Character l = ObjectPool.Instance.CharacterQueue.Dequeue().GetComponent<Character>();
                l.setCharacter(ids[0], getCharacterName(ids[0]), getCharacterIllustNum(ids[0]), illusts[0], new Vector3(-4, 1));
                l.gameObject.SetActive(true);


                if (line["RCharacter"].ToString() != "")
                    ids[2] = line["RCharacter"].ToString();
                if (line["RCIllust"].ToString() != "")
                    illusts[2] = int.Parse(line["RCIllust"].ToString());

                Character r = ObjectPool.Instance.CharacterQueue.Dequeue().GetComponent<Character>();
                r.setCharacter(ids[2], getCharacterName(ids[2]), getCharacterIllustNum(ids[2]), illusts[2], new Vector3(4, 1));
                r.gameObject.SetActive(true);

                // �����ϸ� ���� ���߱�
                if (!l.id.Equals(speakingC))
                {
                    l.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.grey;
                }
                else if (r.id != speakingC)
                {
                    r.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.grey;
                }

                if (characters[1] != null)
                {
                    if (l.id == characters[1].id)
                    {
                        l.transform.position = new Vector3(0, 1);
                        l.moveLeft();
                    }
                    else if (r.id == characters[1].id)
                    {
                        r.transform.position = new Vector3(0, 1);
                        r.moveRight();
                    }
                }

                characters[1] = null;
                characters[0] = l;
                characters[2] = r;
                break;

            default:
                break;
        }
    }

    void createAnswer()
    {
        foreach (Dictionary<string, object> answer in answers)
        {
            Answer a = ObjectPool.Instance.AnswerQueue.Dequeue().GetComponent<Answer>();

            a.content = answer["Content"].ToString();
            a.S1 = int.Parse(answer["S1"].ToString());
            a.S2 = int.Parse(answer["S1"].ToString());

            int answer_offset;
            a.offset = int.TryParse(answer["Nextline"].ToString(), out answer_offset) ? answer_offset : 0;

            a.gameObject.SetActive(true);
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

    string getCharacterName(string id)
    {
        CharacterData element = CharacterTable.cTable[id] as CharacterData;
        return element.name;
    }

    int getCharacterIllustNum(string id)
    {
        CharacterData element = CharacterTable.cTable[id] as CharacterData;
        return element.illustNum;
    }

    // ���̾�α�UI Ű�����
    void openCloseDialogue()
    {
        if (!isEnable)
        {
            dialogueUIs.SetActive(true);
            isEnable = true;

            MapManager.Instance.offInteractiveObject();
        }
        else
        {
            dialogueUIs.SetActive(false);
            isEnable = false;

            MapManager.Instance.onInteractiveObject();
        }
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
