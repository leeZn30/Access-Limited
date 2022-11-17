using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Answer : MonoBehaviour
{
    [Header("Answer 정보")]
    [SerializeField] string _content;
    [SerializeField] int _S1;
    [SerializeField] int _S2;
    [SerializeField] int _S3;
    [SerializeField] int _S4;
    [SerializeField] int _S5;
    [SerializeField] int _S6;
    [SerializeField] int _S7;
    [SerializeField] int _offset;

    [Header("Objects")]
    [SerializeField] TextMeshProUGUI content_b;

    public string content
    {
        get { return _content; }    // _data 반환
        set { _content = value; }   // value 키워드 사용
    }

    public int S1
    {
        get { return _S1; }    // _data 반환
        set { _S1 = value; }   // value 키워드 사용
    }
    public int S2
    {
        get { return _S2; }    // _data 반환
        set { _S2 = value; }   // value 키워드 사용
    }

    public int S3
    {
        get { return _S3; }    // _data 반환
        set { _S3 = value; }   // value 키워드 사용
    }

    public int S4
    {
        get { return _S4; }    // _data 반환
        set { _S4 = value; }   // value 키워드 사용
    }

    public int S5
    {
        get { return _S5; }    // _data 반환
        set { _S5 = value; }   // value 키워드 사용
    }

    public int S6
    {
        get { return _S6; }    // _data 반환
        set { _S6 = value; }   // value 키워드 사용
    }

    public int S7
    {
        get { return _S7; }    // _data 반환
        set { _S7 = value; }   // value 키워드 사용
    }

    public int offset
    {
        get { return _offset; }
        set { _offset = value; }
    }


    void Start()
    {
        content_b.text = content;
        GetComponentInChildren<Button>().onClick.AddListener(selected);
    }

    void selected()
    {
        DialogueManager.Instance.mission = 0;

        setPlayerStatus();

        DialogueManager.Instance.missionRunning = false;
        DialogueManager.Instance.lineoffset = offset;
        DialogueManager.Instance.nextDialogue();
    }


    void setPlayerStatus()
    {
        // status 변경
    }

    void OnDestroy()
    {
        // 다른 Answer도 찾아서 삭제 > 연쇄적
        Destroy(GameObject.FindGameObjectWithTag("Answer"));
    }


}
