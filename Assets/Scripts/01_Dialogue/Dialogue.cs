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
    [SerializeField] Sequence typing;

    void Start()
    {
        // ���� ���� ������
        paragraphs = line.Split('\n');
    }

    public void showline()
    {
        // ���� �������϶� ���� �����ؾ��� -> ����

        DialogueManager.Instance.isLineEnd = false;
        name_b.text = c_name;
        typing = dotTyping(line.Length * 0.1f).SetEase(Ease.Linear);    
    }

    Sequence dotTyping(float speed)
    {
        line_b.text = line;
        line_b.maxVisibleCharacters = 0;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(DOTween.To(x => line_b.maxVisibleCharacters = (int)x, 0f, line_b.text.Length, speed));

        sequence.AppendCallback(() => {
            DialogueManager.Instance.isLineEnd = true;
        });

        sequence.Play();

        return sequence;

    }

    // ĭ ��ũ�Ѹ� - ���� �ʿ��ϸ� ����

    public void showAllLine()
    {
        //line_b.text = line;
        line_b.maxVisibleCharacters = line.Length;
    }

    void stopTyping()
    {
        //StopCoroutine(typing);
        typing.Kill();
    }

    public void callStopTyping()
    {
        stopTyping();
        typing = null;
        showAllLine();

        DialogueManager.Instance.isLineEnd = true;
    }
}
