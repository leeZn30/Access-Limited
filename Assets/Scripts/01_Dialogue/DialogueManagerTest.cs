using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;

public class DialogueManagerTest : Singleton<DialogueManagerTest>
{
    [Header("씬 정보")]
    [SerializeField] int chapter;
    // dialogueUI가 열려있는지 > 이게 열려있어야 모든 대화에 관련된 기능 가능
    public bool isEnable = false;
    [SerializeField] bool isDLogOpen = false;

    [Header("Dialogue 정보")]
    public int mission = 0;
    public bool missionRunning = false;
    [SerializeField] int type = 0;
    public int lineoffset;

    [Header("대사 출력")]
    public bool isLineEnd = false;

    [Header("등장 캐릭터")]
    [SerializeField] string speakingC;
    [SerializeField] int characterNum;
    [SerializeField] Character c_prb;
    [SerializeField] Character[] characters = new Character[3];

    [Header("CSV 파일")]
    [SerializeField] TextAsset d_file;
    [SerializeField] TextAsset a_file; // GameManager에 넣는게 나을 수도

    [Header("CSV 출력")]
    public List<Dictionary<string, object>> lines;
    public Dictionary<string, object> line;
    public List<Dictionary<string, object>> answers;
    Queue<Dictionary<string, object>> lineQueue = new Queue<Dictionary<string, object>>();
    public int nowLine;
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

    void getLineQueue()
    {
        lineQueue.Clear();

        foreach (Dictionary<string, object> line in lines)
        {
            lineQueue.Enqueue(line);
        }

    }

    // 구현중에만 쓸 기능
    public void goLine(int startIdx)
    {
        lineQueue.Clear();

        for (int i = startIdx; i < lines.Count; i++)
        {
            lineQueue.Enqueue(lines[i]);
        }

        readlines();
    }

    // 다이얼로그 매니저 설정
    public void resetDialogueManager(TextAsset d_file)
    {
        lineoffset = 0;

        // CSV파일 읽기
        this.d_file = d_file;
        //lines = CSVReader.Read("CSVfiles/01_Dialogue/" + chapter + "/" + d_file.name);
        lines = CSVReader.Read("CSVfiles/01_Dialogue/" + d_file.name);

        //Debug.Log("[lines Count]: " + lines.Count);

        getLineQueue();

        // 캐릭터 초기화
        speakingC = null;
        characterNum = 0;
        //characters = new Character[3] { null, null, null };

        openCloseDialogue();

        readlines();
    }

    // 다음 대사로 가는 메서드
    public void nextDialogue()
    {
        destroyObjects();
        nowLine++;
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

    // 라인 읽기
    void readlines()
    {
        try
        {
            for (int i = 0; i < lineoffset + 1; i++)
                line = lineQueue.Dequeue();

            int.TryParse(line["Type"].ToString(), out type);

            // 대화에 나타난 단서/인물
            checkInfos();

            // 캐릭터 관련 전달 값
            operateCharacter();

            // 대사 및 이름 전달
            dialogueBox.line = line["Dialogue"].ToString().Replace("|", "\n");
            dialogueBox.c_name = getCharacterName(speakingC);
            dialogueLog.addLog(dialogueBox.c_name, dialogueBox.line);
            dialogueBox.showline();

            // 대답에 따른 반응이 연속된다면
            int tmpLineOffset = 0;
            int.TryParse(line["LineOffset"].ToString(), out tmpLineOffset);
            lineoffset = tmpLineOffset;

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
        catch (ArgumentException e)
        {
            Debug.Log("[Argument Error]:" + e);
        }
        catch (InvalidOperationException e)
        {
            Debug.Log("======CSV 종료=======");
            destroyObjects();
            openCloseDialogue();

            // 로그 지우기
        }
    }

    void checkInfos()
    {
        if (line["PrivisoId"].ToString() != "")
        {
            string privisoId = line["PrivisoId"].ToString();
            int idx = int.Parse(line["PrivisoIdx"].ToString());
            GameData.Instance.addPriviso(privisoId, idx);

            //GameObject.Find("Info").GetComponentInChildren<TextMeshProUGUI>().text = "단서 <color=blue>" + GameData.Instance.getPriviso(privisoId).name + "</color> 데이터베이스 갱신";
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
            case 1: // 선택지
                getAnswers();
                break;

            case 2: // 증거 제출
                break;

            default:
                break;
        }
    }

    // 왜 더 이상해짐
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
                // 캐릭터 수 변함
                if (tmp_cnum != characterNum)
                {
                    int idx = Array.FindIndex(characters, e => e.id == line["C1"].ToString());

                    // 발견함
                    if (idx != -1)
                    {
                        characters[idx].moveMiddle();
                        int illust;
                        if (int.TryParse(line["C1Illust"].ToString(), out illust))
                            characters[idx].setIllust(illust);
                        characters[1] = characters[idx];
                    }
                    // 못발견함
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

                    // 나머지 삭제
                    ObjectPool.Instance.CharacterQueue.Enqueue(characters[0].gameObject);
                    characters[0].gameObject.SetActive(false);
                    ObjectPool.Instance.CharacterQueue.Enqueue(characters[2].gameObject);
                    characters[2].gameObject.SetActive(false);

                }
                // 캐릭터 수 그대로
                else
                {
                    // 삭제하지말고 세팅만 바꾸기
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
                // 수가 다름
                if (tmp_cnum != characterNum)
                {
                    int idx1 = Array.FindIndex(characters, e => e.id == line["C1"].ToString());
                    int idx2 = Array.FindIndex(characters, e => e.id == line["C2"].ToString());

                    // 둘 다 못발견함
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

                        // 나머지 삭제
                        ObjectPool.Instance.CharacterQueue.Enqueue(characters[1].gameObject);
                        characters[1].gameObject.SetActive(false);
                    }
                    else
                    {
                        // c1만 발견
                        if (idx1 != -1)
                        {
                            Debug.Log("c1 발견: " + idx1);
                            characters[idx1].moveLeft();
                            int illust;
                            if (int.TryParse(line["C1Illust"].ToString(), out illust))
                                characters[idx1].setIllust(illust);
                            characters[0] = characters[idx1];
                            characters[idx1] = c_prb;
                        }
                        // c2만 발견
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
                // 수가 같음
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

                            // 발견
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

                            // 발견
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

                // 나머지 삭제
                ObjectPool.Instance.CharacterQueue.Enqueue(characters[1].gameObject);
                characters[1].gameObject.SetActive(false);

                characterNum = tmp_cnum;
                break;

            default:
                // 캐릭터 다 지우기
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
