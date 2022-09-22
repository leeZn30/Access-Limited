using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CharacterTable
{
    [Header("Character Table")]
    public static Hashtable cTable = new Hashtable();

    // DialogueManager, Ȥ�� ���� ������ �� ���� ȣ��
    static public void setTable()
    {
        // cTable �ʱ�ȭ
        cTable.Clear();

        List<Dictionary<string, object>> characters = CSVReader.Read("CSVfiles/00_Common/Character");

        foreach (Dictionary<string, object> c in characters)
        {
            cTable.Add(int.Parse(c["CharacterId"].ToString()), c["Name"].ToString());
        }
    }
}
