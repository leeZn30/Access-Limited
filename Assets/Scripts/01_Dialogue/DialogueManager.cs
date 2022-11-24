using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;

public class DialogueManager : Singleton<DialogueManager>
{
    [Header("씬 정보")]
    [SerializeField] int chapter;
    // dialogueUI가 열려있는지 > 이게 열려있어야 모든 대화에 관련된 기능 가능
    public bool isEnable = false;
    [SerializeField] bool isDLogOpen = false;

    [Header("대화 정보")]
    public int nowTurn;
    [SerializeField] int nextTurn;
    [SerializeField] int type = 0;
    public int mission = 0;
    public bool missionRunning = false;
    public bool isLineEnd = false;

    [Header("등장 캐릭터")]
    [SerializeField] string speakingC;
    [SerializeField] int characterNum;
    [SerializeField] string[] ids = new string[3] {"", "", ""};
    [SerializeField] int[] illusts = new int[3] { -1, -1, -1 };
    [SerializeField] Character[] characters = new Character[3] {null, null, null};

    [Header("CSV 파일")]
    [SerializeField] TextAsset d_file;
    [SerializeField] TextAsset a_file; // GameManager에 넣는게 나을 수도

    [Header("CSV 출력")]
    public List<Dictionary<string, object>> lines;
    public Dictionary<string, object> line;
    public List<Dictionary<string, object>> answers;
    Queue<Dictionary<string, object>> lineQueue = new Queue<Dictionary<string, object>>();
    [SerializeField] bool skipD = false; // tmp

    [Header("오브젝트")]
    [SerializeField] GameObject dialogueUIs;
    [SerializeField] Dialogue dialogueBox;
    [SerializeField] DialogueLog dialogueLog;
    [SerializeField] Background backgroundCanvas;
    [SerializeField] PushEffect push;

    void Awake()
    {
        chapter = GameData.Instance.chapter;

        // 캐릭터 해시테이블 (후에는 게임 최초 실행시로 변경)
        CharacterTable.setTable();

        // 배경 찾기
        backgroundCanvas = GameObject.Find("BackgroundCanvas").GetComponentInChildren<Background>();
    }

    void Start()
    {
        // 다이얼로그 씬일땐 바로 들어가야 함
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

    // 다이얼로그 매니저 설정
    public void resetDialogueManager(TextAsset d_file)
    {
        // CSV파일 읽기
        this.d_file = d_file;
        lines = CSVReader.Read("CSVfiles/01_Dialogue/" + chapter + "/" + d_file.name);
        //lines = CSVReader.Read("CSVfiles/01_Dialogue/" + d_file.name);

        // 캐릭터 초기화
        speakingC = null;
        characterNum = 0;
        ids = new string[3] { "", "", "" };
        illusts = new int[3] { -1, -1, -1 };
        characters = new Character[3] { null, null, null };

        openCloseDialogue();

        nowTurn = 0;
        goTurn(nowTurn);
    }

    // 다음 대사로 가는 메서드
    public void nextDialogue()
    {
        // lineQueue가 비었으면 다음 턴으로
        if (lineQueue.Count == 0)
        {
            goTurn(nextTurn);
            return;
        }

        destroyObjects();
        readlines();
    }

    // 대화가 넘어갈때 지우거나 초기화하는 것들
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

    // 특정 턴으로 이동
    public void goTurn(int startTurn)
    {
        destroyObjects();
        nowTurn = startTurn;
        nextTurn = nowTurn + 1;
        readTurn();
    }


    // 해당 턴의 모든 라인 불러오기
    void readTurn()
    {
        lineQueue.Clear();
        List<Dictionary<string, object>> turnLines =
            lines.Where(e => int.Parse(e["Turn"].ToString()) == nowTurn).ToList();

        // 더 이상 행이 없음
        if (turnLines.Count == 0)
        {
            Debug.Log("======대사 종료=======");
            destroyObjects();
            openCloseDialogue();
            nowTurn = 0;
            return;
        }

        foreach(Dictionary<string, object> element in turnLines)
        {
            lineQueue.Enqueue(element);
        }

        // 캐릭터 처리

        readlines();
    }

    // 라인 읽기
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

        // nextTurn
        int tmpNext;
        nextTurn = int.TryParse(line["NextTurn"].ToString(), out tmpNext) ? tmpNext : nextTurn;

        // 대화에 나타난 단서/인물
        checkInfos();

        // 캐릭터 관련 전달 값


        // 배경 있다면 전달
        int BGid;
        if (int.TryParse(line["Background"].ToString(), out BGid))
            backgroundCanvas.setBackground(chapter, BGid);

        // 미션
        mission = type;

        // 연쇄 오브젝트 대화
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
            case 1: // 선택지
                getAnswers();
                break;

            case 2: // 증거 제출
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

                // 말안하면 색상 낮추기
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
        CharacterData element = CharacterTable.cTable[id] as CharacterData;
        return element.name;
    }

    int getCharacterIllustNum(string id)
    {
        CharacterData element = CharacterTable.cTable[id] as CharacterData;
        return element.illustNum;
    }

    // 다이얼로그UI 키고끄기
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
