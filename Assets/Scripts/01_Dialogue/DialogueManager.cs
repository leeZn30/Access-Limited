using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class DialogueManager : Singleton<DialogueManager>
{
    [Header("Dialogue 정보")]
    [SerializeField] string dialogueId;
    public int mission = 0;
    public int type = 0;
    public int characterNum;

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

    void Awake()
    {
        // CSV파일 읽기
        lines = CSVReader.Read("CSVfiles/01_Dialogue/" + d_file.name);


        // 캐릭터 해시테이블 (후에는 게임 최초 실행시로 변경)
        CharacterTable.setTable();

        // 지금 라인
        now_line = 0;

        // 버튼 설정
        reversebtn.onClick.AddListener(reverseDialogue);
    }


    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
        {
            if (type == 0 && mission == 0)
                nextDialogue();
        }

    }


    public void nextDialogue()
    {

        if (now_line != lines.Count - 1)
        {
            now_line++;
            dialogueBox.readlines();
        }
    }

    void reverseDialogue()
    {
        // now_line이 0이고 전 대화가 사용자의 답변 요구를 하지 않아야 역행 가능
        if (now_line > 0 && int.Parse(lines[now_line - 1]["Type"].ToString()) != 1)
        {
            now_line--;
            dialogueBox.readlines();
        }
    }

    public void getAnswers()
    {
        answers = CSVReader.Read("CSVfiles/01_Dialogue/" + a_file.name).Where(answer => answer["Id"].ToString() == answerId).ToList();
        dialogueBox.createAnswer();

    }


    public string getCharacterName(int id)
    {
        return CharacterTable.cTable[id].ToString();
    }

}
