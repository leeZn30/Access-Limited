using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueLog : MonoBehaviour
{
    [Header("스크롤뷰 오브젝트")]
    [SerializeField] GameObject content;

    [Header("로그 오브젝트")]
    [SerializeField] GameObject dialogue;

    public void addLog(string name, string line)
    {
        GameObject newLog = Instantiate(dialogue, content.transform);

        TextMeshProUGUI[] texts = newLog.GetComponentsInChildren<TextMeshProUGUI>();
        texts[0].text = name;
        texts[1].text = line;

        Debug.Log("한글 깨짐 보기");

    }

}
