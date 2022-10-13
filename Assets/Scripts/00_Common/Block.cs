using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Block : MonoBehaviour
{
    [Header("드래그")]
    [SerializeField] Vector3 startPosition;

    [Header("애니메이션")]
    [SerializeField] bool isFixed = false;

    void Update()
    {
        //StartCoroutine(swing());
    }

    private void OnMouseDown()
    {
        startPosition = transform.position;
    }

    // 스냅 기능
    void OnMouseDrag()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10); 
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition); 
        transform.position = objPosition;
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
