using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : Singleton<DayManager>
{
    [Header("Day에서 풀어야 하는 모든 대화")]
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
