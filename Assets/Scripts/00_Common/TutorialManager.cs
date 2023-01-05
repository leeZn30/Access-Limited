using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : Singleton<TutorialManager>
{
    [Header("Ʃ�丮�� ������")]
    [SerializeField] bool isTutorialRunning = false;
    Coroutine nowCo = null;

    [Header("Ǫ�� �˸�")]
    [SerializeField] PushEffect push;

    [Header("������")]
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
        push.setText("DŰ�� ���� �����ͺ��̽��� ���ּ���.");
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
                    push.setText("��� ������ Ȯ���ϼ���. ����� �� �̷������ �ֽ��ϴ�.");

                    GameObject connectInfo = GameObject.Find("��� ���� �̹���");

                    go = Instantiate(outline, connectInfo.transform);
                    go.GetComponent<RectTransform>().sizeDelta = new Vector2(1700, 80);

                    yield return new WaitForSeconds(2f);

                    push.setText("DŰ�� �����ų�, Exit ��ư�� Ŭ���� �����ּ���.");
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
        push.setText("D�� ���� �����ͺ��̽��� ���ּ���");
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
                    push.setText("��� ������ Ŭ���ϼ���.");

                    GameObject eventfile = GameObject.Find("��� ���� ��ư");

                    go = Instantiate(outline, eventfile.transform);
                    go.GetComponent<RectTransform>().sizeDelta = new Vector2(500, 500);

                    // �ʹ� �̻���
                    while (true)
                    {
                        if (DatabaseManager.Instance.now_rayout == 1)
                        {
                            Destroy(go);
                            if (!figureCheck || !privisoCheck)
                            {
                                push.setText("�ι� �����̳� �ܼ��� Ŭ���ϼ���.");

                                while (true)
                                {
                                    if (DatabaseManager.Instance.now_rayout == 2)
                                    {
                                        if (!figureCheck)
                                        {
                                            figureCheck = true;
                                            push.setText("�ι� ����Ʈ�� Ŭ���ؼ� ������ Ȯ���ϼ���.");
                                        }
                                        else break;
                                    }
                                    else if (DatabaseManager.Instance.now_rayout == 4)
                                    {
                                        if (!privisoCheck)
                                        {
                                            privisoCheck = true;
                                            push.setText("�ܼ��� Ŭ���ؼ� ������ Ȯ���ϼ���.");
                                        }
                                        else break;
                                    }
                                    yield return null;
                                }
                            }
                            else
                            {
                                push.setText("�����ʹ� ��� ���ŵ˴ϴ�.");
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
        push.setText("�����̵� ������ ���⿡ ǥ�ð� ���ϴ�. ȭ��Ű�� ���� �̵��մϴ�.");
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
