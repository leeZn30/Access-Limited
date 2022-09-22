using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    [Header("오브젝트")]
    [SerializeField] TextMeshProUGUI name_b;
    [SerializeField] TextMeshProUGUI line_b;
    [SerializeField] GameObject answer_box;
    [SerializeField] Answer answer_prb;

    [Header("lines")]
    [SerializeField] List<Dictionary<string, object>> lines;
    [SerializeField] int now_line;

    [Header("answers")]
    [SerializeField] List<Dictionary<string, object>> answers;

    void Start()
    {
        lines = DialogueManager.Instance.lines;
        now_line = DialogueManager.Instance.now_line;

        readlines();

    }


    public void readlines()
    {
        // now_line 갱신
        now_line = DialogueManager.Instance.now_line;

        Dictionary<string, object> line = lines[now_line];

        name_b.text = DialogueManager.Instance.getCharacterName(int.Parse(line["CharacterId"].ToString()));
        line_b.text = line["Dialogue"].ToString();

        // DialogueManager에 전달할 값
        DialogueManager.Instance.type = int.Parse(line["Type"].ToString());
        DialogueManager.Instance.answerId = line["AnswerId"].ToString();

        // 캐릭터 관련

        // 플레이어 대답 요구 시
        if (DialogueManager.Instance.type == 1)
        {
            DialogueManager.Instance.mission = 1;
            DialogueManager.Instance.getAnswers();
        }

    }


    public void createAnswer()
    {
        answers = DialogueManager.Instance.answers;

        foreach (Dictionary<string, object> answer in answers)
        {
            answer_prb.content = answer["Content"].ToString();
            answer_prb.S1 = int.Parse(answer["S1"].ToString());
            answer_prb.S2 = int.Parse(answer["S1"].ToString());

            Instantiate(answer_prb, answer_box.transform);
        }

    }

}
