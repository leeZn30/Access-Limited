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
    [SerializeField] TextAsset figureCSV;
    public InfoList figures = new InfoList();

    [Header("챕터별 단서")]
    [SerializeField] TextAsset privisoCSV;
    public InfoList privisos = new InfoList();

    [Header("챕터별 갱신 일기")]
    public int[] saveDiaryData = new int[6];


    void Start()
    {
        // 임시로 단서 넣어두기
        //setTmpPriviso();

        // 임시로 챕터별 일기 갱신 라인 넣어두기
        //saveDiaryData[1] = 5;
       
    }

    public void addFigures(string figureId)
    {
        List<Dictionary<string, object>> csv = CSVReader.Read("CSVfiles/02_Database/Figurefiles/" + figureCSV.name);

        // figure  찾기
        var figure = figures.Find(figureId);
        //var figure = figures.Find(f => f.id == figureId);

        // 없으면 새로 추가
        if (figure == null)
        {
            Dictionary<string, object> element = csv.Where(e => e["Id"].ToString() == figureId).ToList()[0];
            figure = new Figure(element["Id"].ToString(), element["Name"].ToString(), int.Parse(element["Age"].ToString()), element["Gender"].ToString(), element["Content"].ToString());

            figures.Add(figure);
        }
        // 있으면 내용 추가
        else
        {
            string newContent = csv.Where(e => e["Id"].ToString() == figureId).ToList()[figure.updatedInfo]["Content"].ToString();
            figure.addContent(newContent);

        }

    }

    public void addPriviso(string privisoId)
    {

        List<Dictionary<string, object>> csv = CSVReader.Read("CSVfiles/02_Database/Privisofiles/" + privisoCSV.name);

        // figure  찾기
        var priviso = privisos.Find(privisoId);
        //var priviso = privisos.Find(p => p.id == privisoId);

        // 없으면 새로 추가
        if (priviso == null)
        {
            Dictionary<string, object> element = csv.Where(e => e["Id"].ToString() == privisoId).ToList()[0];
            priviso = new Priviso(element["Id"].ToString(), element["Name"].ToString(), element["Content"].ToString());

            privisos.Add(priviso);

        }
        // 있으면 내용 추가
        else
        {
            string newContent = csv.Where(e => e["Id"].ToString() == privisoId).ToList()[priviso.updatedInfo]["Content"].ToString();
            priviso.addContent(newContent);
        }
        Debug.Log("Priviso Add 완료");
    }
}
