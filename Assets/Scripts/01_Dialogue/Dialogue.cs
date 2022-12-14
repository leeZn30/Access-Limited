using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Dialogue : MonoBehaviour
{
    [Header("오브젝트")]
    [SerializeField] TextMeshProUGUI name_b;
    [SerializeField] TextMeshProUGUI line_b;

    [Header("대사")]
    public string line;
    [SerializeField] string[] paragraphs;
    public string c_name;

    [Header("코루틴")]
    [SerializeField] Sequence typing;

    void Start()
    {
        // 문단 별로 나누기
        paragraphs = line.Split('\n');
    }

    public void showline()
    {
        DialogueManager.Instance.isLineEnd = false;
        name_b.text = c_name;

        typing = dotTyping();
    }

    string patternLine()
    {
        string pattern = @"\<[^>]*\>";
        string patterendLine = Regex.Replace(line, pattern, "");

        return patterendLine;
    }

    Sequence dotTyping()
    {
        line_b.text = line;
        line_b.maxVisibleCharacters = 0;

        int length = patternLine().Length;
        float speed = length * 0.07f;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(DOTween.To(x => line_b.maxVisibleCharacters
                                   = (int)x, 0f, length, speed)
                                   .SetEase(Ease.Linear)
                        );

        sequence.AppendCallback(() =>
        {
            DialogueManager.Instance.isLineEnd = true;
        });

        sequence.Play();

        return sequence;

    }

    public void showAllLine()
    {
        line_b.maxVisibleCharacters = patternLine().Length;
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

    public void closeNameBox()
    {
        if (name_b.gameObject.transform.parent.gameObject.activeSelf == true)
        {
            name_b.gameObject.transform.parent.gameObject.SetActive(false);
        }
    }

    public void showNameBox()
    {
        if (!name_b.transform.parent.gameObject.activeSelf)
            transform.GetChild(0).gameObject.SetActive(true);
    }

    public void hideNameBox()
    {
        if (name_b.transform.parent.gameObject.activeSelf)
            transform.GetChild(0).gameObject.SetActive(false);
    }
}
