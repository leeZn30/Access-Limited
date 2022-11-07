using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractiveObject : MonoBehaviour
{
    [Header("나타나는 대화")]
    [SerializeField] List<TextAsset> csvs = new List<TextAsset>();
    [SerializeField] TextAsset lineCSV;
    [SerializeField] TextAsset defaultCSV;

    [Header("오브젝트를 통해 본 대화")]
    public int[] dialogues;
    //public bool[] completeDs;

    [Header("연계되는 대화")]
    public int[] previousDs;

    [Header("오브젝트 정보")]
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
            // 개복잡 실화냐
            // 선행되어야 하는 대화가 완료되었다면
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
                // 여기가 맞을까...
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
