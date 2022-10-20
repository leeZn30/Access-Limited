using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class ChapterIntro : MonoBehaviour
{
    [Header("챕터")]
    [SerializeField] int chapter;
    [SerializeField] bool isEnd = false;
    [SerializeField] string path = "CSVfiles/00_Common/ChapterIntroTexts/";

    [Header("오브젝트")]
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Transform Lbanner;
    [SerializeField] Transform Rbanner;

    [Header("CSV")]
    [SerializeField] TextAsset csv;

    [Header("내용")]
    [SerializeField] int line;
    [SerializeField] bool isLineEnd = false;
    [SerializeField] List<Dictionary<string, object>> contents = new List<Dictionary<string, object>>();

    private void Awake()
    {
        chapter = GameData.Instance.chapter;

        text = FindObjectOfType<TextMeshProUGUI>();
        csv = Resources.Load<TextAsset>(path + chapter);

        contents = CSVReader.Read(path + csv.name);
    }

    private void Start()
    {
        text.text = contents[line]["Line"].ToString();
        StartCoroutine(textFadein(text));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            nextText();
        }
        // 임시
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneChanger.goScene(2);
        }
    }

    void nextText()
    {
        if (isLineEnd && !isEnd)
        {
            line++;
            StartCoroutine(nextTextprocess());
        }
        else if (isEnd)
        {
            SceneChanger.goScene(2);
        }

    }

    IEnumerator nextTextprocess()
    {
        StartCoroutine(textFadeout(text));

        try
        {
            isLineEnd = false;
            text.text = contents[line]["Line"].ToString();

            StartCoroutine(textFadein(text));

        }
        catch (ArgumentException e)
        {
            isEnd = true;
            text.text = "Chapter." + chapter;
            text.color = Color.black;
            //StartCoroutine(textFadein(text));
            StartCoroutine(moveBanner());
        }
        
        yield return null;

        }

    // move는 애니메이션 따로 빼야할듯
    IEnumerator moveBanner()
    {
        /**
        Transform Lbanner = GameObject.Find("LeftBanner").transform;
        Transform Rbanner = GameObject.Find("RightBanner").transform;
        **/

        Vector3 targetPos = new Vector3(0, 0, 0);

        while (Lbanner.position != targetPos &&
               Rbanner.position != targetPos)
        {
            Lbanner.transform.position = Vector3.Lerp(Lbanner.position, targetPos, Time.deltaTime / 0.3f);
            Rbanner.transform.position = Vector3.Lerp(Rbanner.position, -targetPos, Time.deltaTime / 0.3f);

            yield return null;
        }

        SceneChanger.goScene(2);
        yield return null;
    }

    // 글씨 페이드인
    IEnumerator textFadein(TextMeshProUGUI text)
    {
        Color color = text.color;
        color.a = 0;
        text.color = color;

        while(color.a < 1f)
        {
            color.a += Time.deltaTime;

            text.color = color;

            yield return null;
        }

        isLineEnd = true;
        yield return null;
    }


    // 글씨 페이드아웃
    IEnumerator textFadeout(TextMeshProUGUI text)
    {
        Color color = text.color;

        while (color.a > 0f)
        {
            color.a -= Time.deltaTime;

            text.color = color;

            yield return null;
        }
        yield return null;
    }

}
