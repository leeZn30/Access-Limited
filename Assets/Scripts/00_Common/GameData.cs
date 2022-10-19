using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] TextAsset privisoCSVTmp;
    public List<Priviso> privios = new List<Priviso>();

    [Header("é�ͺ� ���� �ϱ�")]
    public int[] saveDiaryData = new int[6];


    void Start()
    {
        // �ӽ÷� �ܼ� �־�α�
        setTmpPriviso();

        // �ӽ÷� é�ͺ� �ϱ� ���� ���� �־�α�
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
