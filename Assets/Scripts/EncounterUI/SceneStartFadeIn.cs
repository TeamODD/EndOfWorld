using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneStartFadeIn : MonoBehaviour
{
    [SerializeField]
    private Image _fadeInImage;

    void Start()
    {
        _fadeInImage = this.gameObject.GetComponent<Image>();

        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float time = 1;

        while (time > 0)
        {
            _fadeInImage.color = new Color(_fadeInImage.color.r, _fadeInImage.color.g, _fadeInImage.color.b, time);
            time -= Time.deltaTime;
            yield return null;
        }

        _fadeInImage.color = new Color(_fadeInImage.color.r, _fadeInImage.color.g, _fadeInImage.color.b, 0);

        Destroy(this.gameObject);

        yield return null;
    }

}
