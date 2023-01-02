using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DialogueManager : Singleton<DialogueManager>
{
    [Header("씬 정보")]
    [SerializeField] int chapter;
    [SerializeField] int day;
    /*
     * mode
     * 0: 기본 대화
     * 1: 오브젝트 클릭 대화
     */
    public int mode;
    // dialogueUI가 열려있는지 > 이게 열려있어야 모든 대화에 관련된 기능 가능
    public bool isEnable = false;
    [SerializeField] bool isDLogOpen = false;

    [Header("대화 정보")]
    public int nowTurn; // 구현할때만 public
    [SerializeField] int nextTurn;
    [SerializeField] int lineofTurn = 0;
    [SerializeField] int type = 0;
    public int mission = 0;
    public bool missionRunning = false;
    public bool isLineEnd = false;

    [Header("등장 캐릭터")]
    [SerializeField] string speakingC;
    [SerializeField] List<Character> characters = new List<Character>();

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
        day = GameData.Instance.day;

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
            {
                dialogueBox.callStopTyping();

                /**
                for(int i = 0; i < characters.Count; i++)
                {
                    Character c = characters[i];
                    if (c != null)
                    {
                        Debug.Log(c.id + ": " + i);
                        c.stopMovingAndPlace(i);
                    }
                }
                **/
            }
            else if (mission != 0 && !missionRunning)
                doMission(type);
        }

        if (Input.GetKeyDown(KeyCode.L) && isEnable)
        {
            openCloseDialogueLog();
        }

    }

    // 다이얼로그 매니저 설정
    public void resetDialogueManager(TextAsset d_file, int mode = 0)
    {
        this.mode = mode;
        string path = "";

        switch (mode)
        {
            case 0:
                path = string.Format("CSVfiles/01_Dialogue/{0}/{1}",
                                            chapter,
                                            d_file.name
                                            );
                break;
            case 1:
                path = string.Format("CSVfiles/01_Dialogue/{0}/Day{1}/Map{2}/{3}",
                                            chapter,
                                            day,
                                            GameManager.Instance.map,
                                            d_file.name
                                            );
                break;
            default:
                break;
        }
        // CSV파일 읽기
        this.d_file = d_file;
        lines = CSVReader.Read(path);
        //lines = CSVReader.Read("CSVfiles/01_Dialogue/" + d_file.name);

        speakingC = null;
        characters = new List<Character> { null, null, null };

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

        if (type == 1)
            destroyAnswers();

        readlines();
    }

    // 대화가 넘어갈때 지우거나 초기화하는 것들
    void destroyAnswers()
    {
        GameObject[] answer_objs = GameObject.FindGameObjectsWithTag("Answer");
        foreach (GameObject answer in answer_objs)
        {
            answer.SetActive(false);
            ObjectPool.Instance.AnswerQueue.Enqueue(answer.gameObject);
        }
    }

    void destroyCharacters()
    {
        GameObject[] character_objs = GameObject.FindGameObjectsWithTag("Character");
        foreach (GameObject character in character_objs)
        {
            character.SetActive(false);
            ObjectPool.Instance.CharacterQueue.Enqueue(character.gameObject);
        }
    }

    // 특정 턴으로 이동
    public void goTurn(int startTurn)
    {
        destroyAnswers();
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
            finishDialogue();
            return;
        }

        foreach (Dictionary<string, object> element in turnLines)
        {
            lineQueue.Enqueue(element);
        }

        lineofTurn = -1;
        readlines();

        // 여기서 캐릭터 오퍼레이터 -> 턴마다 캐릭터 고정 + 일러스트만 바꾸기 (따로 함수) => readline마다
        setTurnCharacter();
        setcharacterGray();
    }

    // 턴의 라인 읽기
    void readlines()
    {
        lineofTurn++;

        line = lineQueue.Dequeue();

        int.TryParse(line["Type"].ToString(), out type); // default = 0

        // SpeakingId
        if (line["SpeakingId"].ToString() != "")
            speakingC = line["SpeakingId"].ToString();

        // Dialogue
        dialogueBox.line = line["Dialogue"].ToString().Replace("@", "\n");
        dialogueBox.c_name = getCharacterName(speakingC);
        //dialogueLog.addLog(dialogueBox.c_name, dialogueBox.line);
        dialogueBox.showline();

        // 대화에 나타난 단서/인물
        checkInfos();

        // nextTurn
        int tmpNext;
        nextTurn = int.TryParse(line["NextTurn"].ToString(), out tmpNext) ? tmpNext : nextTurn;

        // 캐릭터 상태 -> 턴 시작이 아니면...
        if (lineofTurn != 0)
            setCharacterIllust();
        setcharacterGray();

        // 배경 있다면 전달
        int BGid;
        if (int.TryParse(line["Background"].ToString(), out BGid))
            backgroundCanvas.setBackground(chapter, BGid);

        // 미션
        mission = type;

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

    // 다이얼로그 종료
    void finishDialogue()
    {
        //Debug.Log("======대사 종료=======");
        destroyCharacters();
        openCloseDialogue();
        //dialogueLog.clearLog();
        nowTurn = 0;

        if (mode == 1)
        {
            GameManager.Instance.clickedObj.afterDialogueUpdate();
            GameManager.Instance.clickedObj = null;
        }

        mode = 0;
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

    // list에 null이 들어가면 Find가 안되서 해줌
    int existCharacter(string id)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i] != null && characters[i].id == id)
                return i;
        }
        return -1;
    }

    // 코루틴 진행중일때 넘기는거 처리 필요 > 연속으로 스페이스 다다다 누를때
    void setTurnCharacter()
    {
        int idx1 = -1;
        int idx2 = -1;

        /**
        for(int i = 0; i < characters.Count; i++)
        {
            if (characters[i] != null)
                Debug.Log("[" + i + "]: " + characters[i].id);
            else
                Debug.LogFormat("[&0]: Null", i);
        }
        **/

        List<int> deletes = new List<int>(3) { 0, 1, 2 };
        if (line["C1"].ToString() != "")
        {
            idx1 = existCharacter(line["C1"].ToString());

            if (idx1 != -1)
                deletes.Remove(idx1);
        }

        if (line["C2"].ToString() != "")
        {
            idx2 = existCharacter(line["C2"].ToString());

            if (idx2 != -1)
                deletes.Remove(idx2);
        }

        // 필요없는 캐릭터 오브젝트 먼저 지우기
        foreach (int i in deletes)
            destroyCharacter(i);

        Character c1 = null;
        Character c2 = null;
        // characters 바꾸면서 이동 및 추가
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
                characters[2] = c2;

            characters[1] = null;
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

            characters[0] = null;
            characters[2] = null;
        }
    }

    void setcharacterGray()
    {

        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i] != null && characters[i].id != speakingC)
            {
                characters[i].GetComponentInChildren<SpriteRenderer>().color = Color.gray;
            }
            if (characters[i] != null && characters[i].id == speakingC)
            {
                characters[i].GetComponentInChildren<SpriteRenderer>().color = Color.white;
            }

        }
    }

    void setCharacterIllust()
    {
        int illust1;
        if (int.TryParse(line["C1illust"].ToString(), out illust1))
        {
            if (characters[0] != null)
                characters[0].setIllust(illust1);
            else
                characters[1].setIllust(illust1);

        }
        int illust2;
        if (int.TryParse(line["C2illust"].ToString(), out illust2))
            characters[2].setIllust(illust2);
    }

    Character createCharacter()
    {
        Character c = ObjectPool.Instance.CharacterQueue.Dequeue().GetComponent<Character>();
        c.gameObject.SetActive(true);

        return c;
    }

    void destroyCharacter(int idx)
    {
        if (characters[idx] != null)
        {
            //Debug.LogFormat("Destroy Character: {0}", characters[idx].c_name);
            ObjectPool.Instance.CharacterQueue.Enqueue(characters[idx].gameObject);
            characters[idx].gameObject.SetActive(false);
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
        /**
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
        **/
    }
}
