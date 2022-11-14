using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : Singleton<TutorialManager>
{
    [Header("튜토리얼 진행중")]
    [SerializeField] bool isTutorialRunning = false;

    [Header("푸시 알림")]
    [SerializeField] PushEffect push;

    [Header("프리팹")]
    [SerializeField] GameObject outline;

    public void startConnectTuto()
    {
        push.setText("D키를 눌러 데이터베이스를 켜주세요.");
        push.stayInfo();

        StartCoroutine(connectTutoCo());
    }

    IEnumerator connectTutoCo()
    {
        GameObject go = null;
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (!isTutorialRunning)
                {
                    isTutorialRunning = true;
                    push.setText("통신 연결을 확인하세요.");

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
        yield return null;
    }

    public void startFigureTuto()
    {
        push.setText("D를 눌러 데이터베이스를 켜주세요");
        push.stayInfo();

        StartCoroutine(figureTutoCo());
    }

    IEnumerator figureTutoCo()
    {
        GameObject go = null;
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

                    bool tmp = false;

                    // 너무 이상해
                    while (true)
                    {
                        if (DatabaseManager.Instance.now_rayout == 1)
                        {
                            if (!tmp)
                            {
                                push.setText("인물 파일을 클릭하세요.");
                                tmp = true;
                                go.transform.parent = GameObject.Find("인물파일 버튼").transform;
                                go.GetComponent<RectTransform>().sizeDelta = new Vector2(500, 300);

                                bool tmp2 = false;
                                while (true)
                                {
                                    if (DatabaseManager.Instance.now_rayout == 2)
                                    {
                                        if (!tmp2)
                                        {
                                            tmp2 = true;
                                            push.setText("인물 리스트를 클릭해서 정보를 확인하세요.");
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }

                            }
                            else
                                break;

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
        yield return null;
    }
}
