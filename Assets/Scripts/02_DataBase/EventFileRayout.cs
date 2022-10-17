using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventFileRayout : MonoBehaviour
{

    [Header("사건 정보")]
    [SerializeField] int _chapter;
    [SerializeField] List<TextAsset> CSVList;

    public int chapter
    {
        get { return _chapter; }
        set { _chapter = value; }
    }


    [Header("오브젝트")]
    [SerializeField] Button exitbtn;
    [SerializeField] Button eventFilebtn;
    [SerializeField] Button diarybtn;

    [Header("레이아웃")]
    [SerializeField] GameObject eventFileRayout;
    [SerializeField] GameObject figureListRayout;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
