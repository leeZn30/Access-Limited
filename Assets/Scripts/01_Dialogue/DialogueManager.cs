using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DialogueManager : Singleton<DialogueManager>
{
    [Header("CSV 파일")]
    [SerializeField] TextAsset d_file;
    [SerializeField] TextAsset c_file;

    [Header("CSV 출력")]
    public List<Dictionary<string, object>> lines;
    public Dictionary<string, object> line;
    public int now_line;

    [Header("Character")]
    List<Dictionary<string, object>> characters;

    void Awake()
    {
        lines = CSVReader.Read("CSVfiles/01_Dialogue/" + d_file.name);
        characters = CSVReader.Read("CSVfiles/00_Common/" + c_file.name);

        now_line = 0;
    }

    public string getCharacterName(int id)
    {
        /**
        foreach (Dictionary<string, object> character in characters)
        {
            if (int.Parse(characters[i]["Timing"].ToString()) == Cycle)
            {
            }

        }
        **/

        var character = characters.Where(c => int.Parse(c["CharacterId"].ToString()) == id);

        Debug.Log(character.ToString());

        return "hello";


    }

}
