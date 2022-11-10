using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectData
{
    public string name;
    public List<string> dialogueList = new List<string>();

    public List<bool> openDialouges = new List<bool>();
    public List<bool> completeDialogues = new List<bool>();

    public int nowDialogue = 0;

    // ������
    public ObjectData(string name)
    {
        this.name = name;
    }

    public void openDialogue(string dName)
    {
        int idx = dialogueList.FindIndex(e => e == dName);
        openDialouges[idx] = true;

        updateDialoge();
    }

    public void completeDialogue()
    {
        completeDialogues[nowDialogue] = true;
    }

    public void updateDialoge()
    {
        // open = true�� completet=false�� �ֵ� �߿��� ���� index ������ ���� ��
        for (int i = 0; i < dialogueList.Count; i++)
        {
            if (openDialouges[i] && !completeDialogues[i])
            {
                nowDialogue = i;
                break;
            }
        }
    }
}

public static class ObjectTable
{

    [Header("Object Table")]
    public static Hashtable oTable = new Hashtable();

    // ����� GameManager���� ȣ��
    // Day�� �ٲ� ������ ���� ȣ�� �ѹ����� �ؾ���
    static public void setTable()
    {
        oTable.Clear();

        string path = "CSVfiles/03_Objects";

        List<Dictionary<string, object>> objects = CSVReader.Read(path + "/ObjectLists/" 
                                                                 + GameData.Instance.chapter 
                                                                 + "/"
                                                                 + GameData.Instance.day);

        // ����for�� �ſ� ���� �ȵ���
        foreach (Dictionary<string, object> c in objects)
        {
            List<Dictionary<string, object>> objectDatas = CSVReader.Read(path + "/ObjectDatas/"
                                                                 + GameData.Instance.chapter
                                                                 + "/"
                                                                 + GameData.Instance.day)
                                                                 .Where(e => e["Name"].ToString() == c["ObjectList"].ToString()).ToList();

            ObjectData objectData = new ObjectData(c["ObjectList"].ToString());

            foreach (Dictionary<string, object> o in objectDatas)
            {
                objectData.dialogueList.Add(o["DialogueName"].ToString());
                objectData.openDialouges.Add(false);
                objectData.completeDialogues.Add(false);
            }

            objectData.openDialouges[0] = true;

            oTable.Add(objectData.name, objectData);

        }

    }
}
