using System.Collections.Generic;

public class Figure
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

    private string _gender;
    public string gender
    {
        get { return _gender; }
        set { _gender = value; }
    }

    private int _age;
    public int age
    {
        get { return _age; }
        set { _age = value; }
    }

    List<string> contents = new List<string>();
    public int updatedInfo = 0;

    // »ı¼ºÀÚ
    public Figure(string id, string name, int age, string gender, string content)
    {
        this.id = id;
        this.name = name;
        this.age = age;
        this.gender = gender;

        addContents(content);
    }

    public void addContents(string content)
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

        foreach(string c in contents)
        {
            txt += c + "\n";
        }

        return txt;
    }

}
