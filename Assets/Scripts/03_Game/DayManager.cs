using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : Singleton<DayManager>
{
    [Header("Day���� Ǯ��� �ϴ� ��� ��ȭ")]
    public TextAsset[] dialogues;
    public bool[] d_completes = new bool[10];


    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
