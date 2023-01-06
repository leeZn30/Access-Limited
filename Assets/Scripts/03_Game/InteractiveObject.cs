using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class InteractiveObject : MonoBehaviour
{
    [Header("������Ʈ ����")]
    public string o_name;
    public int o_type;
    public ObjectData objectData;
    [SerializeField] GameObject selection;

    [Header("���� ������Ʈ ����")]
    // dictionary�� �ϰ� ������ inspector���� �ٲ���� �ϹǷ�
    [SerializeField] List<string> chainObjs = new List<string>();
    [SerializeField] List<int> chainDs = new List<int>();

    [Header("��Ÿ���� ��ȭ")]
    [SerializeField] List<TextAsset> essentialCsvs = new List<TextAsset>();
    [SerializeField] TextAsset defaultcsv;
    [SerializeField] TextAsset lineCSV;

    void Start()
    {
        objectData = ObjectTable.oTable[o_name] as ObjectData;
    }

    void OnMouseDown()
    {
        if (MapManager.Instance.isInteractiveEnable)
        {
            GameManager.Instance.clickedObj = this;

            switch (o_type)
            {
                // �ι� -> �ܼ� ���� ����
                case 1:
                    Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    pos.z = 10f;
                    Instantiate(
                                selection,
                                pos,
                                Quaternion.identity,
                                FindObjectOfType<Canvas>().transform
                                );
                    MapManager.Instance.offInteractiveObject();
                    break;

                // �Ϲ���
                default:
                    startDialogue();
                    break;
            }

        }
    }

    public void startDialogue()
    {
        if (!objectData.isChecked)
        {
            DialogueManager.Instance.resetDialogueManager(essentialCsvs[objectData.nowDialogue], 1);
        }
        else
        {
            DialogueManager.Instance.resetDialogueManager(defaultcsv);
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
