using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    [SerializeField] TextAsset privisoCSV;
    public List<Priviso> privios = new List<Priviso>();

    [Header("챕터별 갱신 일기")]
    public int[] saveDiaryData = new int[6];


    void Start()
    {
        // 임시로 단서 넣어두기
        //setTmpPriviso();

        // 임시로 챕터별 일기 갱신 라인 넣어두기
        //saveDiaryData[1] = 5;
       
    }

    public void addPriviso(string privisoId)
    {
        List<Dictionary<string, object>> csv = CSVReader.Read("CSVfiles/02_Database/Privisofiles/" + privisoCSV.name);
        Dictionary<string, object> p = csv.Where(p => p["Id"].ToString() == privisoId).ToList()[0];

        Priviso priviso = new Priviso(p["Id"].ToString(), p["Name"].ToString(), p["Content"].ToString());
        privios.Add(priviso);

        // 단서 획득 화면 띄우기
    }
}
