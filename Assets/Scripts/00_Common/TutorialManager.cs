using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : Singleton<TutorialManager>
{
    [Header("Ʃ�丮�� ������")]
    [SerializeField] bool isTutorialRunning = false;

    [Header("Ǫ�� �˸�")]
    [SerializeField] PushEffect push;

    [Header("������")]
    [SerializeField] GameObject outline;

    public void startConnectTuto()
    {
        push.setText("DŰ�� ���� �����ͺ��̽��� ���ּ���.");
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
                    push.setText("��� ������ Ȯ���ϼ���.");

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
        yield return null;
    }

    public void startFigureTuto()
    {
        push.setText("D�� ���� �����ͺ��̽��� ���ּ���");
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
                    push.setText("��� ������ Ŭ���ϼ���.");

                    GameObject eventfile = GameObject.Find("��� ���� ��ư");

                    go = Instantiate(outline, eventfile.transform);
                    go.GetComponent<RectTransform>().sizeDelta = new Vector2(500, 500);

                    bool tmp = false;

                    // �ʹ� �̻���
                    while (true)
                    {
                        if (DatabaseManager.Instance.now_rayout == 1)
                        {
                            if (!tmp)
                            {
                                push.setText("�ι� ������ Ŭ���ϼ���.");
                                tmp = true;
                                go.transform.parent = GameObject.Find("�ι����� ��ư").transform;
                                go.GetComponent<RectTransform>().sizeDelta = new Vector2(500, 300);

                                bool tmp2 = false;
                                while (true)
                                {
                                    if (DatabaseManager.Instance.now_rayout == 2)
                                    {
                                        if (!tmp2)
                                        {
                                            tmp2 = true;
                                            push.setText("�ι� ����Ʈ�� Ŭ���ؼ� ������ Ȯ���ϼ���.");
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
