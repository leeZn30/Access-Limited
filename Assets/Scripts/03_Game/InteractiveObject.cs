using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

    void Update()
    {
        /**
        if (Input.GetMouseButtonDown(0))
            objectClick();
        **/
    }

    void objectClick()
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Mouse의 포지션을 Ray cast 로 변환
        RaycastHit2D[] hit = Physics2D.RaycastAll(position, transform.forward, 1f);
        //RaycastHit2D hit = Physics2D.Raycast(position, transform.forward, 1f);

        for(int i = 0; i < hit.Length; i++)
        {
            RaycastHit2D r = hit[i];
            Debug.Log("[Tag "+ i + "]: " + r.collider.tag + " [Name]: " + r.collider.gameObject.name);
        }

        /**
        if (hit[0].collider.tag == "Object" && hit[0].collider.gameObject == gameObject)
        {
            Debug.Log("[Tag]: " + hit[0].collider.tag + " [Name]: " + hit[0].collider.gameObject.name);

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
        **/
    }

    void OnMouseDown()
    {
        if (MapManager.Instance.isInteractiveEnable)
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
