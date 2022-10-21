using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    [Header("������Ʈ ����")]
    public string objectId;
    public bool isChecked = false;
    [SerializeField] TextAsset lineCSV;
    [SerializeField] TextAsset defaultCSV;


    [Header("���̾�α� �Ŵ���")]
    [SerializeField] GameObject dialogueUIs;

    void Start()
    {
    }

    void OnMouseDown()
    {
        dialogueUIs.SetActive(true);

        if (!isChecked)
        {
            DialogueManager.Instance.setDialogueManager(lineCSV);
            isChecked = true;
        }
        else
        {
            DialogueManager.Instance.setDialogueManager(defaultCSV);
        }
    }
}
