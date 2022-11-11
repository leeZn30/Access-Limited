using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PushEffect : MonoBehaviour
{
    Coroutine co;

    public void setText(string t)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = t;
    }

    public void appearInfo()
    {
        if (co != null)
            StopCoroutine(co);

        gameObject.SetActive(true);
        co = StartCoroutine(appearCo());
    }
    IEnumerator appearCo()
    {

        yield return new WaitForSecondsRealtime(2f);

        dissapearInfo();
    }

    public void dissapearInfo()
    {
        gameObject.SetActive(false);
        //StartCoroutine(dissappearCo());
    }
    IEnumerator dissappearCo()
    {
        yield return null;
    }

    public void stayInfo()
    {

    }
}
