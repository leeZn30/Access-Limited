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
    public List<Figure> figures = new List<Figure>();

    [Header("é�ͺ� �ܼ�")]
    [SerializeField] TextAsset privisoCSV;
    public List<Priviso> privios = new List<Priviso>();

    [Header("é�ͺ� ���� �ϱ�")]
    public int[] saveDiaryData = new int[6];


    void Start()
    {
        // �ӽ÷� �ܼ� �־�α�
        //setTmpPriviso();

        // �ӽ÷� é�ͺ� �ϱ� ���� ���� �־�α�
        //saveDiaryData[1] = 5;
       
    }

    public void addPriviso(string privisoId)
    {
        List<Dictionary<string, object>> csv = CSVReader.Read("CSVfiles/02_Database/Privisofiles/" + privisoCSV.name);
        Dictionary<string, object> p = csv.Where(p => p["Id"].ToString() == privisoId).ToList()[0];

        Priviso priviso = new Priviso(p["Id"].ToString(), p["Name"].ToString(), p["Content"].ToString());
        privios.Add(priviso);

        // �ܼ� ȹ�� ȭ�� ����
    }
}
