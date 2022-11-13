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

                    GameObject eventfile = GameObject.Find("��� ���� �̹���");

                    go = Instantiate(outline, eventfile.transform);
                    go.GetComponent<RectTransform>().sizeDelta = new Vector2(500, 500);


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
}
