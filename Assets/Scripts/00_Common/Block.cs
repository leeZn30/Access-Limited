using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    [Header("�� ����")]
    public int blockIdx;
    public int blockorder = 0;
    public bool upChaining = false;
    public bool downChaining = false;
    [SerializeField] bool isStartBlock = false;

    [Header("�巡��")]
    [SerializeField] Vector3 startPosition;
    [SerializeField] Vector3 offset;

    [Header("�ִϸ��̼�")]
    [SerializeField] bool isWaiting = true;
    
    void Start()
    {
        // �ӽ�
        if (isStartBlock)
        {
            upChaining = true;
        }
            
    }

    void Update()
    {
        //StartCoroutine(swing());
        // �ӽ�
        if (upChaining)
        {
            GetComponent<Image>().color = Color.red;
        }
        else
        {
            GetComponent<Image>().color = Color.yellow;
        }
    }

    private void OnMouseDown()
    {
        if (!isStartBlock && !downChaining)
        {
            startPosition = transform.position;
            offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - startPosition;
        }
    }

    // ���� ���
    void OnMouseDrag()
    {
        if (!isStartBlock && !downChaining)
        {
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = objPosition - offset;
        }
    }

    void OnMouseUp()
    {
        if (!isStartBlock && !downChaining)
        {
            // Circle���� �� ������
            Block other = Physics2D.OverlapCircle(transform.position, 1f).GetComponent<Block>();
            Debug.Log("[Found Colider]: " + other.name);

            if (other == this)
            {
                upChaining = false;
                PackingManager.Instance.disconnectUpBlock(blockorder);
                blockorder = 0;

            }
            else if (other.tag == "Block" && other.upChaining && !other.downChaining)
            {
                // ����: y�� ���߱�
                // ���߿� Ƣ��� �κа� �� �κ��� position ���缭 �ű�ٰ� �ؾ��ҵ���
                float y =
                        other.gameObject.transform.position.y
                        - (other.transform.localScale.y / 2 + transform.localScale.y / 2);
                //- (other.GetComponent<RectTransform>().sizeDelta.y/2 + GetComponent<RectTransform>().sizeDelta.y/2);

                gameObject.transform.position = new Vector3(gameObject.transform.position.x, y);

                upChaining = true;
                other.downChaining = true;
                blockorder = other.blockorder + 1;

                PackingManager.Instance.insertIdx(this, blockorder);
            }

        }

    }


    void mergeBlock()
    {
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + Vector3.up * 0.5f, new Vector2(4, 1f));
    }

    /**
    // ����x �ִϸ��̼�
    IEnumerator swing()
    {
        float deg = 0;
        while (!isFixed)
        {

            deg += Time.deltaTime * 0.001f;
            if (deg < 360)
            {
                var rad = Mathf.Deg2Rad * (deg);
                var x = 2 * Mathf.Sin(rad);
                var y = 2 * Mathf.Cos(rad);

                transform.position = transform.position + new Vector3(x, y);
                transform.rotation = Quaternion.Euler(0, 0, deg * -1); //����� �ٶ󺸰� ���� ����

                yield return null;
            }
            else
            {
                deg = 0;
            }
        }
    }
    **/

}
