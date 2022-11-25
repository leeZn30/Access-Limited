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

    [Header("챕터별 정보")]
    [SerializeField] TextAsset infoCSV;
    public InfoList infos = new InfoList();

    [Header("챕터별 갱신 일기")]
    public int[] saveDiaryData = new int[6];

    public void addInfo(string id, int idx)
    {
        List<Dictionary<string, object>> csv = CSVReader.Read("CSVfiles/02_Database/Infofiles/" + infoCSV.name);

        var figure = figures.Find(id);
        var priviso = privisos.Find(id);

        string text = "";

        try
        {
            if (figure == null && priviso == null)
            {
                // 개복잡 실화냐
                Dictionary<string, object> element = csv.Where(e => e["Id"].ToString() == id).ToList()
                                                     .Where(e => e["Idx"].ToString() == idx.ToString()).ToList()[0];

                if (int.Parse(element["Type"].ToString()) == 0)
                {
                    figure = new Figure(element["Id"].ToString(),
                                        element["Name"].ToString(),
                                        int.Parse(element["Age"].ToString()),
                                        element["Gender"].ToString(),
                                        element["Content"].ToString(),
                                        idx);

                    figures.Add(figure);
                    text = "인물 <color=#FF00F5>" + figure.name + "</color> 정보 갱신";
                }
                else
                {
                    priviso = new Priviso(element["Id"].ToString(), 
                                          element["Name"].ToString(), 
                                          element["Content"].ToString(), 
                                          idx);

                    privisos.Add(priviso);
                    text = "단서 <color=#FF00F5>" + priviso.name + "</color> 정보 갱신";
                }
            }
            else if (figure != null)
            {
                string newContent = csv.Where(e => e["Id"].ToString() == id).ToList()
                    .Where(e => e["Idx"].ToString() == idx.ToString()).ToList()[0]["Content"].ToString();

                figure.addContent(newContent, idx);

                text = "인물 <color=#FF00F5>" + figure.name + "</color> 정보 갱신";
            }
            else
            {
                string newContent = csv.Where(e => e["Id"].ToString() == id).ToList()
                                    .Where(e => e["Idx"].ToString() == idx.ToString()).ToList()[0]["Content"].ToString();

                priviso.addContent(newContent, idx);
                text = "단서 <color=#FF00F5>" + priviso.name + "</color> 정보 갱신";
            }

            DialogueManager.Instance.showInfo(text);
        }
        catch (Exception error)
        {
            Debug.Log("[Info Add 에러 발생]: " + error);
        }
    }

    public Priviso getPriviso(string id)
    {
        Priviso priviso = privisos.Find(id) as Priviso;

        return priviso;
    }

}
