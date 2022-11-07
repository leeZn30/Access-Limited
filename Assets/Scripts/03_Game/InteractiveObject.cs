using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractiveObject : MonoBehaviour
{
    [Header("��Ÿ���� ��ȭ")]
    [SerializeField] List<TextAsset> csvs = new List<TextAsset>();
    [SerializeField] TextAsset lineCSV;
    [SerializeField] TextAsset defaultCSV;

    [Header("������Ʈ�� ���� �� ��ȭ")]
    public int[] dialogues;
    //public bool[] completeDs;

    [Header("����Ǵ� ��ȭ")]
    public int[] previousDs;

    [Header("������Ʈ ����")]
    public string objectId;
    public int nowDialogue = 0;
    public bool isChecked = false;


    void Start()
    {
    }

    void Update()
    {
    }

    /**
    public void updateDialogues()
    {

        if (previousDs != null)
        {
            // ������ ��ȭ��
            // ����Ǿ�� �ϴ� ��ȭ�� �Ϸ�Ǿ��ٸ�
            if (DayManager.Instance.d_completes[previousDs[nowDialogue]])
            {
                nowDialogue++;
                lineCSV = csvs[nowDialogue];
            }
        }
    }
    **/


    void OnMouseDown()
    {
        if (MapManager.Instance.isInteractiveEnable)
        {
            if (!isChecked)
            {
                lineCSV = DayManager.Instance.dialogues[dialogues[nowDialogue]];
                DialogueManager.Instance.resetDialogueManager(lineCSV);
                
                isChecked = true;
                /**
                // ���Ⱑ ������...
                DayManager.Instance.d_completes[dialogues[nowDialogue]] = true;
                updateDialogues();
                **/
            }
            else
            {
                DialogueManager.Instance.resetDialogueManager(defaultCSV);
            }

        }
    }
}
