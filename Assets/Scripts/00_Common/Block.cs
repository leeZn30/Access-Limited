using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    [Header("블럭 정보")]
    public int blockIdx;
    public int blockorder = 0;
    public bool upChaining = false;
    public bool downChaining = false;
    [SerializeField] bool isStartBlock = false;

    [Header("드래그")]
    [SerializeField] Vector3 startPosition;
    [SerializeField] Vector3 offset;

    [Header("애니메이션")]
    [SerializeField] bool isWaiting = true;
    
    void Start()
    {
        // 임시
        if (isStartBlock)
        {
            upChaining = true;
        }
            
    }

    void Update()
    {
        //StartCoroutine(swing());
        // 임시
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

    // 스냅 기능
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
            // Circle말고 더 좋은거
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
                // 현재: y축 맞추기
                // 나중에 튀어나온 부분과 들어간 부분의 position 맞춰서 거기다가 해야할듯함
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
    // 고정x 애니메이션
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
                transform.rotation = Quaternion.Euler(0, 0, deg * -1); //가운데를 바라보게 각도 조절

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
