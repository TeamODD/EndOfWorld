using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FadeManager : MonoBehaviour
{
    [SerializeField]
    TMP_Text _textComponent;



    private void Start()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn()
    {
        float time = 1;

        while (time > 0)
        {
            _textComponent.color = new Color(1, 1, 1, time);
            time -= Time.deltaTime;
            yield return null;
        }

        _textComponent.color = new Color(1, 1, 1, 0);

        StartCoroutine(FadeOut());
        yield return null;
    }

    IEnumerator FadeOut()
    {
        float time = 0;

        while (time < 1)
        {
            _textComponent.color = new Color(1, 1, 1, time);
            time += Time.deltaTime;
            yield return null;
        }

        _textComponent.color = new Color(1, 1, 1, 1);

        StartCoroutine(FadeIn());
        yield return null;
    }

}
