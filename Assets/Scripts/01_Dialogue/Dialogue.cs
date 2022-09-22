using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    [Header("오브젝트")]
    [SerializeField] TextMeshProUGUI name_b;
    [SerializeField] TextMeshProUGUI line_b;

    [Header("lines")]
    [SerializeField] List<Dictionary<string, object>> lines;
    [SerializeField] int now_line;


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

        // type 전달
        DialogueManager.Instance.type = int.Parse(line["Type"].ToString());
    }

}
