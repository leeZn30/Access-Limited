using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : Singleton<GameData>
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // LoadData에서 가져오기
        // figures = SaveData.Instance.
        // chapter = SaveData
    }

    [Header("챕터 정보")]
    [SerializeField] int _chapter;
    public int chapter
    {
        get { return _chapter; }
        set { _chapter = value; }
    }

    [Header("챕터별 인물")]
    public List<Figure> figures = new List<Figure>();

    [Header("챕터별 단서")]
    [SerializeField] TextAsset privisoCSVTmp;
    public List<Priviso> privios = new List<Priviso>();

    [Header("챕터별 갱신 일기")]
    public int[] saveDiaryData = new int[6];


    void Start()
    {
        // 임시로 단서 넣어두기
        setTmpPriviso();

        // 임시로 챕터별 일기 갱신 라인 넣어두기
        saveDiaryData[1] = 5;
        
    }

    void setTmpPriviso()
    {
        List<Dictionary<string, object>> tmp = CSVReader.Read("CSVfiles/02_Database/Privisofiles/" + privisoCSVTmp.name);

        foreach(Dictionary<string, object> item in tmp)
        {
            Priviso p = new Priviso(item["Id"].ToString(), item["Name"].ToString(), item["Content"].ToString());

            privios.Add(p);
        }
    }
}
