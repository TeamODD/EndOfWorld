using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneFadeInManager : MonoBehaviour
{
    [SerializeField]
    Image _fadeOutImage;

    [SerializeField]
    TouchManager _touchManager;

    public void FadeInAndSceneTransmition()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float time = 0;

        while (time < 1)
        {
            _fadeOutImage.color = new Color(_fadeOutImage.color.r, _fadeOutImage.color.g, _fadeOutImage.color.b, time);
            time += Time.deltaTime;
            yield return null;
        }

        _fadeOutImage.color = new Color(_fadeOutImage.color.r, _fadeOutImage.color.g, _fadeOutImage.color.b, 1);

        yield return new WaitForSeconds(0.5f);

        _touchManager.SceneTransmition();

        yield return null;
    }
}
