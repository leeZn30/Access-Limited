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

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addLog(string name, string line)
    {
        GameObject newLog = Instantiate(dialogue, content.transform);

        TextMeshProUGUI[] texts = newLog.GetComponentsInChildren<TextMeshProUGUI>();
        texts[0].text = name;
        texts[1].text = line;

    }

}
