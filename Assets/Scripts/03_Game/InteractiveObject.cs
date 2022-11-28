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

    [Header("연쇄 오브젝트 정보")]
    public List<string> chainDs = new List<string>();
    //public Dictionary<string, int> chainDs = new Dictionary<string, int>();

    [Header("나타나는 대화")]
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

                // 실화냐 너무 복잡함
                // 일단 여기 => 나중에 대화 다 끝나고 모드별로 처리해야할듯
                if (chainDs[objectData.nowDialogue] != null)
                {
                    ObjectData triggerObj = ObjectTable.oTable[chainDs[objectData.nowDialogue]] as ObjectData;
                    triggerObj.openDialogue(objectData.chainObjs[chainDs[objectData.nowDialogue]]);
                }

                //isChecked = true;
                objectData.updateDialoge();

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
