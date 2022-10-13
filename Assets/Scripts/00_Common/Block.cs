using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Block : MonoBehaviour
{
    [Header("�巡��")]
    [SerializeField] Vector3 startPosition;

    [Header("�ִϸ��̼�")]
    [SerializeField] bool isFixed = false;

    void Update()
    {
        //StartCoroutine(swing());
    }

    private void OnMouseDown()
    {
        startPosition = transform.position;
    }

    // ���� ���
    void OnMouseDrag()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10); 
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition); 
        transform.position = objPosition;
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
