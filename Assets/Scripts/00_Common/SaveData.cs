using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : Singleton<SaveData>
{
    [Header("é�� ����")]
    [SerializeField] int chapter;

    [Header("é�ͺ� �ι�")]
    [SerializeField] List<Figure> figures;

    public void saveData()
    {

    }

    public void loadData()
    {

    }
}
