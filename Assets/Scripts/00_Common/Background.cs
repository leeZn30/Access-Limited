using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour
{
    [Header("�������� ����")]
    [SerializeField] int chapter;

    [Header("��� ����")]
    [SerializeField] string backgroundId;
    [SerializeField] int mode;


    // Start is called before the first frame update
    void Start()
    {
        chapter = GameData.Instance.chapter;
    }

    // ���� �ִϸ��̼� - �ڵ� �ߺ� = character�� (�ϳ��� ������ �����)
    IEnumerator fadeIn()
    {
        Color color = GetComponent<SpriteRenderer>().color;
        color.a = 0;

        while (color.a < 1f)
        {
            color.a += Time.deltaTime / 1f;
            GetComponent<SpriteRenderer>().color = color;
            yield return null;
        }

    }

    public void setBackground(int chapter, string id, int mode)
    {
        Debug.Log("Change!");
        this.chapter = chapter;
        backgroundId = id;
        this.mode = mode;

        switch (mode)
        {
            case 0:
                Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("Object");
                break;
            case 1:
                Camera.main.cullingMask = Camera.main.cullingMask & ~(1 << LayerMask.NameToLayer("Object"));
                break;
            default:
                break;
        }

        changeBackground();
    }


    public void changeBackground()
    {
        string path = "Images/Maps/" + chapter + "/" + backgroundId;
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(path);

        StartCoroutine(fadeIn());

    }


}
