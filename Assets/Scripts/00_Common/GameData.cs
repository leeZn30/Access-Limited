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

        // LoadData���� ��������
        // figures = SaveData.Instance.
        // chapter = SaveData
    }

    [Header("é�� ����")]
    [SerializeField] int _chapter;
    public int chapter
    {
        get { return _chapter; }
        set { _chapter = value; }
    }

    [Header("Day ����")]
    public int day;

    [Header("é�ͺ� �ι�")]
    [SerializeField] TextAsset figureCSV;
    public InfoList figures = new InfoList();

    [Header("é�ͺ� �ܼ�")]
    [SerializeField] TextAsset privisoCSV;
    public InfoList privisos = new InfoList();

    [Header("é�ͺ� ����")]
    [SerializeField] TextAsset infoCSV;
    public InfoList infos = new InfoList();

    [Header("é�ͺ� ���� �ϱ�")]
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
                // ������ ��ȭ��
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
                    text = "�ι� <color=#FF00F5>" + figure.name + "</color> ���� ����";
                }
                else
                {
                    priviso = new Priviso(element["Id"].ToString(), 
                                          element["Name"].ToString(), 
                                          element["Content"].ToString(), 
                                          idx);

                    privisos.Add(priviso);
                    text = "�ܼ� <color=#FF00F5>" + priviso.name + "</color> ���� ����";
                }
            }
            else if (figure != null)
            {
                string newContent = csv.Where(e => e["Id"].ToString() == id).ToList()
                    .Where(e => e["Idx"].ToString() == idx.ToString()).ToList()[0]["Content"].ToString();

                figure.addContent(newContent, idx);

                text = "�ι� <color=#FF00F5>" + figure.name + "</color> ���� ����";
            }
            else
            {
                string newContent = csv.Where(e => e["Id"].ToString() == id).ToList()
                                    .Where(e => e["Idx"].ToString() == idx.ToString()).ToList()[0]["Content"].ToString();

                priviso.addContent(newContent, idx);
                text = "�ܼ� <color=#FF00F5>" + priviso.name + "</color> ���� ����";
            }

            DialogueManager.Instance.showInfo(text);
        }
        catch (Exception error)
        {
            Debug.Log("[Info Add ���� �߻�]: " + error);
        }
    }

    public Priviso getPriviso(string id)
    {
        Priviso priviso = privisos.Find(id) as Priviso;

        return priviso;
    }

}
