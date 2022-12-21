using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DatabaseManager : Singleton<DatabaseManager>
{
    [Header("챕터 정보")]
    [SerializeField] int _chapter;
    public int chapter
    {
        get { return _chapter; }
        set { _chapter = value; }
    }

    [Header("필요한 CSV파일")]
    [SerializeField] TextAsset ef; //eventfiles

    [Header("현재 레이아웃 정보")]
    [SerializeField] int _now_rayout;
    public int now_rayout
    {
        get { return _now_rayout; }
        set { _now_rayout = value; }
    }


    [Header("버튼")]
    [SerializeField] Button exitBtn;
    [SerializeField] Button backBtn;

    [Header("레이아웃")]
    const int rayoutNum = 7;
    [SerializeField] GameObject[] rayouts = new GameObject[rayoutNum];
    [SerializeField] LinkedList<int>[] pageLinks = new LinkedList<int>[rayoutNum];

    [Header("팝업창")]
    public bool isPopupOpen = false;
    [SerializeField] GameObject popup;
    [SerializeField] GameObject background;


    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    void Start()
    {
        // 버튼 리스너
        exitBtn.onClick.AddListener(openClosePopup);
        backBtn.onClick.AddListener(backPage);

        // 레이아웃 링크 설정
        setLinks();
    }

    void setLinks()
    {
        for (int i = 1; i < rayoutNum; i++)
        {
            LinkedList<int> tmp = new LinkedList<int>();
            LinkedListNode<int> node = tmp.AddLast(i);
            pageLinks[i] = tmp;

            if (i == 5) // 일기
                tmp.AddBefore(node, 0);
            else if (i == 4) // 단서
                tmp.AddBefore(node, 1);
            else
                tmp.AddBefore(node, i - 1);
        }
    }

    public void openClosePopup()
    {
        if (!isPopupOpen)
        {
            popup.SetActive(true);
            background.SetActive(true);
            isPopupOpen = true;

            if (!DialogueManager.Instance.isEnable)
            {
                MapManager.Instance.offPlaceTranslator();
                MapManager.Instance.offInteractiveObject();
            }

        }
        else
        {
            goPage(0);

            popup.SetActive(false);
            background.SetActive(false);
            isPopupOpen = false;

            if (!DialogueManager.Instance.isEnable)
            {
                MapManager.Instance.onPlaceTranslator();
                MapManager.Instance.onInteractiveObject();
            }

        }
    }

    public List<Dictionary<string, object>> getCSV(int mode)
    {
        switch (mode)
        {
            case 0:
                return CSVReader.Read("CSVfiles/02_Database/Eventfiles/" + ef.name);

            default:
                return null;
        }
    }

    void backPage()
    {
        if (pageLinks[now_rayout] != null)
        {
            rayouts[now_rayout].SetActive(false);
            rayouts[pageLinks[now_rayout].Find(now_rayout).Previous.Value].SetActive(true);
            now_rayout = pageLinks[now_rayout].Find(now_rayout).Previous.Value;
        }
        else
            Debug.Log("No Previous Database Page!");

    }

    public void goPage(int pageIdx)
    {
        rayouts[now_rayout].SetActive(false);
        rayouts[pageIdx].SetActive(true);

        now_rayout = pageIdx;
    }

    public void pickFigure(Figure figure)
    {
        FigureInfoRayout flrayout = rayouts[3].GetComponent<FigureInfoRayout>();

        flrayout.pickedFigure = figure;
        flrayout.setContent();
    }

    public void pickDiary(int chapter)
    {
        DiaryDescRayout ddrayout = rayouts[6].GetComponent<DiaryDescRayout>();

        ddrayout.selectedC = chapter;
    }

    public int getNowRayout()
    {
        return now_rayout;
    }
}
