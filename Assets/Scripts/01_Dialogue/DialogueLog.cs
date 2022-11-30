using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueLog : MonoBehaviour
{
    [Header("��ũ�Ѻ� ������Ʈ")]
    [SerializeField] GameObject content;

    [Header("�α� ������Ʈ")]
    [SerializeField] GameObject dialogue;

    public void addLog(string name, string line)
    {
        GameObject newLog = Instantiate(dialogue, content.transform);

        TextMeshProUGUI[] texts = newLog.GetComponentsInChildren<TextMeshProUGUI>();
        texts[0].text = name;
        texts[1].text = line;

        Debug.Log("�ѱ� ���� ����");

    }

}
