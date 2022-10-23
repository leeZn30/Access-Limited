using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    [Header("오브젝트 정보")]
    public string objectId;
    public bool isChecked = false;
    [SerializeField] TextAsset lineCSV;
    [SerializeField] TextAsset defaultCSV;


    void Start()
    {
        //GetComponent<BoxCollider2D>().size = GetComponent<SpriteRenderer>().sprite.bounds.size;
    }

    void OnMouseDown()
    {
        if (!isChecked)
        {
            DialogueManager.Instance.resetDialogueManager(lineCSV);
            isChecked = true;
        }
        else
        {
            DialogueManager.Instance.resetDialogueManager(defaultCSV);
        }
    }
}
