using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiaryDescRayout : MonoBehaviour
{
    [Header("선택된 챕터")]
    public int selectedC;

    [Header("갱신 일기")]
    [SerializeField] TextAsset[] csvs = new TextAsset[6]; //figureAdds
    public int saveDiaryData;
    public int nowPage = 0;
    [SerializeField] List<List<Dictionary<string, object>>> dividedDiary = new List<List<Dictionary<string, object>>>(); // 개복잡;;

    [Header("오브젝트")]
    [SerializeField] GameObject[] blanks = new GameObject[4];
    [SerializeField] bool[] blankEmpties = new bool[4];

    void Start()
    {
        saveDiaryData = GameData.Instance.saveDiaryData[selectedC];
        divideDiary();
        showDiary();

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            nextPage();

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            prevPage();
    }

    void nextPage()
    {
        nowPage++;
        showDiary();
    }

    void prevPage()
    {
        nowPage--;
        showDiary();
    }

    void showDiary()
    {
        initializeEmpties();

        List<Dictionary<string, object>> pageList = dividedDiary[nowPage];

        for (int i = 0; i < pageList.Count; i++)
        {
            blanks[i].GetComponentInChildren<TextMeshProUGUI>().text = pageList[i]["Content"].ToString();
            blankEmpties[i] = true;

        }
        checkEmpties();
    }

    void initializeEmpties()
    {
        for (int i = 0; i < blankEmpties.Length; i++)
            blankEmpties[i] = false;
    }

    void checkEmpties()
    {
        for (int i = 0; i < blankEmpties.Length; i++)
        {
            if (blankEmpties[i])
            {
                blanks[i].SetActive(true);
            }
            else
                blanks[i].SetActive(false);
        }

    }

    void initializeDL(int count)
    {
        if (count % 4 != 0)
        {
            count = count / 4 + 1;
        }
        else
        {
            count /= 4;
        }

        for (int i = 0; i < count; i++)
        {
            dividedDiary.Add(new List<Dictionary<string, object>>());
        }
    }


    void divideDiary()
    {
        List<Dictionary<string, object>> diary = CSVReader.Read("CSVfiles/02_Database/Diaryfiles/" + csvs[selectedC].name);
        diary = diary.GetRange(0, saveDiaryData);

        initializeDL(diary.Count);

        /**
        for (int i = 0; i < diary.Count; i += 4)
        {
            //dividedDiary.Add(diary.GetRange(i, 4)); // 범위 넘음
        }
        **/
        for (int i = 0, j = -1; i < diary.Count; i++)
        {
            if (i % 4 == 0)
                j++;

            dividedDiary[j].Add(diary[i]);
        }
    }
}
