using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : Singleton<TutorialManager>
{
    [Header("튜토리얼 진행중")]
    [SerializeField] bool isTutorialRunning = false;
    Coroutine nowCo = null;

    [Header("푸시 알림")]
    [SerializeField] PushEffect push;

    [Header("프리팹")]
    [SerializeField] GameObject outline;

    [Header("Exit")]
    [SerializeField] Button exitbtn;
    GameObject go = null;

    void Start()
    {
        exitbtn.onClick.AddListener(delegate
        {
            if (nowCo != null)
                StopCoroutine(nowCo);
            isTutorialRunning = false;
            if (go != null)
                Destroy(go);
            push.dissapearInfo();
            TutorialDialogueManager.Instance.mission = 0;
        });
    }

    public void startConnectTuto()
    {
        push.setText("D키를 눌러 데이터베이스를 켜주세요.");
        push.stayInfo();

        nowCo = StartCoroutine(connectTutoCo());
    }

    IEnumerator connectTutoCo()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (!isTutorialRunning)
                {
                    isTutorialRunning = true;
                    push.setText("통신 연결을 확인하세요. 통신이 잘 이루어지고 있습니다.");

                    GameObject connectInfo = GameObject.Find("통신 연결 이미지");

                    go = Instantiate(outline, connectInfo.transform);
                    go.GetComponent<RectTransform>().sizeDelta = new Vector2(1700, 80);

                    yield return new WaitForSeconds(2f);

                    push.setText("D키를 누르거나, Exit 버튼을 클릭해 나가주세요.");
                }
                else
                {
                    push.dissapearInfo();
                    Destroy(go);
                    break;
                }
            }

            yield return null;
        }

        isTutorialRunning = false;
        TutorialDialogueManager.Instance.mission = 0;
        TutorialDialogueManager.Instance.missionRunning = false;
        yield return null;
    }

    public void startFigureTuto()
    {
        push.setText("D를 눌러 데이터베이스를 켜주세요");
        push.stayInfo();

        nowCo = StartCoroutine(figureTutoCo());
    }

    IEnumerator figureTutoCo()
    {
        GameObject go = null;

        bool figureCheck = false;
        bool privisoCheck = false;

        while (true)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (!isTutorialRunning)
                {
                    isTutorialRunning = true;
                    push.setText("사건 파일을 클릭하세요.");

                    GameObject eventfile = GameObject.Find("사건 파일 버튼");

                    go = Instantiate(outline, eventfile.transform);
                    go.GetComponent<RectTransform>().sizeDelta = new Vector2(500, 500);

                    // 너무 이상해
                    while (true)
                    {
                        if (DatabaseManager.Instance.now_rayout == 1)
                        {
                            Destroy(go);
                            if (!figureCheck || !privisoCheck)
                            {
                                push.setText("인물 파일이나 단서를 클릭하세요.");

                                while (true)
                                {
                                    if (DatabaseManager.Instance.now_rayout == 2)
                                    {
                                        if (!figureCheck)
                                        {
                                            figureCheck = true;
                                            push.setText("인물 리스트를 클릭해서 정보를 확인하세요.");
                                        }
                                        else break;
                                    }
                                    else if (DatabaseManager.Instance.now_rayout == 4)
                                    {
                                        if (!privisoCheck)
                                        {
                                            privisoCheck = true;
                                            push.setText("단서를 클릭해서 정보를 확인하세요.");
                                        }
                                        else break;
                                    }
                                    yield return null;
                                }
                            }
                            else
                            {
                                push.setText("데이터는 상시 갱신됩니다.");
                                break;
                            }

                        }
                        yield return null;
                    }
                }
                else
                {
                    push.dissapearInfo();
                    if (go != null)
                        Destroy(go);
                    break;
                }
            }

            yield return null;
        }

        isTutorialRunning = false;
        TutorialDialogueManager.Instance.mission = 0;
        TutorialDialogueManager.Instance.missionRunning = false;
        yield return null;
    }

    public void startMapSliding()
    {
        push.setText("슬라이딩 가능한 방향에 표시가 납니다. 화살키를 눌러 이동합니다.");
        push.stayInfo();

        GameObject moveRight = GameObject.Find("mapRight");
        go = Instantiate(outline, moveRight.transform);
        go.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);

        nowCo = StartCoroutine(mapSliding());
    }

    IEnumerator mapSliding()
    {
        isTutorialRunning = true;

        yield return new WaitForSeconds(2f);

        MapManager.Instance.goRight();
        //go.transform.parent = GameObject.Find("mapLeft").transform;

        yield return new WaitForSeconds(2f);

        MapManager.Instance.goLeft();

        Destroy(go);
        isTutorialRunning = false;
        push.dissapearInfo();
        TutorialDialogueManager.Instance.mission = 0;
        TutorialDialogueManager.Instance.missionRunning = false;
    }

}
