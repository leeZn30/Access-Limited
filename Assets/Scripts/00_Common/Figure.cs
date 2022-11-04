using System.Collections.Generic;

public class Figure : Info
{
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


    // »ı¼ºÀÚ
    public Figure(string id, string name, int age, string gender, string content, int c_idx)
    {
        this.id = id;
        this.name = name;
        this.age = age;
        this.gender = gender;

        addContent(content, c_idx);
    }


}
