using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Figure
{
    private int _id;
    public int id
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

    // »ı¼ºÀÚ
    public Figure(int id, string name, int age, string gender, string content)
    {
        this.id = id;
        this.name = name;
        this.age = age;
        this.gender = gender;

        contents.Add(content);
    }

    public void addContents(string content)
    {
        contents.Add(content);
    }

    public List<string> getContents()
    {
        return contents;
    }

}
