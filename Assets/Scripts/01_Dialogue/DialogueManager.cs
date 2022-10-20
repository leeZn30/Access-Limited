using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : Singleton<DialogueManager>
{
    [Header("�� ����")]
    [SerializeField] int chapter;
    public bool isEnable = true;

    [Header("Dialogue ����")]
    public int mission = 0;
    public int now_turn;
    public int type = 0;
    public int chosen_line;
    [SerializeField] bool nextEnd = false;

    [Header("��� ���")]
    public bool isLineEnd = false;

    [Header("���� ĳ����")]
    public int characterNum;
    [SerializeField] Character[] characters = new Character[3] {null, null, null};

    [Header("CSV ����")]
    [SerializeField] TextAsset d_file;
    [SerializeField] TextAsset a_file; // GameManager�� �ִ°� ���� ����

    [Header("CSV ���")]
    public List<Dictionary<string, object>> lines;
    public Dictionary<string, object> line;
    public List<Dictionary<string, object>> answers;

    [Header("Answer ����")]
    public string answerId;

    [Header("������Ʈ")]
    [SerializeField] Button reversebtn;
    [SerializeField] Button databasebtn;
    [SerializeField] Dialogue dialogueBox;
    [SerializeField] GameObject answer_box;
    [SerializeField] DialogueLog dialogueLog;
    [SerializeField] Background backgroundCanvas;

    [Header("��� ������")]
    [SerializeField] Answer answer_prb;
    [SerializeField] Character character_prb;

    [SerializeField] bool isDLogOpen = false;

    void Awake()
    {
        chapter = GameData.Instance.chapter;

        // CSV���� �б�
        lines = CSVReader.Read("CSVfiles/01_Dialogue/" + chapter + "/" + d_file.name);

        // ĳ���� �ؽ����̺� (�Ŀ��� ���� ���� ����÷� ����)
        CharacterTable.setTable();

        // ���� ��µǴ� �ϰ� �ش� ����
        now_turn = 0;
        chosen_line = 0;

        // ��ư ����
        reversebtn.onClick.AddListener(previousDialogue);
        databasebtn.onClick.AddListener(delegate { DatabaseManager.Instance.openPopup(); });

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
            // �̼� ���� & ���� �� ���
            if (isEnable)
            {
                if (mission == 0 && isLineEnd)
                    nextDialogue();
                else if (!isLineEnd)
                    dialogueBox.callStopTyping();
            }
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            openCloseDialogueLog();
        }

    }

    // ���̾�α� �Ŵ��� ����
    public void setDialogueManager()
    {
        chapter = GameData.Instance.chapter;
    }

    public void nextDialogue()
    {
        if (!nextEnd)
        {
            destoryObjects();
            now_turn++;
            readlines();
        }
    }

    void previousDialogue()
    {
        // now_turn�� 0�̰� �� ��ȭ�� Ư�� ��ȭ(type != 0)�� �ƴϾ�� ���� ����
        // ����� ��� Ÿ���� ���� ������, �ε���[0]���� ����
        if (now_turn > 0 && int.Parse(lines.Where(turn => turn["Turn"].ToString() == (now_turn - 1).ToString()).ToList()[0]["Type"].ToString()) == 0)
        {
            destoryObjects();
            now_turn--;
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
        line = lines.Where(turn => turn["Turn"].ToString() == now_turn.ToString()).ToList()[chosen_line]; // �� int.Parse �ȵ�

        type = int.Parse(line["Type"].ToString());
        answerId = line["AnswerId"].ToString();

        // ��� �� �̸� ����
        dialogueBox.line = line["Dialogue"].ToString();
        dialogueBox.c_name = getCharacterName(int.Parse(line["CharacterId"].ToString()));
        dialogueLog.addLog(dialogueBox.c_name, dialogueBox.line);
        dialogueBox.showline();

        // ��信 ���� ���� ���� ���� ������
        int lineoffset;
        if (int.TryParse(line["LineOffset"].ToString(), out lineoffset))
            chosen_line = lineoffset;


        // ��� �ִٸ� ����
        int BGid;
        if (int.TryParse(line["Background"].ToString(), out BGid))
            backgroundCanvas.setBackground(chapter, BGid);

        // ĳ���� ���� ���� ��
        characterNum = int.Parse(line["CharacterNum"].ToString());
        // �� ���� ���� �ȵ�
        switch (characterNum)
        {
            case 1:
                Character character = Instantiate(character_prb, new Vector3(0, 1), Quaternion.identity);
                character.id = int.Parse(line["MCharacter"].ToString());
                character.c_name = getCharacterName(character.id);
                character.illust_num = getCharacterIllustNum(character.id);
                character.now_illust = int.Parse(line["MCIllust"].ToString());

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

            int answer_offset;
            answer_prb.offset = int.TryParse(answer["Nextline"].ToString(), out answer_offset)? answer_offset:0;

            Instantiate(answer_prb, answer_box.transform);
        }

    }

    void getAnswers()
    {
        if (a_file != null)
        {
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
            isEnable = false;
        }
        else
        {
            dialogueLog.gameObject.SetActive(false);
            isDLogOpen = false;
            isEnable = true;
        }
    }
}
