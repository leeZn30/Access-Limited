using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractiveObject : MonoBehaviour
{
    [Header("오브젝트 정보")]
    public string o_name;
    public ObjectData objectData;
    public bool isChecked = false;

    [Header("나타나는 대화")]
    [SerializeField] List<TextAsset> csvs = new List<TextAsset>();
    [SerializeField] TextAsset lineCSV;
    [SerializeField] TextAsset defaultCSV;

    void Start()
    {
        objectData = ObjectTable.oTable[o_name] as ObjectData;

        Debug.Log("[Object] " + objectData.name + "\n" + "nowDialogue: " + objectData.nowDialogue + "\ndialogueCount: " + objectData.dialogueList.Count);
    }

    void Update()
    {
    }

    void OnMouseDown()
    {
        if (MapManager.Instance.isInteractiveEnable)
        {
            if (!isChecked)
            {
                objectData.completeDialogue();

                lineCSV = csvs[objectData.nowDialogue];
                DialogueManager.Instance.resetDialogueManager(lineCSV);
                
                //isChecked = true;
                objectData.updateDialoge();
            }
            else
            {
                DialogueManager.Instance.resetDialogueManager(defaultCSV);
            }

        }
    }
}
