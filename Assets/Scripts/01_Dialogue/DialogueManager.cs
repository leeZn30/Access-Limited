using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;

struct PosPair
{
    public int pos;
    public Character character;

    public PosPair(int p, Character c)
    {
        pos = p;
        character = c;
    }

}

public class DialogueManager : Singleton<DialogueManager>
{
    [Header("�� ����")]
    [SerializeField] int chapter;
    // dialogueUI�� �����ִ��� > �̰� �����־�� ��� ��ȭ�� ���õ� ��� ����
    public bool isEnable = false;
    [SerializeField] bool isDLogOpen = false;

    [Header("��ȭ ����")]
    public int nowTurn; // �����Ҷ��� public
    [SerializeField] int nextTurn;
    [SerializeField] int type = 0;
    public int mission = 0;
    public bool missionRunning = false;
    public bool isLineEnd = false;

    [Header("���� ĳ����")]
    [SerializeField] string speakingC;
    [SerializeField] List<PosPair> Cposes = new List<PosPair>();
    [SerializeField] List<Character> characters = new List<Character>();
    /**
    [SerializeField] int characterNum;
    [SerializeField] string[] ids = new string[3] {"", "", ""};
    [SerializeField] int[] illusts = new int[3] { -1, -1, -1 };
    [SerializeField] Character[] characters = new Character[3] {null, null, null};
    **/

    [Header("CSV ����")]
    [SerializeField] TextAsset d_file;
    [SerializeField] TextAsset a_file; // GameManager�� �ִ°� ���� ����

    [Header("CSV ���")]
    public List<Dictionary<string, object>> lines;
    public Dictionary<string, object> line;
    public List<Dictionary<string, object>> answers;
    Queue<Dictionary<string, object>> lineQueue = new Queue<Dictionary<string, object>>();
    [SerializeField] bool skipD = false; // tmp

    [Header("������Ʈ")]
    [SerializeField] GameObject dialogueUIs;
    [SerializeField] Dialogue dialogueBox;
    [SerializeField] DialogueLog dialogueLog;
    [SerializeField] Background backgroundCanvas;
    [SerializeField] PushEffect push;
    [SerializeField] Character characterPrb;

    void Awake()
    {
        chapter = GameData.Instance.chapter;

        // ĳ���� �ؽ����̺� (�Ŀ��� ���� ���� ����÷� ����)
        CharacterTable.setTable();

        // ��� ã��
        backgroundCanvas = GameObject.Find("BackgroundCanvas").GetComponentInChildren<Background>();
    }

    void Start()
    {
        // ���̾�α� ���϶� �ٷ� ���� ��
        if (d_file != null && !skipD)
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
            else if (mission != 0 && !missionRunning)
                doMission(type);
        }

        if (Input.GetKeyDown(KeyCode.L) && isEnable)
        {
            openCloseDialogueLog();
        }

    }

    // ���̾�α� �Ŵ��� ����
    public void resetDialogueManager(TextAsset d_file)
    {
        // CSV���� �б�
        this.d_file = d_file;
        lines = CSVReader.Read("CSVfiles/01_Dialogue/" + chapter + "/" + d_file.name);
        //lines = CSVReader.Read("CSVfiles/01_Dialogue/" + d_file.name);

        // ĳ���� �ʱ�ȭ
        speakingC = null;
        /**
        characterNum = 0;
        ids = new string[3] { "", "", "" };
        illusts = new int[3] { -1, -1, -1 };
        characters = new Character[3] { null, null, null };
        **/

        openCloseDialogue();

        nowTurn = 0;
        goTurn(nowTurn);
    }

    // ���� ���� ���� �޼���
    public void nextDialogue()
    {
        // lineQueue�� ������� ���� ������
        if (lineQueue.Count == 0)
        {
            goTurn(nextTurn);
            return;
        }

        destroyObjects();
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
        
        /**
        GameObject[] character_objs = GameObject.FindGameObjectsWithTag("Character");
        foreach (GameObject character in character_objs)
        {
            character.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
            character.SetActive(false);
            ObjectPool.Instance.CharacterQueue.Enqueue(character);
        }
        **/

    }

    // Ư�� ������ �̵�
    public void goTurn(int startTurn)
    {
        destroyObjects();
        nowTurn = startTurn;
        nextTurn = nowTurn + 1;
        readTurn();
    }


    // �ش� ���� ��� ���� �ҷ�����
    void readTurn()
    {
        lineQueue.Clear();
        List<Dictionary<string, object>> turnLines =
            lines.Where(e => int.Parse(e["Turn"].ToString()) == nowTurn).ToList();

        // �� �̻� ���� ����
        if (turnLines.Count == 0)
        {
            //Debug.Log("======��� ����=======");
            destroyObjects();
            openCloseDialogue();
            nowTurn = 0;
            return;
        }

        foreach(Dictionary<string, object> element in turnLines)
        {
            lineQueue.Enqueue(element);
        }

        readlines();

        // ���1) ���⼭ ĳ���� ���۷����� -> �ϸ��� ĳ���� ���� + �Ϸ���Ʈ�� �ٲٱ� (���� �Լ�) => readline����
        //characterMoving();
        testCharacter();
    }

    // ���� �б�
    void readlines()
    {
        line = lineQueue.Dequeue();

        int.TryParse(line["Type"].ToString(), out type); // default = 0

        // SpeakingId
        if (line["SpeakingId"].ToString() != "")
            speakingC = line["SpeakingId"].ToString();

        // Dialogue
        dialogueBox.line = line["Dialogue"].ToString().Replace("|", "\n");
        dialogueBox.c_name = getCharacterName(speakingC);
        dialogueLog.addLog(dialogueBox.c_name, dialogueBox.line);
        dialogueBox.showline();

        // ��ȭ�� ��Ÿ�� �ܼ�/�ι�
        checkInfos();

        // nextTurn
        int tmpNext;
        nextTurn = int.TryParse(line["NextTurn"].ToString(), out tmpNext) ? tmpNext : nextTurn;

        //���2) ĳ���� �� ���θ��� üũ�ϱ�
        //characterOperator();

        // ��� �ִٸ� ����
        int BGid;
        if (int.TryParse(line["Background"].ToString(), out BGid))
            backgroundCanvas.setBackground(chapter, BGid);

        // �̼�
        mission = type;

        // ���� ������Ʈ ��ȭ
        string chaining = line["TriggerObject"].ToString();
        if (chaining != "")
        {
            ObjectData objectdata = ObjectTable.oTable[chaining] as ObjectData;
            objectdata.openDialogue(line["OpenDialogue"].ToString());
        }

    }

    void checkInfos()
    {
        if (line["InfoId"].ToString() != "")
        {
            string privisoId = line["InfoId"].ToString();
            int idx = int.Parse(line["InfoIdx"].ToString());
            GameData.Instance.addInfo(privisoId, idx);
        }

    }

    public void showInfo(string text)
    {
        push.setText(text);
        push.appearInfo();
    }

    protected virtual void doMission(int type)
    {
        missionRunning = true;
        switch (type)
        {
            case 1: // ������
                getAnswers();
                break;

            case 2: // ���� ����
                break;

            default:
                break;
        }
    }

    void printPospair()
    {
        Debug.Log("=================Character Poses=====================");
        foreach(PosPair item in Cposes)
        {
            Debug.Log("[Pos]: " + item.pos + " [Character]: " + item.character.id);
        }
    }

    void testCharacter()
    {
        int idx1 = -1;
        int idx2 = -1;

        List<int> deletes = new List<int>(3) { 0, 1, 2 };
        if (line["C1"].ToString() != "")
        {
            idx1 = characters.FindIndex(e => e.id == line["C1"].ToString());

            if (idx1 != -1)
                deletes.Remove(idx1);
        }

        if (line["C2"].ToString() != "")
        {
            idx2 = characters.FindIndex(e => e.id == line["C2"].ToString());

            if (idx2 != -1)
                deletes.Remove(idx2);
        }

        Debug.Log("[idx1]: " + idx1 + " [idx2]: " + idx2);

        // �ʿ���� ĳ���� ������Ʈ ���� �����
        foreach (int i in deletes)
        {
            Debug.Log("[Delete]: " + i);
            destroyCharacter(i);
        }

        Character c1 = null;
        Character c2 = null;
        // characters �ٲٸ鼭 �̵� �� �߰�
        if (line["C2"].ToString() != "")
        {
            if (idx1 != -1)
                characters[idx1].moveLeft();
            else
            {
                c1 = createCharacter();
                c1.setCharacter(line["C1"].ToString(), getCharacterName(line["C1"].ToString()), 0);
                c1.setPosition(0);
            }
            
            if (idx2 != -1)
                characters[idx2].moveRight();
            else
            {
                c2 = createCharacter();
                c2.setCharacter(line["C2"].ToString(), getCharacterName(line["C2"].ToString()), 0);
                c2.setPosition(2);
            }

            if (idx1 != -1)
                characters[0] = characters[idx1];
            else
                characters[0] = c1;

            if (idx2 != -1)
                characters[2] = characters[idx2];
            else
                characters[0] = c1;

            characters[1] = characterPrb;
        }
        else
        {
            if (idx1 != -1)
                characters[idx1].moveMiddle();
            else
            {
                c1 = createCharacter();
                c1.setCharacter(line["C1"].ToString(), getCharacterName(line["C1"].ToString()), 0);
                c1.setPosition(1);
            }

            if (idx1 != -1)
                characters[1] = characters[idx1];
            else
                characters[1] = c1;
        }

    }

    void characterOperator()
    {
        int characterNum;
        if (int.TryParse(line["Cnum"].ToString(), out characterNum))
        {
            switch (characterNum)
            {
                case 0:
                    Cposes.Clear();
                    break;

                default:
                    characterMoving();
                    break;
            }
        }
        else // �״��
        {
            characterMoving();
        }
    }

    void characterMoving()
    {
        List<PosPair> pairs = Cposes;

        bool multiPeople = false;

        // C1�� C2�� ������ ���� ù ������
        if(line["C2"].ToString() != "")
        {
            PosPair posC2 = Cposes.Find(e => e.character.id == line["C2"].ToString());
            Character c2 = null;

            if (posC2.character == null)
            {
                c2 = createCharacter();
                c2.setCharacter(
                                line["C2"].ToString(),
                                getCharacterName(line["C2"].ToString()),
                                0// �ϴ�
                                ) ;
                c2.setPosition(2);

                PosPair tmp = new PosPair(2, c2);
                pairs.Add(tmp);
            }
            else
            {
                if (posC2.pos != 2)
                {
                    // �ڱ� �ڸ�
                    int idx = Cposes.FindIndex(e => e.Equals(posC2));

                    c2 = posC2.character;
                    c2.moveRight();
                    pairs[idx] = new PosPair(2, c2);
                }
            }
            multiPeople = true;
        }

        if (line["C1"].ToString() != "")
        {
            PosPair posC1 = Cposes.Find(e => e.character.id == line["C1"].ToString());
            Character c1 = null;

            if (posC1.character == null)
            {
                c1 = createCharacter();
                c1.setCharacter(
                                line["C1"].ToString(),
                                getCharacterName(line["C1"].ToString()),
                                0 // �ϴ�
                                );

                PosPair tmp;
                if (multiPeople)
                {
                    tmp = new PosPair(0, c1);
                    c1.setPosition(0);
                }
                else
                {
                    tmp = new PosPair(1, c1);
                    c1.setPosition(1);
                }
                pairs.Add(tmp);
            }
            else
            {
                c1 = posC1.character;

                if (multiPeople)
                {
                    if (posC1.pos != 0)
                    {
                        // �ڱ� �ڸ�
                        int idx = Cposes.FindIndex(e => e.Equals(posC1));
                        c1.moveLeft();
                        pairs[idx] = new PosPair(0, c1);

                    }
                }
                else
                {
                    if (posC1.pos != 1)
                    {
                        int idx = Cposes.FindIndex(e => e.Equals(posC1));
                        c1.moveMiddle();
                        pairs[idx] = new PosPair(1, c1);

                    }
                }
            }
        }

        Cposes = pairs;
        printPospair();
    }

    Character createCharacter()
    {
        Character c = ObjectPool.Instance.CharacterQueue.Dequeue().GetComponent<Character>();
        c.gameObject.SetActive(true);

        return c;
    }

    void destroyCharacter(int idx)
    {
        ObjectPool.Instance.CharacterQueue.Enqueue(characters[idx].gameObject);
        characters[idx].gameObject.SetActive(false);
    }

    protected void getAnswers()
    {
        if (a_file != null)
        {
            string answerId = line["AnswerId"].ToString();

            answers = CSVReader.Read("CSVfiles/01_Dialogue/" + chapter + "/" + a_file.name).Where(answer => answer["Id"].ToString() == answerId).ToList();
            //answers = CSVReader.Read("CSVfiles/01_Dialogue/" + a_file.name).Where(answer => answer["Id"].ToString() == answerId).ToList();
            createAnswer();
        }
    }

    void createAnswer()
    {
        foreach (Dictionary<string, object> answer in answers)
        {
            Answer a = ObjectPool.Instance.AnswerQueue.Dequeue().GetComponent<Answer>();

            a.content = answer["Content"].ToString();
            /**
            a.S1 = int.Parse(answer["S1"].ToString());
            a.S2 = int.Parse(answer["S1"].ToString());
            **/

            int tmpNext;
            a.nextTurn = int.TryParse(answer["NextTurn"].ToString(), out tmpNext) ? tmpNext : nextTurn;

            a.gameObject.SetActive(true);
        }

    }

    string getCharacterName(string id)
    {
        try
        {
            CharacterData element = CharacterTable.cTable[id] as CharacterData;
            return element.name;
        }
        catch
        {
            Debug.LogError("No Such id: " + id);
            return null;
        }
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

            if (MapManager.Instance != null)
            {
                MapManager.Instance.offInteractiveObject();
                MapManager.Instance.offPlaceTranslator();
                MapManager.Instance.isSlidingEnable = false;
            }
        }
        else
        {
            dialogueUIs.SetActive(false);
            isEnable = false;

            if (MapManager.Instance != null)
            {
                MapManager.Instance.onInteractiveObject();
                MapManager.Instance.onPlaceTranslator();
                MapManager.Instance.isSlidingEnable = true;
            }
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
