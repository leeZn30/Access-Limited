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


    void Start()
    {

    }

    void Update()
    {
        
    }
}
