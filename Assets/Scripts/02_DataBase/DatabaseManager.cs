using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DatabaseManager : Singleton<DatabaseManager>
{
    [Header("é�� ����")]
    public int chapter;

    [Header("�ʿ��� CSV����")]
    [SerializeField] TextAsset ef; //eventfiles
    [SerializeField] TextAsset ff; //figurefiles
    [SerializeField] TextAsset fa; //figureAdds

    [Header("���� ���̾ƿ� ����")]
    [SerializeField] int now_rayout;
    [SerializeField] int prev_rayout = -1;

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
    [SerializeField] GameObject[] rayouts = new GameObject[5];
    [SerializeField] GameObject DBRayout;
    [SerializeField] GameObject EFRayout;
    [SerializeField] GameObject FFRayout;
    [SerializeField] GameObject FLRayout;
    [SerializeField] GameObject FIRayout;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }


    // Start is called before the first frame update
    void Start()
    {
        // ��ư ������
        exitBtn.onClick.AddListener(exit);
        backBtn.onClick.AddListener(backPage);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void exit()
    {
        // �ʱ�ȭ(DB���̾ƿ� Ȱ��ȭ) �ʿ�
    }

    void backPage()
    {
        rayouts[now_rayout].SetActive(false);
        rayouts[prev_rayout].SetActive(true);

        int tmp = prev_rayout;
        now_rayout = prev_rayout;
        prev_rayout = tmp;
    }

    public void goPage(int pageIdx)
    {
        rayouts[now_rayout].SetActive(false);
        rayouts[pageIdx].SetActive(true);

        prev_rayout = now_rayout;
        now_rayout = pageIdx;
    }
}
