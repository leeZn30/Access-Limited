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

    // ���� ������ �� ���� ȣ�� (����� DialogueManager ȣ�� ��)
    static public void setTable()
    {
        // cTable �ʱ�ȭ
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
