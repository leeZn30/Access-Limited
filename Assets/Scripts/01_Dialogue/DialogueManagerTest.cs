using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;

public class DialogueManagerTest : Singleton<DialogueManagerTest>
{
    [Header("�� ����")]
    [SerializeField] int chapter;
    // dialogueUI�� �����ִ��� > �̰� �����־�� ��� ��ȭ�� ���õ� ��� ����
    public bool isEnable = false;
    [SerializeField] bool isDLogOpen = false;

    [Header("Dialogue ����")]
    public int mission = 0;
    public bool missionRunning = false;
    [SerializeField] int type = 0;
    public int lineoffset;

    [Header("��� ���")]
    public bool isLineEnd = false;

    [Header("���� ĳ����")]
    [SerializeField] string speakingC;
    [SerializeField] int characterNum;
    [SerializeField] Character c_prb;
    [SerializeField] Character[] characters = new Character[3];

    [Header("CSV ����")]
    [SerializeField] TextAsset d_file;
    [SerializeField] TextAsset a_file; // GameManager�� �ִ°� ���� ����

    [Header("CSV ���")]
    public List<Dictionary<string, object>> lines;
    public Dictionary<string, object> line;
    public List<Dictionary<string, object>> answers;
    Queue<Dictionary<string, object>> lineQueue = new Queue<Dictionary<string, object>>();
    public int nowLine;
    [SerializeField] bool skipD = false; // tmp

    [Header("������Ʈ")]
    [SerializeField] GameObject dialogueUIs;
    [SerializeField] Dialogue dialogueBox;
    [SerializeField] DialogueLog dialogueLog;
    [SerializeField] Background backgroundCanvas;
    [SerializeField] PushEffect push;

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

    void getLineQueue()
    {
        lineQueue.Clear();

        foreach (Dictionary<string, object> line in lines)
        {
            lineQueue.Enqueue(line);
        }

    }

    // �����߿��� �� ���
    public void goLine(int startIdx)
    {
        lineQueue.Clear();

        for (int i = startIdx; i < lines.Count; i++)
        {
            lineQueue.Enqueue(lines[i]);
        }

        readlines();
    }

    // ���̾�α� �Ŵ��� ����
    public void resetDialogueManager(TextAsset d_file)
    {
        lineoffset = 0;

        // CSV���� �б�
        this.d_file = d_file;
        //lines = CSVReader.Read("CSVfiles/01_Dialogue/" + chapter + "/" + d_file.name);
        lines = CSVReader.Read("CSVfiles/01_Dialogue/" + d_file.name);

        //Debug.Log("[lines Count]: " + lines.Count);

        getLineQueue();

        // ĳ���� �ʱ�ȭ
        speakingC = null;
        characterNum = 0;
        //characters = new Character[3] { null, null, null };

        openCloseDialogue();

        readlines();
    }

    // ���� ���� ���� �޼���
    public void nextDialogue()
    {
        destroyObjects();
        nowLine++;
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

        mission = 0;
    }

    // ���� �б�
    void readlines()
    {
        try
        {
            for (int i = 0; i < lineoffset + 1; i++)
                line = lineQueue.Dequeue();

            int.TryParse(line["Type"].ToString(), out type);

            // ��ȭ�� ��Ÿ�� �ܼ�/�ι�
            checkInfos();

            // ĳ���� ���� ���� ��
            operateCharacter();

            // ��� �� �̸� ����
            dialogueBox.line = line["Dialogue"].ToString().Replace("|", "\n");
            dialogueBox.c_name = getCharacterName(speakingC);
            dialogueLog.addLog(dialogueBox.c_name, dialogueBox.line);
            dialogueBox.showline();

            // ��信 ���� ������ ���ӵȴٸ�
            int tmpLineOffset = 0;
            int.TryParse(line["LineOffset"].ToString(), out tmpLineOffset);
            lineoffset = tmpLineOffset;

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
        catch (ArgumentException e)
        {
            Debug.Log("[Argument Error]:" + e);
        }
        catch (InvalidOperationException e)
        {
            Debug.Log("======CSV ����=======");
            destroyObjects();
            openCloseDialogue();

            // �α� �����
        }
    }

    void checkInfos()
    {
        if (line["PrivisoId"].ToString() != "")
        {
            string privisoId = line["PrivisoId"].ToString();
            int idx = int.Parse(line["PrivisoIdx"].ToString());
            GameData.Instance.addPriviso(privisoId, idx);

            //GameObject.Find("Info").GetComponentInChildren<TextMeshProUGUI>().text = "�ܼ� <color=blue>" + GameData.Instance.getPriviso(privisoId).name + "</color> �����ͺ��̽� ����";
        }

        if (line["FigureId"].ToString() != "")
        {
            string figureId = line["FigureId"].ToString();
            int idx = int.Parse(line["FigureIdx"].ToString());
            GameData.Instance.addFigures(figureId, idx);
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

    // �� �� �̻�����
    void operateCharacter()
    {
        /**
        if (line["SpeakingId"].ToString() != "")
            speakingC = line["SpeakingId"].ToString();

        int checkNum;
        int tmp_cnum = (int.TryParse(line["CharacterNum"].ToString(), out checkNum)) ? checkNum : characterNum;

        switch (tmp_cnum)
        {
            case 1:
                // ĳ���� �� ����
                if (tmp_cnum != characterNum)
                {
                    int idx = Array.FindIndex(characters, e => e.id == line["C1"].ToString());

                    // �߰���
                    if (idx != -1)
                    {
                        characters[idx].moveMiddle();
                        int illust;
                        if (int.TryParse(line["C1Illust"].ToString(), out illust))
                            characters[idx].setIllust(illust);
                        characters[1] = characters[idx];
                    }
                    // ���߰���
                    else
                    {
                        Character c1 = ObjectPool.Instance.CharacterQueue.Dequeue().GetComponent<Character>();
                        //c1.setCharacter(line["C1"].ToString(),
                                        getCharacterName(line["C1"].ToString()),
                                        int.Parse(line["C1Illust"].ToString())
                                        );
                        c1.gameObject.SetActive(true);
                        c1.setPosition(new Vector3(0, 1));

                        characters[1] = c1;
                    }

                    // ������ ����
                    ObjectPool.Instance.CharacterQueue.Enqueue(characters[0].gameObject);
                    characters[0].gameObject.SetActive(false);
                    ObjectPool.Instance.CharacterQueue.Enqueue(characters[2].gameObject);
                    characters[2].gameObject.SetActive(false);

                }
                // ĳ���� �� �״��
                else
                {
                    // ������������ ���ø� �ٲٱ�
                    if (line["C1"].ToString() != "")
                        characters[1].setCharacter(line["C1"].ToString(),
                                        getCharacterName(line["C1"].ToString()),
                                        int.Parse(line["C1Illust"].ToString())
                                        );
                    else
                    {
                        int illust;
                        if (int.TryParse(line["C1Illust"].ToString(), out illust))
                            characters[1].setIllust(illust);
                    }
                }
                characterNum = tmp_cnum;
                break;

            case 2:
                // ���� �ٸ�
                if (tmp_cnum != characterNum)
                {
                    int idx1 = Array.FindIndex(characters, e => e.id == line["C1"].ToString());
                    int idx2 = Array.FindIndex(characters, e => e.id == line["C2"].ToString());

                    // �� �� ���߰���
                    if (idx1 == -1 && idx2 == -1)
                    {
                        Character c1 = ObjectPool.Instance.CharacterQueue.Dequeue().GetComponent<Character>();
                        c1.setCharacter(line["C1"].ToString(),
                                        getCharacterName(line["C1"].ToString()),
                                        int.Parse(line["C1Illust"].ToString())
                                        );
                        c1.gameObject.SetActive(true);
                        c1.setPosition(new Vector3(-4, 1));
                        characters[0] = c1;

                        Character c2 = ObjectPool.Instance.CharacterQueue.Dequeue().GetComponent<Character>();
                        c2.setCharacter(line["C2"].ToString(),
                                        getCharacterName(line["C2"].ToString()),
                                        int.Parse(line["C2Illust"].ToString())
                                        );
                        c2.gameObject.SetActive(true);
                        c2.setPosition(new Vector3(4, 1));
                        characters[2] = c2;

                        // ������ ����
                        ObjectPool.Instance.CharacterQueue.Enqueue(characters[1].gameObject);
                        characters[1].gameObject.SetActive(false);
                    }
                    else
                    {
                        // c1�� �߰�
                        if (idx1 != -1)
                        {
                            Debug.Log("c1 �߰�: " + idx1);
                            characters[idx1].moveLeft();
                            int illust;
                            if (int.TryParse(line["C1Illust"].ToString(), out illust))
                                characters[idx1].setIllust(illust);
                            characters[0] = characters[idx1];
                            characters[idx1] = c_prb;
                        }
                        // c2�� �߰�
                        else if (idx2 != -1)
                        {
                            characters[idx2].moveRight();
                            int illust;
                            if (int.TryParse(line["C2Illust"].ToString(), out illust))
                                characters[idx2].setIllust(illust);
                            characters[2] = characters[idx2];
                            characters[idx2] = c_prb;
                        }

                    }

                }
                // ���� ����
                else
                {
                    if (line["C1"].ToString() != "" && line["C2"].ToString() != "")
                    {

                    }
                    else
                    {
                        if (line["C1"].ToString() != "")
                        {
                            int idx = Array.FindIndex(characters, e => e.id == line["C1"].ToString());

                            // �߰�
                            if (idx != -1)
                            {
                                characters[idx].moveLeft();
                                int illust;
                                if (int.TryParse(line["C1Illust"].ToString(), out illust))
                                    characters[idx].setIllust(illust);
                                characters[0] = characters[idx];
                                characters[idx] = c_prb;
                            }
                            else
                            {
                                Character c1 = ObjectPool.Instance.CharacterQueue.Dequeue().GetComponent<Character>();
                                c1.setCharacter(line["C1"].ToString(),
                                                getCharacterName(line["C1"].ToString()),
                                                int.Parse(line["C1Illust"].ToString())
                                                );
                                c1.gameObject.SetActive(true);
                                c1.setPosition(new Vector3(-4, 1));
                                characters[0] = c1;
                            }

                        }
                        else if (line["C2"].ToString() != "")
                        {
                            int idx = Array.FindIndex(characters, e => e.id == line["C2"].ToString());

                            // �߰�
                            if (idx != -1)
                            {
                                characters[idx].moveLeft();
                                int illust;
                                if (int.TryParse(line["C2Illust"].ToString(), out illust))
                                    characters[idx].setIllust(illust);
                                characters[2] = characters[idx];
                                characters[idx] = c_prb;
                            }
                            else
                            {
                                Character c2 = ObjectPool.Instance.CharacterQueue.Dequeue().GetComponent<Character>();
                                c2.setCharacter(line["C2"].ToString(),
                                                getCharacterName(line["C1"].ToString()),
                                                int.Parse(line["C1Illust"].ToString())
                                                );
                                c2.gameObject.SetActive(true);
                                c2.setPosition(new Vector3(4, 1));
                                characters[0] = c2;
                            }
                        }
                    }
                }

                // ������ ����
                ObjectPool.Instance.CharacterQueue.Enqueue(characters[1].gameObject);
                characters[1].gameObject.SetActive(false);

                characterNum = tmp_cnum;
                break;

            default:
                // ĳ���� �� �����
                break;

        }
        **/
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

            int answer_offset;
            a.offset = int.TryParse(answer["Nextline"].ToString(), out answer_offset) ? answer_offset : 0;

            a.gameObject.SetActive(true);
        }

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
