using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapManager : Singleton<MapManager>
{
    [Header("맵 정보")]
    [SerializeField] int _nowMapIdx;
    [SerializeField] int mapCount;
    [SerializeField] List<Vector3> mapPos = new List<Vector3>();

    public int nowMapIdx
    {
        get { return _nowMapIdx; }
        set { _nowMapIdx = value; }
    }

    [Header("맵 전환 버튼")]
    [SerializeField] Button placeBtn;
    [SerializeField] bool isTranslateEnable = true;
    [SerializeField] bool isClicked = false;

    [Header("장소 리스트")]
    [SerializeField] List<Scene> places = new List<Scene>();

    [Header("오브젝트 상호작용")]
    public bool isInteractiveEnable = true;

    [Header("맵 슬라이딩")]
    public bool isSlidingEnable = true;
    [SerializeField] GameObject map;
    [SerializeField] GameObject mapLeft;
    [SerializeField] GameObject mapRight;

    [Header("코루틴")]
    [SerializeField] Coroutine currCo;

    void Awake()
    {
        placeBtn = GameObject.Find("이동버튼").GetComponent<Button>();
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

    // 맵 트랜스레이트
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
                // 아직 아님
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
