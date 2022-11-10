using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

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

    [Header("Day 정보")]
    public int day;

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
       
    }

    public void addFigures(string figureId, int idx)
    {
        List<Dictionary<string, object>> csv = CSVReader.Read("CSVfiles/02_Database/Figurefiles/" + figureCSV.name);

        // figure  찾기
        var figure = figures.Find(figureId);
        //var figure = figures.Find(f => f.id == figureId);

        try
        {
            // 없으면 새로 추가
            if (figure == null)
            {
                // 개복잡 실화냐
                Dictionary<string, object> element = csv.Where(e => e["Id"].ToString() == figureId).ToList()
                                                     .Where(e => e["Idx"].ToString() == idx.ToString()).ToList()[0];

                figure = new Figure(element["Id"].ToString(), 
                                    element["Name"].ToString(), 
                                    int.Parse(element["Age"].ToString()), 
                                    element["Gender"].ToString(), 
                                    element["Content"].ToString(), 
                                    idx);

                figures.Add(figure);
            }
            // 있으면 내용 추가
            else
            {
                string newContent = csv.Where(e => e["Id"].ToString() == figureId).ToList()
                    .Where(e => e["Idx"].ToString() == idx.ToString()).ToList()[0]["Content"].ToString();

                figure.addContent(newContent, idx);
            }

        }
        catch (Exception error)
        {
            Debug.Log("[Figure Add 에러 발생]: " + error);
        }
    }

    public void addPriviso(string privisoId, int idx)
    {

        try
        {
            List<Dictionary<string, object>> csv = CSVReader.Read("CSVfiles/02_Database/Privisofiles/" + privisoCSV.name);

            // figure  찾기
            var priviso = privisos.Find(privisoId);
            //var priviso = privisos.Find(p => p.id == privisoId);

            // 없으면 새로 추가
            if (priviso == null)
            {
                Dictionary<string, object> element = csv.Where(e => e["Id"].ToString() == privisoId).ToList()
                                                     .Where(e => e["Idx"].ToString() == idx.ToString()).ToList()[0];

                priviso = new Priviso(element["Id"].ToString(), element["Name"].ToString(), element["Content"].ToString(), idx);

                privisos.Add(priviso);

            }
            // 있으면 내용 추가
            else
            {
                string newContent = csv.Where(e => e["Id"].ToString() == privisoId).ToList()
                                    .Where(e => e["Idx"].ToString() == idx.ToString()).ToList()[0]["Content"].ToString();

                priviso.addContent(newContent, idx);
            }

        }
        catch (Exception error)
        {
            Debug.Log("Priviso Add 에러 발생]: " + error);
        }
    }

    public Priviso getPriviso(string id)
    {
        var priviso = (Priviso)privisos.Find(id);

        return priviso;
    }

}
