using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour
{
    [Header("�������� ����")]
    [SerializeField] int chapter;

    [Header("��� ����")]
    [SerializeField] int backgroundId;
    [SerializeField] Image background;


    // Start is called before the first frame update
    void Start()
    {
        background = GetComponentInChildren<Image>();
    }

    // ���� �ִϸ��̼� - �ڵ� �ߺ� = character�� (�ϳ��� ������ �����)
    IEnumerator fadeIn()
    {
        Color color = GetComponentInChildren<Image>().color;
        color.a = 0;

        while (color.a < 1f)
        {
            color.a += Time.deltaTime / 1f;
            GetComponentInChildren<Image>().color = color;
            yield return null;
        }

    }

    public void setBackground(int chapter, int id)
    {
        this.chapter = chapter;
        backgroundId = id;

        changeBackground();
    }


    public void changeBackground()
    {
        string path = "Images/Maps/" + chapter + "/" + backgroundId;
        background.sprite = Resources.Load<Sprite>(path);

        StartCoroutine(fadeIn());

    }


}
