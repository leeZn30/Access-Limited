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

    [Header("é�ͺ� ���� �ϱ�")]
    public int[] saveDiaryData = new int[6];

    public void addFigures(string figureId, int idx)
    {
        List<Dictionary<string, object>> csv = CSVReader.Read("CSVfiles/02_Database/Figurefiles/" + figureCSV.name);

        // figure  ã��
        var figure = figures.Find(figureId);
        //var figure = figures.Find(f => f.id == figureId);

        try
        {
            // ������ ���� �߰�
            if (figure == null)
            {
                // ������ ��ȭ��
                Dictionary<string, object> element = csv.Where(e => e["Id"].ToString() == figureId).ToList()
                                                     .Where(e => e["Idx"].ToString() == idx.ToString()).ToList()[0];

                //Debug.Log(element.Values);

                figure = new Figure(element["Id"].ToString(), 
                                    element["Name"].ToString(), 
                                    int.Parse(element["Age"].ToString()), 
                                    element["Gender"].ToString(), 
                                    element["Content"].ToString(), 
                                    idx);

                figures.Add(figure);

                Debug.Log("[New Figure �߰�]: " + figure.id);
            }
            // ������ ���� �߰�
            else
            {
                string newContent = csv.Where(e => e["Id"].ToString() == figureId).ToList()
                    .Where(e => e["Idx"].ToString() == idx.ToString()).ToList()[0]["Content"].ToString();

                figure.addContent(newContent, idx);

                Debug.Log("[Figure ���� ����]: " + figure.id);
            }

            string text = "�ι� <color=#FF00F5>" + figure.name + "</color> ���� ����";
            DialogueManager.Instance.showInfo(text);

        }
        catch (Exception error)
        {
            Debug.Log("[Figure Add ���� �߻�]: " + error);
        }
    }

    public void addPriviso(string privisoId, int idx)
    {
        try
        {
            List<Dictionary<string, object>> csv = CSVReader.Read("CSVfiles/02_Database/Privisofiles/" + privisoCSV.name);

            // priviso  ã��
            var priviso = privisos.Find(privisoId);
            //var priviso = privisos.Find(p => p.id == privisoId);

            // ������ ���� �߰�
            if (priviso == null)
            {
                Dictionary<string, object> element = csv.Where(e => e["Id"].ToString() == privisoId).ToList()
                                                     .Where(e => e["Idx"].ToString() == idx.ToString()).ToList()[0];

                priviso = new Priviso(element["Id"].ToString(), element["Name"].ToString(), element["Content"].ToString(), idx);

                privisos.Add(priviso);

            }
            // ������ ���� �߰�
            else
            {
                string newContent = csv.Where(e => e["Id"].ToString() == privisoId).ToList()
                                    .Where(e => e["Idx"].ToString() == idx.ToString()).ToList()[0]["Content"].ToString();

                priviso.addContent(newContent, idx);
            }

            string text = "�ܼ� <color=#FF00F5>" + priviso.name + "</color> ���� ����";
            DialogueManager.Instance.showInfo(text);

        }
        catch (Exception error)
        {
            Debug.Log("Priviso Add ���� �߻�]: " + error);
        }
    }

    public Priviso getPriviso(string id)
    {
        Priviso priviso = privisos.Find(id) as Priviso;

        return priviso;
    }

}
