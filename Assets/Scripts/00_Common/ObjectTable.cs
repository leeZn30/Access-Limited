using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ObjectData
{
    public string name;
    public int dialogueNum;
    //public List<string> dialogueList = new List<string>();

    public List<bool> openDialouges = new List<bool>();
    public List<bool> completeDialogues = new List<bool>();

    public Dictionary<string, int> chainObjs = new Dictionary<string, int>();

    public int nowDialogue = 0;

    public bool isChecked = false;

    // 생성자
    public ObjectData(string name)
    {
        this.name = name;
    }

    public void openDialogue(int idx)
    {
        try
        {
            openDialouges[idx] = true;
        }
        catch (ArgumentException e)
        {
            Debug.Log("연쇄 대화 열지 못함!");
        }

        updateDialoge();
    }

    public void completeDialogue()
    {
        completeDialogues[nowDialogue] = true;
    }

    public void updateDialoge()
    {
        // open = true고 completet=false인 애들 중에서 제일 index 먼저인 것을 고름
        for (int i = 0; i < dialogueNum; i++)
        {
            if (openDialouges[i] && !completeDialogues[i])
            {
                nowDialogue = i;
                isChecked = false;
                return;
            }
        }
        isChecked = true;
    }
}

public static class ObjectTable
{

    [Header("Object Table")]
    public static Hashtable oTable = new Hashtable();

    // 현재는 GameManager에서 호출
    // Day가 바뀔 때마다 최초 호출 한번으로 해야함
    static public void setTable()
    {
        oTable.Clear();

        // day별 오브젝트 리스트
        string path = string.Format("CSVfiles/03_Objects/{0}", GameData.Instance.chapter);
        List<Dictionary<string, object>> objectLists = CSVReader.Read(path)
                                                       .Where(e => int.Parse(e["Day"].ToString()) 
                                                                   == GameData.Instance.day).ToList();
        
        foreach (Dictionary<string, object> c in objectLists)
        {
            if (oTable.ContainsKey(c["Name"].ToString()))
            {
                ObjectData existObj = oTable[c["Name"].ToString()] as ObjectData;
                existObj.chainObjs[c["TriggerObj"].ToString()] = int.Parse(c["TriggerIdx"].ToString());
                continue;
            }

            ObjectData objectData = new ObjectData(c["Name"].ToString());
            objectData.dialogueNum = int.Parse(c["DialogueNum"].ToString());
            for (int i = 0; i < objectData.dialogueNum; i++)
            {
                objectData.openDialouges.Add(false);
                objectData.completeDialogues.Add(false);
            }

            objectData.openDialouges[0] = true;

            oTable.Add(objectData.name, objectData);
            //Debug.Log("[" + objectData.name + "] 오브젝트 추가");
        }

    }
}
