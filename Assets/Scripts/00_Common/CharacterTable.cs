using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData
{
    public string id;
    public string name;
    public int illustNum;
}

public static class CharacterTable
{
    [Header("Character Table")]
    public static Hashtable cTable = new Hashtable();

    // 게임 시작할 때 최초 호출 (현재는 DialogueManager 호출 시)
    static public void setTable()
    {
        // cTable 초기화
        cTable.Clear();

        List<Dictionary<string, object>> characters = CSVReader.Read("CSVfiles/00_Common/Character");

        foreach (Dictionary<string, object> c in characters)
        {
            CharacterData element = new CharacterData();
            element.id = c["CharacterId"].ToString();
            element.name = c["Name"].ToString();
            element.illustNum = int.Parse(c["IllustNum"].ToString());

            cTable.Add(c["CharacterId"].ToString(), element);
            //cTable.Add(int.Parse(c["CharacterId"].ToString()), c["Name"].ToString());
        }

    }
}
