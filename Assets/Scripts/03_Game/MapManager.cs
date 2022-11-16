using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapManager : Singleton<MapManager>
{
    [Header("�� ����")]
    [SerializeField] int _nowMapIdx;
    [SerializeField] int mapCount;
    [SerializeField] List<Vector3> mapPos = new List<Vector3>();

    public int nowMapIdx
    {
        get { return _nowMapIdx; }
        set { _nowMapIdx = value; }
    }

    [Header("�� ��ȯ ��ư")]
    [SerializeField] Button placeBtn;
    [SerializeField] bool isTranslateEnable = true;
    [SerializeField] bool isClicked = false;

    [Header("��� ����Ʈ")]
    [SerializeField] List<Scene> places = new List<Scene>();

    [Header("������Ʈ ��ȣ�ۿ�")]
    public bool isInteractiveEnable = true;

    [Header("�� �����̵�")]
    public bool isSlidingEnable = true;
    [SerializeField] GameObject map;
    [SerializeField] GameObject mapLeft;
    [SerializeField] GameObject mapRight;

    [Header("�ڷ�ƾ")]
    [SerializeField] Coroutine currCo;

    void Awake()
    {
        placeBtn = GameObject.Find("�̵���ư").GetComponent<Button>();
        mapLeft = GameObject.Find("mapLeft");
        mapRight = GameObject.Find("mapRight");
    }

    void Start()
    {
        checkMapIdx();
        placeBtn.onClick.AddListener(openClosePlaces);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && isSlidingEnable)
        {
            goRight();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && isSlidingEnable)
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

    void checkMapIdx()
    {
        if (nowMapIdx - 1 >= 0)
            mapLeft.SetActive(true);
        else
            mapLeft.SetActive(false);

        if (nowMapIdx + 1 < mapCount)
            mapRight.SetActive(true);
        else
            mapRight.SetActive(false);
    }


    public void goRight()
    {
        if (nowMapIdx < mapCount - 1)
        {
            if (currCo != null)
                StopCoroutine(currCo);

            nowMapIdx++;
            checkMapIdx();
            currCo = StartCoroutine(moveBackgroundCo(mapPos[nowMapIdx]));
        }
    }

    public void goLeft()
    {
        if (nowMapIdx > 0)
        {
            if (currCo != null)
                StopCoroutine(currCo);

            nowMapIdx--;
            checkMapIdx();
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

    // �� Ʈ��������Ʈ
    public void onPlaceTranslator()
    {
        placeBtn.gameObject.SetActive(true);
        isTranslateEnable = true;
    }

    public void offPlaceTranslator()
    {
        placeBtn.gameObject.SetActive(false);
        isTranslateEnable = false;
    }

    void openClosePlaces()
    {
        if (!isClicked && isTranslateEnable)
        {

            foreach (Scene p in places)
            {
                Button btn = ObjectPool.Instance.PlaceQueue.Dequeue().GetComponent<Button>();
                btn.gameObject.SetActive(true);
                // ���� �ƴ�
                btn.onClick.AddListener(delegate { SceneManager.LoadScene(p.name); });
            }
            isClicked = true;

        }
        else
        {
            GameObject[] places = GameObject.FindGameObjectsWithTag("PlaceBtn");
            foreach (GameObject b in places)
            {
                b.SetActive(false);
                ObjectPool.Instance.PlaceQueue.Enqueue(b);
            }
            isClicked = false;

        }
    }

}
