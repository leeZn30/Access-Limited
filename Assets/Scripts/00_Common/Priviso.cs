
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

    // �ܼ��� ���� �߰� ������

    private string _content;
    public string content
    {
        get { return _content; }
        set { _content = value; }
    }

    // ������
    public Priviso(string id, string name, string content)
    {
        this.id = id;
        this.name = name;
        this.content = content;
    }

}
