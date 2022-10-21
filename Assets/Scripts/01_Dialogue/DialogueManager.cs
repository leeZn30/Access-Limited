using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogueManager : Singleton<DialogueManager>
{
    [Header("씬 정보")]
    [SerializeField] int chapter;
    public bool isEnable = true;

    [Header("Dialogue 정보")]
    public int mission = 0;
    public int now_turn;
    public int type = 0;
    public int chosen_line;
    [SerializeField] bool nextEnd = false;

    [Header("대사 출력")]
    public bool isLineEnd = false;

    [Header("등장 캐릭터")]
    public int characterNum;
    [SerializeField] Character[] characters = new Character[3] {null, null, null};

    [Header("CSV 파일")]
    [SerializeField] TextAsset d_file;
    [SerializeField] TextAsset p_file;
    [SerializeField] TextAsset a_file; // GameManager에 넣는게 나을 수도

    [Header("CSV 출력")]
    public List<Dictionary<string, object>> lines;
    public Dictionary<string, object> line;
    public List<Dictionary<string, object>> answers;

    [Header("Answer 정보")]
    public string answerId;

    [Header("오브젝트")]
    [SerializeField] Button databasebtn;
    [SerializeField] Dialogue dialogueBox;
    [SerializeField] GameObject answer_box;
    [SerializeField] DialogueLog dialogueLog;
    [SerializeField] Background backgroundCanvas;

    [Header("사용 프리팹")]
    [SerializeField] GameObject dialogueUIs;
    [SerializeField] Answer answer_prb;
    [SerializeField] Character character_prb;

    [SerializeField] bool isDLogOpen = false;

    void Awake()
    {
        chapter = GameData.Instance.chapter;

        // 캐릭터 해시테이블 (후에는 게임 최초 실행시로 변경)
        CharacterTable.setTable();

        // 현재 출력되는 턴과 해당 라인
        now_turn = 0;
        chosen_line = 0;

        // 버튼 설정
        databasebtn.onClick.AddListener(delegate { DatabaseManager.Instance.openPopup(); });

    }

    void start()
    {
        //readlines(); 왜 여기다가 하면 안되지?
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            // 미션 성공 & 글자 다 출력
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

    // 다이얼로그 매니저 설정
    public void setDialogueManager(TextAsset d_file)
    {
        this.d_file = d_file;

        // CSV파일 읽기
        lines = CSVReader.Read("CSVfiles/01_Dialogue/" + chapter + "/" + d_file.name);
        readlines();
    }

    public void resetDialogue()
    {
        destoryObjects();
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

    // 대화 진행,역행시 지우거나 초기화하는 것들
    void destoryObjects()
    {
        // 만약 현재 타입이 n이라면 처리해주기
        switch (type)
        {
            case 1:
                // 다른 Answer도 찾아서 삭제 > 연쇄적
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

        // 다른 Character도 찾아서 삭제 > 연쇄적
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
        try
        {
            line = lines.Where(turn => turn["Turn"].ToString() == now_turn.ToString()).ToList()[chosen_line]; // 왜 int.Parse 안됨
        }
        catch (ArgumentException e)
        {
            dialogueUIs.SetActive(false);
            resetDialogue();
        }

        type = int.Parse(line["Type"].ToString());
        answerId = line["AnswerId"].ToString();

        // 대사 및 이름 전달
        dialogueBox.line = line["Dialogue"].ToString();
        dialogueBox.c_name = getCharacterName(int.Parse(line["CharacterId"].ToString()));
        dialogueLog.addLog(dialogueBox.c_name, dialogueBox.line);
        dialogueBox.showline();

        // 대답에 따른 반응이 연속이라면
        int lineoffset;
        if (int.TryParse(line["LineOffset"].ToString(), out lineoffset))
            chosen_line = lineoffset;

        // 배경 있다면 전달
        int BGid;
        if (int.TryParse(line["Background"].ToString(), out BGid))
            backgroundCanvas.setBackground(chapter, BGid);

        // 캐릭터 관련 전달 값
        characterNum = int.Parse(line["CharacterNum"].ToString());
        moveCharacter(characterNum);

        // 단서 관련 전달
        string priviso;
        if (!line["PrivisoId"].ToString().Equals(""))
        {
            priviso = line["PrivisoId"].ToString();
            GameData.Instance.addPriviso(priviso);
        }


        // 플레이어 대답 요구 시
        if (type == 1)
        {
            Instance.mission = 1;
            Instance.getAnswers();
        }
    }

    void moveCharacter(int characterNum)
    {


        // 좀 많이 맘에 안듦
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

                // 말안하면 색상 낮추기
                if (characterL.id != int.Parse(line["CharacterId"].ToString()))
                {
                    characterL.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.grey;
                }
                else if (characterR.id != int.Parse(line["CharacterId"].ToString()))
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
