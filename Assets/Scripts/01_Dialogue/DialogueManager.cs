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
    // dialogueUI가 열려있는지 > 이게 열려있어야 모든 대화에 관련된 기능 가능
    public bool isEnable = false;
    [SerializeField] bool isDLogOpen = false;

    [Header("Dialogue 정보")]
    public int mission = 0;
    [SerializeField] int now_turn;
    [SerializeField] int type = 0;
    public int chosen_line;
    [SerializeField] bool nextEnd = false;

    [Header("대사 출력")]
    public bool isLineEnd = false;

    [Header("등장 캐릭터")]
    [SerializeField] int speakingC;
    [SerializeField] int characterNum;
    [SerializeField] int[] ids = new int[3] {-1, -1, -1};
    [SerializeField] Character[] characters = new Character[3] {null, null, null};

    [Header("CSV 파일")]
    [SerializeField] TextAsset d_file;
    [SerializeField] TextAsset p_file;
    [SerializeField] TextAsset a_file; // GameManager에 넣는게 나을 수도

    [Header("CSV 출력")]
    public List<Dictionary<string, object>> lines;
    public Dictionary<string, object> line;
    public List<Dictionary<string, object>> answers;

    [Header("오브젝트")]
    [SerializeField] GameObject dialogueUIs;
    [SerializeField] Dialogue dialogueBox;
    [SerializeField] GameObject answer_box;
    [SerializeField] DialogueLog dialogueLog;
    [SerializeField] Background backgroundCanvas;

    [Header("사용 프리팹")]
    [SerializeField] Answer answer_prb;
    [SerializeField] Character character_prb;

    void Awake()
    {
        chapter = GameData.Instance.chapter;

        // 캐릭터 해시테이블 (후에는 게임 최초 실행시로 변경)
        CharacterTable.setTable();

        // 현재 출력되는 턴과 해당 라인
        now_turn = 0;
        chosen_line = 0;

        // 배경 찾기
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

    // 다이얼로그 매니저 설정
    public void setDialogueManager(TextAsset d_file)
    {
        // CSV파일 읽기
        this.d_file = d_file;
        lines = CSVReader.Read("CSVfiles/01_Dialogue/" + chapter + "/" + d_file.name);

        openCloseDialogue();

        readlines();
    }

    // 다이얼로그UI 키고끄기
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

    // 다음 대사로 가는 메서드
    public void nextDialogue()
    {
        if (!nextEnd)
        {
            destroyObjects();
            now_turn++;
            readlines();
        }
    }

    // 대화가 넘어갈때 지우거나 초기화하는 것들
    void destroyObjects()
    {
        GameObject[] answer_objs = GameObject.FindGameObjectsWithTag("Answer");
        foreach (GameObject answer in answer_objs)
        {
            Destroy(answer);
        }

        // 다른 Character도 찾아서 삭제 > 연쇄적
        GameObject[] character_objs = GameObject.FindGameObjectsWithTag("Character");
        foreach (GameObject character in character_objs)
        {
            Destroy(character);
        }

        mission = 0;
    }

    // 라인 읽기
    void readlines()
    {
        try
        {
            line = lines.Where(turn => turn["Turn"].ToString() == now_turn.ToString()).ToList()[chosen_line]; // 왜 int.Parse 안됨
        }
        catch (ArgumentException e)
        {
            Debug.Log("+++++++csv 종료+++++++++");
            destroyObjects();
            openCloseDialogue();
        }

        int.TryParse(line["Type"].ToString(), out type);

        // 대화에 나타난 단서/인물
        checkInfos();

        // 캐릭터 관련 전달 값
        if (line["CharacterId"].ToString() != "")
            speakingC = int.Parse(line["CharacterId"].ToString());

        if (line["CharacterNum"].ToString() != "")
            characterNum = int.Parse(line["CharacterNum"].ToString());
        operateCharacter();

        // 대사 및 이름 전달
        dialogueBox.line = line["Dialogue"].ToString();
        dialogueBox.c_name = getCharacterName(speakingC);
        dialogueLog.addLog(dialogueBox.c_name, dialogueBox.line);
        dialogueBox.showline();

        // 대답에 따른 반응이 연속된다면
        int lineoffset;
        if (int.TryParse(line["LineOffset"].ToString(), out lineoffset))
            chosen_line = lineoffset;

        // 배경 있다면 전달
        int BGid;
        if (int.TryParse(line["Background"].ToString(), out BGid))
            backgroundCanvas.setBackground(chapter, BGid);

        // 미션
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
        // 좀 많이 맘에 안듦
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

                // 말안하면 색상 낮추기
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
