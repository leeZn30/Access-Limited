using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DatabaseManager : Singleton<DatabaseManager>
{
    [Header("챕터 정보")]
    public int chapter;

    [Header("필요한 CSV파일")]
    [SerializeField] TextAsset ef; //eventfiles
    [SerializeField] TextAsset ff; //figurefiles
    [SerializeField] TextAsset fa; //figureAdds

    [Header("현재 레이아웃 정보")]
    [SerializeField] int now_rayout;
    [SerializeField] int prev_rayout = -1;

    [Header("버튼")]
    [SerializeField] Button exitBtn;
    [SerializeField] Button backBtn;
    [SerializeField] Button EFBtn;
    [SerializeField] Button diaryBtn;
    [SerializeField] Button FFBtn;
    [SerializeField] Button privisonBtn;
    [SerializeField] Button FLBtn;
    [SerializeField] Button FIBtn;

    [Header("레이아웃")]
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
        // 버튼 리스너
        exitBtn.onClick.AddListener(exit);
        backBtn.onClick.AddListener(backPage);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void exit()
    {
        // 초기화(DB레이아웃 활성화) 필요
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
