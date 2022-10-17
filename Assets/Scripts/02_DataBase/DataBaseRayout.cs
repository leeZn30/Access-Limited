using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataBaseRayout : MonoBehaviour
{
    [Header("������Ʈ")]
    [SerializeField] Button exitbtn;
    [SerializeField] Button eventFilebtn;
    [SerializeField] Button diarybtn;

    [Header("���̾ƿ�")]
    [SerializeField] GameObject DBRayout;
    [SerializeField] GameObject eventFileRayout;
    [SerializeField] GameObject diaryRayout;

    [Header("é�� ����")]
    public int chapter;


    // Start is called before the first frame update
    void Start()
    {
        eventFilebtn.onClick.AddListener(openEventFileRayout);
        diarybtn.onClick.AddListener(openDiaryRayout);

    }

    // �˾� �ʱ�ȭ => �˾� ��ũ��Ʈ����

    // DBRayout ����
    void openDBRayout()
    {
        // �ٸ� ��� ���̾ƿ� �����


        DBRayout.SetActive(true);
    }

    //DBRayout �ݱ�
    void closeDBRayout()
    {
        DBRayout.SetActive(false);
    }

    // ��� ���� ����
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
