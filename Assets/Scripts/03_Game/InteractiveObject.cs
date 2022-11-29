using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class InteractiveObject : MonoBehaviour
{
    [Header("������Ʈ ����")]
    public string o_name;
    public ObjectData objectData;

    [Header("���� ������Ʈ ����")]
    // dictionary�� �ϰ� ������ inspector���� �ٲ���� �ϹǷ�
    [SerializeField] List<string> chainObjs = new List<string>();
    [SerializeField] List<int> chainDs = new List<int>();
    //public Dictionary<string, int> chainDs = new Dictionary<string, int>();

    [Header("��Ÿ���� ��ȭ")]
    [SerializeField] List<TextAsset> csvs = new List<TextAsset>();
    //[SerializeField] List<TextAsset> defaultCSVs = new List<TextAsset>();
    [SerializeField] TextAsset defaultcsv;
    [SerializeField] TextAsset lineCSV;

    void Start()
    {
        objectData = ObjectTable.oTable[o_name] as ObjectData;
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
                GameManager.Instance.clickedObj = this;

                lineCSV = csvs[objectData.nowDialogue];
                DialogueManager.Instance.resetDialogueManager(lineCSV, 1);
            }
            else
            {
                DialogueManager.Instance.resetDialogueManager(defaultcsv);
            }

        }
    }

    /* 
     * ������Ʈ ��ȭ�� �� ��, ������Ʈ �ϴ� �͵�
     * ������Ʈ Ŭ�� �� �ٷ� �����ϸ� �ȵǱ� ������
     * 
     * ȣ���� dialgoueManager���� mode�� ���� ȣ��
     */

    public void afterDialogueUpdate()
    {
        // ChainObjs�� ���� ������Ʈ���� �� ��ȭ�� ������ ����Ǵ� ������Ʈ �̸�
        // ChainDs�� ����� ������Ʈ���� ���° ��ȭ�� �� ������
        try
        {
            ObjectData target = ObjectTable.oTable[chainObjs[objectData.nowDialogue]] as ObjectData;
            target.openDialogue(chainDs[objectData.nowDialogue]);
        }
        catch (ArgumentException)
        {
            Debug.Log("���� ��ȭ ����");
        }

        objectData.completeDialogue();
    }
}
