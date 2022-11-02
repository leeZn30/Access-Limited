using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Threading.Tasks;
using System.Threading;

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
        // 문단 여러개일때 엔터 적용해야함

        DialogueManager.Instance.isLineEnd = false;
        name_b.text = c_name;
        //typing = StartCoroutine(Typing(line, 0.07f));
        typing = dotTyping(line.Length * 0.1f);

        /**
        // Dotween 버전
        line_b.text = line;
        line_b.maxVisibleCharacters = 0;
        DOTween.To(x => line_b.maxVisibleCharacters = (int)x, 0f, line_b.text.Length, 1f);
        **/
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

    // 칸 스크롤링 - 추후 필요하면 개발

    IEnumerator Typing(string message, float speed)
    {
        // 색상, 볼드체 등 처리

        for (int i = 0; i < message.Length; i++)
        {
            line_b.text = message.Substring(0, i + 1);
            yield return new WaitForSeconds(speed);
        }

        DialogueManager.Instance.isLineEnd = true;
    }

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
