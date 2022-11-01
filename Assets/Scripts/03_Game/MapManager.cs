using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    [Header("�� ����")]
    [SerializeField] int nowMapIdx;
    [SerializeField] int mapCount;
    [SerializeField] List<Vector3> mapPos = new List<Vector3>();

    [Header("������Ʈ ��ȣ�ۿ�")]
    public bool isInteractiveEnable = true;

    [Header("������Ʈ")]
    [SerializeField] GameObject map;

    [Header("�ڷ�ƾ")]
    [SerializeField] Coroutine currCo;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            goRight();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            goLeft();
        }
    }

    public void onInteractiveObject()
    {
        if (!isInteractiveEnable)
        {
            isInteractiveEnable = true;
        }
    }

    public void offInteractiveObject()
    {
        if (isInteractiveEnable)
        {
            isInteractiveEnable = false;
        }
    }


    void goRight()
    {
        if (nowMapIdx < mapCount - 1)
        {
            if (currCo != null)
                StopCoroutine(currCo);
            nowMapIdx++;
            currCo = StartCoroutine(moveBackgroundCo(mapPos[nowMapIdx]));
        }
    }

    void goLeft()
    {
        if (nowMapIdx > 0)
        {
            if (currCo != null)
                StopCoroutine(currCo);

            nowMapIdx--;
            currCo = StartCoroutine(moveBackgroundCo(mapPos[nowMapIdx]));
        }

    }

    IEnumerator moveBackgroundCo(Vector3 targetPos)
    {
        float duration = 0;

        while (duration < 1f)
        {
            duration += Time.deltaTime;
            map.transform.position = Vector3.Lerp(map.transform.position, targetPos, Time.deltaTime / 0.2f);
            yield return null;
        }
        map.transform.position = targetPos;

        currCo = null;
        yield return null;

    }

}
