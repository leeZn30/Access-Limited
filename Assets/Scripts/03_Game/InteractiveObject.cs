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
    bool isMouseOver = false;


    void Start()
    {
        //GetComponent<BoxCollider2D>().size = GetComponent<SpriteRenderer>().sprite.bounds.size;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            objectClick();
        }
    }

    void objectClick()
    {
        Vector3 positon = Input.mousePosition;

        Ray cast = Camera.main.ScreenPointToRay(positon);


        // Mouse�� �������� Ray cast �� ��ȯ
        RaycastHit hit;

        if (Physics.Raycast(cast, out hit))
        {
            if (hit.collider.tag == "Object")
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
    }

    /**
    void OnMouseEnter()
    {
        if (!isMouseOver)
        {
            isMouseOver = true;
        }
    }

    void OnMouseExit()
    {
        if (isMouseOver)
        {
            isMouseOver = false;
        }
    }

    void OnMouseDown()
    {
        if (isMouseOver)
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
    **/
}
