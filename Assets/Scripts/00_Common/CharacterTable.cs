using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CharacterTable
{
    [Header("Character Table")]
    public static Hashtable cTable = new Hashtable();

    // DialogueManager, 혹은 게임 시작할 때 최초 호출
    static public void setTable()
    {
        // cTable 초기화
        cTable.Clear();

        List<Dictionary<string, object>> characters = CSVReader.Read("CSVfiles/00_Common/Character");

        foreach (Dictionary<string, object> c in characters)
        {
            cTable.Add(int.Parse(c["CharacterId"].ToString()), c["Name"].ToString());
        }
    }
}
