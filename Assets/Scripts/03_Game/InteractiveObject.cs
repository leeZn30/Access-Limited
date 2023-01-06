using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class InteractiveObject : MonoBehaviour
{
    [Header("오브젝트 정보")]
    public string o_name;
    public int o_type;
    public ObjectData objectData;
    [SerializeField] GameObject selection;

    [Header("연쇄 오브젝트 정보")]
    // dictionary로 하고 싶으나 inspector에서 바꿔줘야 하므로
    [SerializeField] List<string> chainObjs = new List<string>();
    [SerializeField] List<int> chainDs = new List<int>();

    [Header("나타나는 대화")]
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
                // 인물 -> 단서 제출 가능
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

                // 일반적
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
     * 오브젝트 대화를 본 후, 업데이트 하는 것들
     * 오브젝트 클릭 후 바로 적용하면 안되기 때문에
     * 
     * 호출은 dialgoueManager에서 mode에 따라 호출
     */

    public void afterDialogueUpdate()
    {
        // ChainObjs는 현재 오브젝트에서 각 대화를 열고나서 연쇄되는 오브젝트 이름
        // ChainDs는 연쇄된 오브젝트에서 몇번째 대화를 열 것인지
        try
        {
            ObjectData target = ObjectTable.oTable[chainObjs[objectData.nowDialogue]] as ObjectData;
            target.openDialogue(chainDs[objectData.nowDialogue]);
        }
        catch (ArgumentException)
        {
            Debug.Log("연쇄 대화 없음");
        }

        objectData.completeDialogue();
    }
}
