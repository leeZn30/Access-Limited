using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour
{
    [Header("스테이지 정보")]
    [SerializeField] int chapter;

    [Header("배경 정보")]
    [SerializeField] int backgroundId;
    [SerializeField] Image background;


    // Start is called before the first frame update
    void Start()
    {
        background = GetComponentInChildren<Image>();
    }

    // 등장 애니메이션 - 코드 중복 = character랑 (하나의 폴더로 만들까)
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
