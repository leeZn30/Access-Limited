using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    [Header("é�ͺ� �ι�")]
    [SerializeField] TextAsset figureCSV;
    public InfoList figures = new InfoList();

    [Header("é�ͺ� �ܼ�")]
    [SerializeField] TextAsset privisoCSV;
    public InfoList privisos = new InfoList();

    [Header("é�ͺ� ���� �ϱ�")]
    public int[] saveDiaryData = new int[6];


    void Start()
    {
        // �ӽ÷� �ܼ� �־�α�
        //setTmpPriviso();

        // �ӽ÷� é�ͺ� �ϱ� ���� ���� �־�α�
        //saveDiaryData[1] = 5;
       
    }

    public void addFigures(string figureId)
    {
        List<Dictionary<string, object>> csv = CSVReader.Read("CSVfiles/02_Database/Figurefiles/" + figureCSV.name);

        // figure  ã��
        var figure = figures.Find(figureId);
        //var figure = figures.Find(f => f.id == figureId);

        // ������ ���� �߰�
        if (figure == null)
        {
            Dictionary<string, object> element = csv.Where(e => e["Id"].ToString() == figureId).ToList()[0];
            figure = new Figure(element["Id"].ToString(), element["Name"].ToString(), int.Parse(element["Age"].ToString()), element["Gender"].ToString(), element["Content"].ToString());

            figures.Add(figure);
        }
        // ������ ���� �߰�
        else
        {
            string newContent = csv.Where(e => e["Id"].ToString() == figureId).ToList()[figure.updatedInfo]["Content"].ToString();
            figure.addContent(newContent);

        }

    }

    public void addPriviso(string privisoId)
    {

        List<Dictionary<string, object>> csv = CSVReader.Read("CSVfiles/02_Database/Privisofiles/" + privisoCSV.name);

        // figure  ã��
        var priviso = privisos.Find(privisoId);
        //var priviso = privisos.Find(p => p.id == privisoId);

        // ������ ���� �߰�
        if (priviso == null)
        {
            Dictionary<string, object> element = csv.Where(e => e["Id"].ToString() == privisoId).ToList()[0];
            priviso = new Priviso(element["Id"].ToString(), element["Name"].ToString(), element["Content"].ToString());

            privisos.Add(priviso);

        }
        // ������ ���� �߰�
        else
        {
            string newContent = csv.Where(e => e["Id"].ToString() == privisoId).ToList()[priviso.updatedInfo]["Content"].ToString();
            priviso.addContent(newContent);
        }
        Debug.Log("Priviso Add �Ϸ�");
    }
}
