using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DatabaseManager : Singleton<DatabaseManager>
{
    [Header("é�� ����")]
    [SerializeField] int _chapter;
    public int chapter
    {
        get { return _chapter; }
        set { _chapter = value;  }
    }

    [Header("�ʿ��� CSV����")]
    [SerializeField] TextAsset ef; //eventfiles

    [Header("���� ���̾ƿ� ����")]
    [SerializeField] int now_rayout;

    [Header("��ư")]
    [SerializeField] Button exitBtn;
    [SerializeField] Button backBtn;
    [SerializeField] Button EFBtn;
    [SerializeField] Button diaryBtn;
    [SerializeField] Button FFBtn;
    [SerializeField] Button privisonBtn;
    [SerializeField] Button FLBtn;
    [SerializeField] Button FIBtn;

    [Header("���̾ƿ�")]
    const int rayoutNum = 7;
    [SerializeField] GameObject[] rayouts = new GameObject[rayoutNum];
    [SerializeField] LinkedList<int>[] pageLinks = new LinkedList<int>[rayoutNum];

    [Header("�˾�â")]
    [SerializeField] bool isPopupOpen = false;
    [SerializeField] GameObject popup;


    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        // ��ư ������
        exitBtn.onClick.AddListener(exit);
        backBtn.onClick.AddListener(backPage);

        // ���̾ƿ� ��ũ ����
        setLinks();
    }

    void setLinks()
    {
        for (int i = 1; i < rayoutNum; i++)
        {
            LinkedList<int> tmp = new LinkedList<int>();
            LinkedListNode<int> node = tmp.AddLast(i);
            pageLinks[i] = tmp;

            if (i == 5) // �ϱ�
                tmp.AddBefore(node, 0);
            else
                tmp.AddBefore(node, i - 1);
        }
    }

    public void openPopup()
    {
        if (!isPopupOpen)
        {
            popup.SetActive(true);
            isPopupOpen = true;
            DialogueManager.Instance.isEnable = false;
        }
    }

    public void closePopup()
    {
        if (isPopupOpen)
        {
            popup.SetActive(false);
            isPopupOpen = false;
            DialogueManager.Instance.isEnable = true;
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

    void exit()
    {
        // �ʱ�ȭ(DB���̾ƿ� Ȱ��ȭ) �ʿ�
        goPage(0);

        //�˾�â �ݱ�
        closePopup();

    }

    void backPage()
    {
        rayouts[now_rayout].SetActive(false);
        rayouts[pageLinks[now_rayout].Find(now_rayout).Previous.Value].SetActive(true);
        now_rayout = pageLinks[now_rayout].Find(now_rayout).Previous.Value;
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
}
