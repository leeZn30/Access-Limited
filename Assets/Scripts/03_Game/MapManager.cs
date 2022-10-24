using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Header("맵 정보")]
    [SerializeField] int nowMapIdx;
    [SerializeField] int mapCount;
    [SerializeField] List<Vector3> mapPos = new List<Vector3>();

    [Header("오브젝트")]
    [SerializeField] GameObject map;

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


    void goRight()
    {
        if (nowMapIdx < mapCount - 1)
        {
            nowMapIdx++;
            StartCoroutine(moveBackgroundCo(mapPos[nowMapIdx]));
        }
    }

    void goLeft()
    {
        if (nowMapIdx > 0)
        {
            nowMapIdx--;
            StartCoroutine(moveBackgroundCo(mapPos[nowMapIdx]));
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

        yield return null;

    }

}
