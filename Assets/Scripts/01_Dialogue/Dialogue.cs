using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Dialogue : MonoBehaviour
{
    [Header("������Ʈ")]
    [SerializeField] TextMeshProUGUI name_b;
    [SerializeField] TextMeshProUGUI line_b;

    [Header("���")]
    public string line;
    [SerializeField] string[] paragraphs;
    public string c_name;

    [Header("�ڷ�ƾ")]
    [SerializeField] Coroutine typing;

    void Start()
    {
        // ���� ���� ������
        paragraphs = line.Split('\n');
    }

    public void showline()
    {
        // ���� �������϶� ���� �����ؾ���

        DialogueManager.Instance.isLineEnd = false;
        name_b.text = c_name;
        typing = StartCoroutine(Typing(line, 0.07f));

        /**
        // Dotween ����
        line_b.text = line;
        line_b.maxVisibleCharacters = 0;
        DOTween.To(x => line_b.maxVisibleCharacters = (int)x, 0f, line_b.text.Length, 1f);
        **/
    }

    // ĭ ��ũ�Ѹ� - ���� �ʿ��ϸ� ����

    IEnumerator Typing(string message, float speed)
    {
        for (int i = 0; i < message.Length; i++)
        {
            line_b.text = message.Substring(0, i + 1);
            yield return new WaitForSeconds(speed);
        }

        DialogueManager.Instance.isLineEnd = true;
    }

    public void showAllLine()
    {
        line_b.text = line;
    }

    void stopTyping()
    {
        StopCoroutine(typing);
    }

    public void callStopTyping()
    {
        stopTyping();
        typing = null;
        showAllLine();

        DialogueManager.Instance.isLineEnd = true;
    }
}
