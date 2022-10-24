using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Priviso
{
    private string _id;
    public string id
    {
        get { return _id; }
        set { _id = value; }
    }

    private string _name;
    public string name
    {
        get { return _name; }
        set { _name = value; }
    }

    List<string> contents = new List<string>();
    public int updatedInfo = 0;

    Sprite _image = null;
    public Sprite image
    {
        get { return _image; }
        set { _image = value; }
    }

    // »ý¼ºÀÚ
    public Priviso(string id, string name, string content)
    {
        this.id = id;
        this.name = name;
        image = Resources.Load<Sprite>("Images/Privisos/" + GameData.Instance.chapter + "/" + id);

        addContent(content);
    }

    public void addContent(string content)
    {
        contents.Add(content);
        updatedInfo++;
    }

    public List<string> getContents()
    {
        return contents;
    }
    public string showContentsTxt()
    {
        string txt = "";

        foreach (string c in contents)
        {
            txt += c + "\n";
        }

        return txt;
    }

}
