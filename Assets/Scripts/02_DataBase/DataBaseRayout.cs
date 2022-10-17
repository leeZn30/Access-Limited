using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataBaseRayout : MonoBehaviour
{
    [Header("오브젝트")]
    [SerializeField] Button exitbtn;
    [SerializeField] Button eventFilebtn;
    [SerializeField] Button diarybtn;

    [Header("레이아웃")]
    [SerializeField] GameObject DBRayout;
    [SerializeField] GameObject eventFileRayout;
    [SerializeField] GameObject diaryRayout;

    [Header("챕터 정보")]
    public int chapter;


    // Start is called before the first frame update
    void Start()
    {
        eventFilebtn.onClick.AddListener(openEventFileRayout);
        diarybtn.onClick.AddListener(openDiaryRayout);

    }

    // 팝업 초기화 => 팝업 스크립트에서

    // DBRayout 열기
    void openDBRayout()
    {
        // 다른 모든 레이아웃 지우기


        DBRayout.SetActive(true);
    }

    //DBRayout 닫기
    void closeDBRayout()
    {
        DBRayout.SetActive(false);
    }

    // 사건 파일 열기
    void openEventFileRayout()
    {
        closeDBRayout();

        eventFileRayout.SetActive(true);
    }

    void openDiaryRayout()
    {
        closeDBRayout();

        diaryRayout.SetActive(true);
    }

}
