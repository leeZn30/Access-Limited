using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiaryListRayout : MonoBehaviour
{
    [Header("챕터")]
    [SerializeField] int chapter;
    string text = "Chapter. ";
    [SerializeField] int selectChapter;

    [Header("오브젝트")]
    [SerializeField] GameObject chapterObj;
    [SerializeField] TextMeshProUGUI chapterNumtxt;

    void Start()
    {
        chapter = DatabaseManager.Instance.chapter;
        selectChapter = chapter;

        chapterNumtxt.text = text + chapter;

        chapterObj.GetComponent<Button>().onClick.AddListener(delegate { DatabaseManager.Instance.pickDiary(selectChapter); });

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (selectChapter - 1 > 0)
            {
                StartCoroutine(moveLeft());
                selectChapter--;
                chapterNumtxt.text = text + selectChapter;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (selectChapter + 1 <= chapter)
            {
                StartCoroutine(moveRight());
                selectChapter++;
                chapterNumtxt.text = text + selectChapter;

            }
        }
    }


    // 움직임 애니메이션 좀 많이 고쳐야함
    IEnumerator moveLeft()
    {
        float duration = 0.15f;
        float time = 0f;
        float speed = 30f;

        RectTransform rect = chapterObj.GetComponent<RectTransform>();

        Vector3 originPos = chapterObj.transform.position;
        Vector2 originWidth = rect.sizeDelta;

        while (time < duration)
        {
            time += Time.deltaTime;

            chapterObj.transform.position += Vector3.left * Time.deltaTime * speed;
            rect.sizeDelta -= new Vector2(100, 0) * Time.deltaTime * 20;
            yield return null;
        }

        rect.sizeDelta = originWidth;
        rect.position = originPos;

        yield return null;
    }

    IEnumerator moveRight()
    {
        float duration = 0.15f;
        float time = 0f;
        float speed = 30f;

        RectTransform rect = chapterObj.GetComponent<RectTransform>();

        Vector3 originPos = chapterObj.transform.position;
        Vector2 originWidth = rect.sizeDelta;

        while (time < duration)
        {
            time += Time.deltaTime;

            chapterObj.transform.position += Vector3.right * Time.deltaTime * speed;
            rect.sizeDelta -= new Vector2(100, 0) * Time.deltaTime * 20;
            yield return null;
        }

        rect.sizeDelta = originWidth;
        rect.position = originPos;

        yield return null;
    }

}
