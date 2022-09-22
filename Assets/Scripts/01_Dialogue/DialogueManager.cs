using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class DialogueManager : Singleton<DialogueManager>
{
    [Header("Dialogue ����")]
    [SerializeField] string dialogueId;
    public int mission = 0;
    public int type = 0;
    public int characterNum;

    [Header("CSV ����")]
    [SerializeField] TextAsset d_file;
    [SerializeField] TextAsset a_file; // GameManager�� �ִ°� ���� ����

    [Header("CSV ���")]
    public List<Dictionary<string, object>> lines;
    public Dictionary<string, object> line;
    public int now_line;
    public List<Dictionary<string, object>> answers;

    [Header("Answer ����")]
    public string answerId;

    [Header("������Ʈ")]
    [SerializeField] Button reversebtn;
    [SerializeField] Dialogue dialogueBox;

    void Awake()
    {
        // CSV���� �б�
        lines = CSVReader.Read("CSVfiles/01_Dialogue/" + d_file.name);


        // ĳ���� �ؽ����̺� (�Ŀ��� ���� ���� ����÷� ����)
        CharacterTable.setTable();

        // ���� ����
        now_line = 0;

        // ��ư ����
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
        // now_line�� 0�̰� �� ��ȭ�� ������� �亯 �䱸�� ���� �ʾƾ� ���� ����
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
