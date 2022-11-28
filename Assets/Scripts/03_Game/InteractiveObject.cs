using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractiveObject : MonoBehaviour
{
    [Header("������Ʈ ����")]
    public string o_name;
    public ObjectData objectData;
    public bool isChecked = false;

    [Header("���� ������Ʈ ����")]
    public Dictionary<string, int> chainDs = new Dictionary<string, int>();

    [Header("��Ÿ���� ��ȭ")]
    [SerializeField] List<TextAsset> csvs = new List<TextAsset>();
    //[SerializeField] List<TextAsset> defaultCSVs = new List<TextAsset>();
    [SerializeField] TextAsset defaultcsv;
    [SerializeField] TextAsset lineCSV;

    void Start()
    {
        objectData = ObjectTable.oTable[o_name] as ObjectData;

        //Debug.Log("[Object] " + objectData.name + "\n" + "nowDialogue: " + objectData.nowDialogue + "\ndialogueCount: " + objectData.dialogueList.Count);
    }

    /**
    void checkAllDialogue()
    {
        if (objectData.completeDialogues[objectData.dialogueList.Count - 1])
        {
            isAllDsChecked = true;
        }
    }
    **/

    void OnMouseDown()
    {
        if (MapManager.Instance.isInteractiveEnable)
        {
            if (!objectData.isChecked)
            {
                objectData.completeDialogue();

                lineCSV = csvs[objectData.nowDialogue];
                DialogueManager.Instance.resetDialogueManager(lineCSV);
                
                //isChecked = true;
                objectData.updateDialoge();

                // �ϴ� ����

            }
            else
            {
                /**
                lineCSV = defaultCSVs[objectData.nowDialogue];
                DialogueManager.Instance.resetDialogueManager(lineCSV);
                **/
                DialogueManager.Instance.resetDialogueManager(defaultcsv);
            }

        }
    }
}
