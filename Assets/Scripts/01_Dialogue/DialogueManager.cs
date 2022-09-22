using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class DialogueManager : Singleton<DialogueManager>
{
    [Header("CSV 파일")]
    [SerializeField] TextAsset d_file;

    [Header("CSV 출력")]
    public List<Dictionary<string, object>> lines;
    public Dictionary<string, object> line;
    public int now_line;

    [Header("대화 타입")]
    public int type = 0;

    [Header("오브젝트")]
    [SerializeField] Button reversebtn;
    [SerializeField] Dialogue dialogueBox;

    void Awake()
    {
        lines = CSVReader.Read("CSVfiles/01_Dialogue/" + d_file.name);
        CharacterTable.setTable();

        now_line = 0;
        reversebtn.onClick.AddListener(reverseDialogue);
    }


    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
        {
            nextDialogue();
        }
        
    }


    void nextDialogue()
    {
        if (type == 0)
        {
            if (now_line != lines.Count - 1)
            {
                now_line++;
                dialogueBox.readlines();
            }
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

    public string getCharacterName(int id)
    {
        return CharacterTable.cTable[id].ToString();
    }

}
