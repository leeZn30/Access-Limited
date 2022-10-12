using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : Singleton<DialogueManager>
{
    [Header("Dialogue 정보")]
    public int mission = 0;
    public int type = 0;

    [Header("등장 캐릭터")]
    public int characterNum;
    [SerializeField] Character[] characters = new Character[3] {null, null, null};


    [Header("CSV 파일")]
    [SerializeField] TextAsset d_file;
    [SerializeField] TextAsset a_file; // GameManager에 넣는게 나을 수도

    [Header("CSV 출력")]
    public List<Dictionary<string, object>> lines;
    public Dictionary<string, object> line;
    public int now_line;
    public List<Dictionary<string, object>> answers;

    [Header("Answer 정보")]
    public string answerId;

    [Header("오브젝트")]
    [SerializeField] Button reversebtn;
    [SerializeField] Dialogue dialogueBox;
    [SerializeField] GameObject answer_box;
    [SerializeField] Answer answer_prb;
    [SerializeField] Character character_prb;

    void Awake()
    {
        // CSV파일 읽기
        lines = CSVReader.Read("CSVfiles/01_Dialogue/" + d_file.name);

        // 캐릭터 해시테이블 (후에는 게임 최초 실행시로 변경)
        CharacterTable.setTable();

        // 지금 라인
        now_line = 0;

        // 버튼 설정
        reversebtn.onClick.AddListener(previousDialogue);

        readlines();
    }

    void start()
    {
        //readlines(); 왜 여기다가 하면 안되지?
    }


    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (type == 0 && mission == 0)
                nextDialogue();
        }

    }


    public void nextDialogue()
    {

        if (now_line != lines.Count - 1)
        {
            destoryObjects();
            now_line++;
            readlines();
        }
    }

    void previousDialogue()
    {
        // now_line이 0이고 전 대화가 특수 대화(type != 0)가 아니어야 역행 가능
        if (now_line > 0 && int.Parse(lines[now_line - 1]["Type"].ToString()) == 0)
        {
            destoryObjects();

            now_line--;
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
        line = lines[now_line];

        type = int.Parse(line["Type"].ToString());
        answerId = line["AnswerId"].ToString();

        // 대사 및 이름 전달
        dialogueBox.line = line["Dialogue"].ToString();
        dialogueBox.c_name = getCharacterName(int.Parse(line["CharacterId"].ToString()));
        dialogueBox.showline();

        // 캐릭터 관련 전달 값
        characterNum = int.Parse(line["CharacterNum"].ToString());
        // 좀 많이 맘에 안듦
        switch (characterNum)
        {
            case 1:
                Character character = Instantiate(character_prb, new Vector3(0, 1), Quaternion.identity);
                character.id = int.Parse(line["MCharacter"].ToString());
                character.c_name = getCharacterName(character.id);
                character.setIllust(int.Parse(line["MCIllust"].ToString()));

                if (characters[0] != null || characters[2] != null)
                {
                    if (character.id == characters[0].id)
                    {
                        character.transform.position = new Vector3(-5, 1);
                        character.moveMiddle();
                    }
                    else if (character.id == characters[2].id)
                    {
                        character.transform.position = new Vector3(5, 1);
                        character.moveMiddle();
                    }

                }

                characters[1] = character;
                characters[0] = null;
                characters[2] = null;
                break;

            case 2:
                Character characterL = Instantiate(character_prb, new Vector3(-5, 1), Quaternion.identity);
                characterL.id = int.Parse(line["LCharacter"].ToString());
                characterL.c_name = getCharacterName(characterL.id);
                characterL.setIllust(int.Parse(line["LCIllust"].ToString()));

                Character characterR = Instantiate(character_prb, new Vector3(5, 1), Quaternion.identity);
                characterR.id = int.Parse(line["RCharacter"].ToString());
                characterR.c_name = getCharacterName(characterR.id);
                characterR.setIllust(int.Parse(line["RCIllust"].ToString()));

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

        // 플레이어 대답 요구 시
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

            Instantiate(answer_prb, answer_box.transform);
        }

    }

    void getAnswers()
    {
        answers = CSVReader.Read("CSVfiles/01_Dialogue/" + a_file.name).Where(answer => answer["Id"].ToString() == answerId).ToList();
        createAnswer();
    }


    string getCharacterName(int id)
    {
        return CharacterTable.cTable[id].ToString();
    }

}
